using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using CadBridgeDesigner.Tool.Bridge.Base;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool;
using CadBridgeDesigner.Tool.Tool.CadTool.EntityTool;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;

namespace CadBridgeDesigner.Tool.Tool.CadTool.InteractTool
{
    /// <summary>
    /// 拖动Jig类，用于在移动过程中实时预览实体
    /// </summary>
    public class DrawJigInMove : DrawJig
    {
        public readonly DBObjectCollection Entities;
        private readonly Point3d _basePoint;
        private Point3d _currentPoint;
        public Vector3d Displacement;
        public DrawJigInMove(DBObjectCollection entities, Point3d basePoint)
        {
            Entities = entities;
            _basePoint = basePoint;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions jigPromptPointOptions = new JigPromptPointOptions
            {
                UserInputControls = UserInputControls.Accept3dCoordinates,
                Message = "\n选择插入位置",
                BasePoint = _basePoint,
                UseBasePoint = true
            };

            PromptPointResult result = prompts.AcquirePoint(jigPromptPointOptions);

            if (result.Status == PromptStatus.OK)
            {
                _currentPoint = result.Value;
                Displacement = _currentPoint - _basePoint;
                return SamplerStatus.OK;
            }
            return SamplerStatus.Cancel;
        }
        protected override bool WorldDraw(WorldDraw draw)
        {
            WorldGeometry drawGeometry = draw.Geometry;
            if (drawGeometry == null) return true;

            drawGeometry.PushModelTransform(Matrix3d.Displacement(Displacement));
            for (int i = 0; i < Entities.Count; i++)
            {
                Entity entity = (Entity)Entities[i];
                drawGeometry.Draw(entity);
            }
            drawGeometry.PopModelTransform();
            return true;
        }
    }

    /// <summary>
    /// Jig工具类，提供与Jig交互相关的静态方法
    /// </summary>
    public static class JigTool
    {
        /// <summary>
        /// 将多个实体添加到数据库，并在添加前通过Jig让用户拖动确定插入位置
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="entities">要添加的实体集合（未添加到数据库的临时实体）</param>
        public static void AddEntityToDataBaseWithDrawJigInMove(Document document, DBObjectCollection entities)
        {
            Database database = document.Database;
            Editor editor = document.Editor;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    if (blockTable == null) return;
                    BlockTableRecord modelSpace = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (modelSpace == null) return;

                    // 先把实体临时写入模型空间，使其成为数据库驻留对象（着色视图下才能被正确渲染）
                    foreach (Entity entity in entities)
                    {
                        modelSpace.AppendEntity(entity);
                        transaction.AddNewlyCreatedDBObject(entity, true);
                    }

                    DrawJigInMove jigInMove = new DrawJigInMove(entities, Point3d.Origin);
                    PromptResult jigResult = editor.Drag(jigInMove);
                    Vector3d displacement = jigInMove.Displacement;

                    if (jigResult.Status == PromptStatus.OK)
                    {
                        foreach (Entity entity in jigInMove.Entities)
                        {
                            entity.TransformBy(Matrix3d.Displacement(displacement));
                        }
                        transaction.Commit();
                    }
                    else
                    {
                        // 取消：临时入库的实体随事务回滚自动丢弃
                        transaction.Abort();
                    }
                }
            }
        }

        public static void AddBridgeComponentDiagramInJigInMove(Document document, BridgeComponentDiagram bridgeComponentDiagram)
        {
            DBObjectCollection entities = new DBObjectCollection();
            foreach (Polyline polyline in bridgeComponentDiagram.CenterCurves)
            {
                polyline.LayerId = LayerTool.GetLayerId(document, "CENTER");
                entities.Add(polyline);
            }
            foreach (Polyline polyline in bridgeComponentDiagram.StructureCurves)
            {
                polyline.LayerId = LayerTool.GetLayerId(document, "STRUCTURE_CONTINUOUS");
                entities.Add(polyline);
            }
            foreach (Polyline polyline in bridgeComponentDiagram.DashedCurves)
            {
                polyline.LayerId = LayerTool.GetLayerId(document, "STRUCTURE_DASHED");
                entities.Add(polyline);
            }
            foreach (Polyline polyline in bridgeComponentDiagram.HatchCurves)
            {
                polyline.LayerId = LayerTool.GetLayerId(document, "STRUCTURE_FILL");
                Hatch hatch = polyline.TranslateToHatch();
                entities.Add(polyline);
                entities.Add(hatch);
            }
            foreach (RotatedDimension rotatedDimension in bridgeComponentDiagram.Dimensions)
            {
                rotatedDimension.LayerId = LayerTool.GetLayerId(document, "DIM");
                rotatedDimension.DimensionStyle = DimStyleTool.GetDimStyleId(document, "标注样式");
                entities.Add(rotatedDimension);
            }
            
            Database database = document.Database;
            Editor editor = document.Editor;
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForWrite) as BlockTable;
                    if (blockTable == null)
                    {
                        LogerTool.Log("[AddTbeamCommand]块表为空, 添加失败");
                        return;
                    }
                    DrawJigInMove jigInMove = new DrawJigInMove(entities, Point3d.Origin);
                    PromptResult jigResult = editor.Drag(jigInMove);
                    if (jigResult.Status != PromptStatus.OK)
                    {
                        LogerTool.Log("[AddTbeamCommand]用户取消操作, 添加失败");
                        return;
                    }
                    BlockTableRecord modelSpace = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (modelSpace == null)
                    {
                        LogerTool.Log("[AddTbeamCommand]模型空间为空, 添加失败");
                        return;
                    }

                    foreach (Entity entity in entities)
                    {
                        ModifyTool.MoveByVector(entity, new Vector3d(jigInMove.Displacement.X, jigInMove.Displacement.Y, jigInMove.Displacement.Z));
                        modelSpace.AppendEntity(entity);
                        transaction.AddNewlyCreatedDBObject(entity, true);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}