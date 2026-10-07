using System;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using CadBridgeDesigner.Tool.Bridge.Base;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier.View;
using CadBridgeDesigner.Tool.BridgeManager;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool;
using CadBridgeDesigner.Tool.Tool.CadTool.EntityTool;
using CadBridgeDesigner.Tool.Tool.CadTool.InteractTool;

namespace CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier
{
    public static class ColumnPierTool
    {
        private static DBObjectCollection GetColumnPierEntities(this ColumnPierModel columnPierModel)
        {
            DBObjectCollection bridgePierEntities = new DBObjectCollection();
            
            double lengthOfPierCap = columnPierModel.LengthOfPierCap;
            double widthOfPierCap = columnPierModel.WidthOfPierCap;
            double heightOfPierCap = columnPierModel.HeightOfPierCap;
            double lengthOfPierCapSlope = columnPierModel.LengthOfPierCapSlope;
            double heightOfPierCapSlope = columnPierModel.HeightOfPierCapSlope;
            
            double lengthOfShearKey = columnPierModel.LengthOfShearKey;
            double widthOfShearKey = columnPierModel.WidthOfShearKey;
            double heightOfShearKey = columnPierModel.HeightOfShearKey;
            double gapOfShearKey = columnPierModel.GapOfShearKey;
            
            double rowOfBearing = columnPierModel.RowOfBearing;
            double rowSpaceOfBearing = columnPierModel.RowSpaceOfBearing;
            double columnOfBearing = columnPierModel.ColumnOfBearing;
            double columnSpaceOfBearings = columnPierModel.ColumnSpaceOfBearing;
            double lengthOfBearingPedestal = columnPierModel.LengthOfBearingPedestal;
            double heightOfBearingPedestal = columnPierModel.HeightOfBearingPedestal;
            double diameterOfRubberGasket = columnPierModel.DiameterOfRubberGasket;
            double heightOfRubberGasket = columnPierModel.HeightOfRubberGasket;
            
            int numberOfPierColumns = columnPierModel.NumberOfPierColumn;
            double spaceOfPierColumn = columnPierModel.SpaceOfPierColumn;
            double heightOfPierColumn = columnPierModel.HeightOfPierColumn;
            double diameterOfPierColumn = columnPierModel.DiameterOfPierColumn;
            
            double numberOfTieBeam = columnPierModel.NumberOfTieBeam;
            double spaceOfTieBeam = columnPierModel.SpaceOfTieBeam;
            double widthOfTieBeam = columnPierModel.WidthOfTieBeam;
            double heightOfTieBeam = columnPierModel.HeightOfTieBeam;
            
            double lengthOfFoundation = columnPierModel.LengthOfFoundation;
            double widthOfFoundation = columnPierModel.WidthOfFoundation;
            double heightOfFoundation = columnPierModel.HeightOfFoundation;
            
            int rowOfPile = columnPierModel.RowOfPile;
            int columnOfPile = columnPierModel.ColumnOfPile;
            double rowSpaceOfPile = columnPierModel.RowSpaceOfPile;
            double columnSpaceOfPile = columnPierModel.ColumnSpaceOfPile;
            double diameterOfPile = columnPierModel.DiameterOfPile;
            double heightOfPile = columnPierModel.HeightOfPile;
            
            // 盖梁
            Polyline columnPierCapPolyline = new Polyline();
            columnPierCapPolyline.AddVertexAt(columnPierCapPolyline.NumberOfVertices, new Point2d(- lengthOfPierCap / 2, heightOfPierCap), 0, 0, 0);
            columnPierCapPolyline.AddVertexAt(columnPierCapPolyline.NumberOfVertices, new Point2d(lengthOfPierCap / 2, heightOfPierCap), 0, 0, 0);
            columnPierCapPolyline.AddVertexAt(columnPierCapPolyline.NumberOfVertices, new Point2d(lengthOfPierCap / 2, heightOfPierCap - heightOfPierCapSlope), 0, 0, 0);
            columnPierCapPolyline.AddVertexAt(columnPierCapPolyline.NumberOfVertices, new Point2d(lengthOfPierCap / 2 - lengthOfPierCapSlope, 0), 0, 0, 0);
            columnPierCapPolyline.AddVertexAt(columnPierCapPolyline.NumberOfVertices, new Point2d(-(lengthOfPierCap / 2 - lengthOfPierCapSlope), 0), 0, 0, 0);
            columnPierCapPolyline.AddVertexAt(columnPierCapPolyline.NumberOfVertices, new Point2d(-lengthOfPierCap / 2, heightOfPierCap - heightOfPierCapSlope), 0, 0, 0);
            columnPierCapPolyline.Closed = true;
            Region columnPierCapRegion = columnPierCapPolyline.TranslateToRegion();
            Solid3d columnPierCapSolid = SolidTool.ExtrudeSection(columnPierCapRegion, widthOfPierCap, true);
            ModifyTool.MoveByVector(columnPierCapSolid, new Vector3d(0, -(heightOfPierCap + heightOfBearingPedestal + heightOfRubberGasket), 0));
            ModifyTool.RotateEntityByAngle(columnPierCapSolid, Point3d.Origin, Vector3d.XAxis, Math.PI / 2);
            bridgePierEntities.Add(columnPierCapSolid);
            
            // 抗震挡块
            Polyline rightShearKeyPolyline = new Polyline();
            rightShearKeyPolyline.AddVertexAt(rightShearKeyPolyline.NumberOfVertices, new Point2d(0, 0), 0, 0, 0);
            rightShearKeyPolyline.AddVertexAt(rightShearKeyPolyline.NumberOfVertices, new Point2d(lengthOfShearKey, 0), 0, 0, 0);
            rightShearKeyPolyline.AddVertexAt(rightShearKeyPolyline.NumberOfVertices, new Point2d(lengthOfShearKey, heightOfShearKey), 0, 0, 0);
            rightShearKeyPolyline.AddVertexAt(rightShearKeyPolyline.NumberOfVertices, new Point2d(0, heightOfShearKey), 0, 0, 0);
            rightShearKeyPolyline.Closed = true;
            Region rightShearKeyRegion = rightShearKeyPolyline.TranslateToRegion();
            Solid3d rightShearKeySolid = SolidTool.ExtrudeSection(rightShearKeyRegion, widthOfShearKey, true);
            ModifyTool.MoveByVector(rightShearKeySolid, new Vector3d(columnSpaceOfBearings * (columnOfBearing - 1) / 2 + gapOfShearKey - lengthOfShearKey / 2, -(heightOfBearingPedestal + heightOfRubberGasket), 0));
            ModifyTool.RotateEntityByAngle(rightShearKeySolid, Point3d.Origin, Vector3d.XAxis, Math.PI / 2);
            Solid3d leftShearKeySolid = rightShearKeySolid.Clone() as Solid3d;
            ModifyTool.MirrorEntityByPlane(leftShearKeySolid, new Plane(Point3d.Origin, Vector3d.XAxis));
            bridgePierEntities.Add(leftShearKeySolid);
            bridgePierEntities.Add(rightShearKeySolid);
            
            // 支座
            for (int i = 0; i < columnOfBearing; i++)
            {
                for (int j = 0; j < rowOfBearing; j++)
                {
                    Solid3d bearingPedestalSolid = new Solid3d();
                    bearingPedestalSolid.CreateBox(lengthOfBearingPedestal, lengthOfBearingPedestal, heightOfBearingPedestal);
                    ModifyTool.MoveByVector(bearingPedestalSolid, new Vector3d(0,0, -(heightOfRubberGasket / 2 + heightOfBearingPedestal)));
                    Solid3d rubberGasketSolid = new Solid3d();
                    rubberGasketSolid.CreateFrustum(heightOfRubberGasket, diameterOfRubberGasket / 2, diameterOfRubberGasket / 2, diameterOfRubberGasket / 2);
                    ModifyTool.MoveByVector(rubberGasketSolid, new Vector3d(0,0, -(heightOfRubberGasket / 2)));
                    double x = -columnSpaceOfBearings * (columnOfBearing - 1) / 2 + columnSpaceOfBearings * i;
                    double y = -rowSpaceOfBearing * (rowOfBearing - 1) / 2 + rowSpaceOfBearing * j;
                    ModifyTool.MoveByVector(bearingPedestalSolid, new Vector3d(x, y, 0));
                    ModifyTool.MoveByVector(rubberGasketSolid, new Vector3d(x, y, 0));
                    bridgePierEntities.Add(bearingPedestalSolid);
                    bridgePierEntities.Add(rubberGasketSolid);
                }
            }
            
            // 桥墩柱
            for (int j = 0; j < numberOfPierColumns; j++)
            {
                Solid3d columnPierColumnSolid = new Solid3d();
                columnPierColumnSolid.CreateFrustum(heightOfPierColumn, diameterOfPierColumn / 2, diameterOfPierColumn / 2, diameterOfPierColumn / 2);
                ModifyTool.MoveByVector(columnPierColumnSolid, new Vector3d(0, 0, -(heightOfRubberGasket + heightOfBearingPedestal + heightOfPierCap + heightOfPierColumn / 2)));
                double x = -spaceOfPierColumn * (numberOfPierColumns - 1) / 2 + spaceOfPierColumn * j;
                ModifyTool.MoveByVector(columnPierColumnSolid, new Vector3d(x, 0, 0));
                bridgePierEntities.Add(columnPierColumnSolid);
            }
            
            // 系梁
            for (int i = 0; i < numberOfPierColumns - 1; i++)
            {
                for (int j = 0; j < numberOfTieBeam; j++)
                {
                    Solid3d tieBeamSolid = new Solid3d();
                    tieBeamSolid.CreateBox(spaceOfPierColumn, widthOfTieBeam, heightOfTieBeam);
                    ModifyTool.MoveByVector(tieBeamSolid, new Vector3d(0, 0, -(heightOfRubberGasket + heightOfBearingPedestal + heightOfPierCap + heightOfTieBeam / 2)));
                    double x = -spaceOfPierColumn * numberOfPierColumns / 2 + spaceOfPierColumn * i + spaceOfPierColumn;
                    double z = -(j + 1) * spaceOfTieBeam;
                    ModifyTool.MoveByVector(tieBeamSolid, new Vector3d(x, 0, z));
                    bridgePierEntities.Add(tieBeamSolid);
                }
            }
            
            
            // 承台
            Solid3d columnPierFoundationSolid = new Solid3d();
            columnPierFoundationSolid.CreateBox(lengthOfFoundation, widthOfFoundation, heightOfFoundation);
            ModifyTool.MoveByVector(columnPierFoundationSolid, new Vector3d(0, 0, -(heightOfRubberGasket + heightOfBearingPedestal + heightOfPierCap + heightOfPierColumn + heightOfFoundation / 2)));
            bridgePierEntities.Add(columnPierFoundationSolid);
            
            // 桥桩
            for (int i = 0; i < rowOfPile; i++)
            {
                for (int j = 0; j < columnOfPile; j++)
                {
                    Solid3d pileSolid = new Solid3d();
                    pileSolid.CreateFrustum(heightOfPile, diameterOfPile / 2, diameterOfPile / 2, diameterOfPile / 2);
                    ModifyTool.MoveByVector(pileSolid, new Vector3d(0, 0, -(heightOfRubberGasket + heightOfBearingPedestal + heightOfPierCap + heightOfPierColumn + heightOfFoundation + heightOfPile / 2)));
                    double x = -columnSpaceOfPile * (columnOfPile - 1) / 2 + columnSpaceOfPile * j;
                    double y = -rowSpaceOfPile * (rowOfPile - 1) / 2 + rowSpaceOfPile * i;
                    ModifyTool.MoveByVector(pileSolid, new Vector3d(x, y, 0));
                    bridgePierEntities.Add(pileSolid);
                }
            }
            return bridgePierEntities;
        }
        private static ResultBuffer GetColumnPierResultBuffer(this ColumnPierModel columnPierModel)
        {
            ResultBuffer resultBuffer = new ResultBuffer();
            resultBuffer.Add(new TypedValue((int)DxfCode.Text, columnPierModel.Name));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.Mileage));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.LengthOfPierCap));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.WidthOfPierCap));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfPierCap));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.LengthOfPierCapSlope));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfPierCapSlope));

            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.LengthOfShearKey));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.WidthOfShearKey));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfShearKey));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.GapOfShearKey));

            resultBuffer.Add(new TypedValue((int)DxfCode.Int16, columnPierModel.RowOfBearing));
            resultBuffer.Add(new TypedValue((int)DxfCode.Int16, columnPierModel.ColumnOfBearing));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.RowSpaceOfBearing));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.ColumnSpaceOfBearing));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.LengthOfBearingPedestal));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfBearingPedestal));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.DiameterOfRubberGasket));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfRubberGasket));

            resultBuffer.Add(new TypedValue((int)DxfCode.Int16, columnPierModel.NumberOfPierColumn));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.SpaceOfPierColumn));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfPierColumn));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.DiameterOfPierColumn));

            resultBuffer.Add(new TypedValue((int)DxfCode.Int16, columnPierModel.NumberOfTieBeam));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.SpaceOfTieBeam));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.WidthOfTieBeam));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfTieBeam));

            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.LengthOfFoundation));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.WidthOfFoundation));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfFoundation));

            resultBuffer.Add(new TypedValue((int)DxfCode.Int16, columnPierModel.RowOfPile));
            resultBuffer.Add(new TypedValue((int)DxfCode.Int16, columnPierModel.ColumnOfPile));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.RowSpaceOfPile));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.ColumnSpaceOfPile));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.DiameterOfPile));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, columnPierModel.HeightOfPile));
            
            return resultBuffer;
        }
        public static ColumnPierModel GetColumnPierModel(Document document, ObjectId blockTableRecordId)
        {
            ResultBuffer typeResultBuffer = BlockTableRecordTool.GetExtensionDictionaryValue(document, blockTableRecordId, "TYPE");
            if (typeResultBuffer == null)
            {
                LogerTool.Log("[GetColumnPierModel]块定义不存在TYPE,返回null");
                return null;
            }
            if ((string)typeResultBuffer.AsArray()[0].Value != "COLUMN_PIER")
            {
                LogerTool.Log("[GetColumnPierModel]块定义不是COLUMN_PIER,返回null");
                return null;
            }
            ResultBuffer infoResultBuffer = BlockTableRecordTool.GetExtensionDictionaryValue(document, blockTableRecordId, "INFO");
            if (infoResultBuffer == null)
            {
                LogerTool.Log("[GetColumnPierModel]块定义不存在INFO,返回null");
                return null;
            }
            TypedValue[] dataArray = infoResultBuffer.AsArray();
            int index = 0;
            
            string name = Convert.ToString(dataArray[index].Value); index++;
            double mileage = Convert.ToDouble(dataArray[index].Value); index++;
            
            double lengthOfPierCap = Convert.ToDouble(dataArray[index].Value); index++;
            double widthOfPierCap = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfPierCap = Convert.ToDouble(dataArray[index].Value); index++;
            double lengthOfPierCapSlope = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfPierCapSlope = Convert.ToDouble(dataArray[index].Value); index++;

            double lengthOfShearKey = Convert.ToDouble(dataArray[index].Value); index++;
            double widthOfShearKey = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfShearKey = Convert.ToDouble(dataArray[index].Value); index++;
            double gapOfShearKey = Convert.ToDouble(dataArray[index].Value); index++;

            int rowOfBearing = Convert.ToInt32(dataArray[index].Value); index++;
            int columnOfBearing = Convert.ToInt32(dataArray[index].Value); index++;
            double rowSpaceOfBearing = Convert.ToDouble(dataArray[index].Value); index++;
            double columnSpaceOfBearing = Convert.ToDouble(dataArray[index].Value); index++;
            double lengthOfBearingPedestal = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfBearingPedestal = Convert.ToDouble(dataArray[index].Value); index++;
            double diameterOfRubberGasket = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfRubberGasket = Convert.ToDouble(dataArray[index].Value); index++;

            int numberOfPierColumn = Convert.ToInt32(dataArray[index].Value); index++;
            double spaceOfPierColumn = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfPierColumn = Convert.ToDouble(dataArray[index].Value); index++;
            double diameterOfPierColumn = Convert.ToDouble(dataArray[index].Value); index++;

            int numberOfTieBeam = Convert.ToInt32(dataArray[index].Value); index++;
            double spaceOfTieBeam = Convert.ToDouble(dataArray[index].Value); index++;
            double widthOfTieBeam = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfTieBeam = Convert.ToDouble(dataArray[index].Value); index++;

            double lengthOfFoundation = Convert.ToDouble(dataArray[index].Value); index++;
            double widthOfFoundation = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfFoundation = Convert.ToDouble(dataArray[index].Value); index++;

            int rowOfPile = Convert.ToInt32(dataArray[index].Value); index++;
            int columnOfPile = Convert.ToInt32(dataArray[index].Value); index++;
            double rowSpaceOfPile = Convert.ToDouble(dataArray[index].Value); index++;
            double columnSpaceOfPile = Convert.ToDouble(dataArray[index].Value); index++;
            double diameterOfPile = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfPile = Convert.ToDouble(dataArray[index].Value);

            ColumnPierModel bridgeColumnPierModel = new ColumnPierModel
            {
                Type = BridgeComponentType.ColumnPier,
                Name = name,
                Mileage = mileage,
                BlockTableRecordId = blockTableRecordId,
                BlockTableRecordName = BlockTableRecordTool.GetBlockTableRecordByBlockTableRecordId(document, blockTableRecordId).Name,

                LengthOfPierCap = lengthOfPierCap,
                WidthOfPierCap = widthOfPierCap,
                HeightOfPierCap = heightOfPierCap,
                LengthOfPierCapSlope = lengthOfPierCapSlope,
                HeightOfPierCapSlope = heightOfPierCapSlope,

                LengthOfShearKey = lengthOfShearKey,
                WidthOfShearKey = widthOfShearKey,
                HeightOfShearKey = heightOfShearKey,
                GapOfShearKey = gapOfShearKey,

                RowOfBearing = rowOfBearing,
                ColumnOfBearing = columnOfBearing,
                RowSpaceOfBearing = rowSpaceOfBearing,
                ColumnSpaceOfBearing = columnSpaceOfBearing,
                LengthOfBearingPedestal = lengthOfBearingPedestal,
                HeightOfBearingPedestal = heightOfBearingPedestal,
                DiameterOfRubberGasket = diameterOfRubberGasket,
                HeightOfRubberGasket = heightOfRubberGasket,

                NumberOfPierColumn = numberOfPierColumn,
                SpaceOfPierColumn = spaceOfPierColumn,
                HeightOfPierColumn = heightOfPierColumn,
                DiameterOfPierColumn = diameterOfPierColumn,
                
                NumberOfTieBeam = numberOfTieBeam,
                SpaceOfTieBeam = spaceOfTieBeam,
                WidthOfTieBeam = widthOfTieBeam,
                HeightOfTieBeam = heightOfTieBeam,

                LengthOfFoundation = lengthOfFoundation,
                WidthOfFoundation = widthOfFoundation,
                HeightOfFoundation = heightOfFoundation,

                RowOfPile = rowOfPile,
                ColumnOfPile = columnOfPile,
                RowSpaceOfPile = rowSpaceOfPile,
                ColumnSpaceOfPile = columnSpaceOfPile,
                DiameterOfPile = diameterOfPile,
                HeightOfPile = heightOfPile,
                BlockReferenceIdCollection = BlockTableRecordTool.GerBlockReferenceIdCollection(document, blockTableRecordId)
            };
            return bridgeColumnPierModel;
        }
        public static void ColumnPierUpdate(Document document, ColumnPierModel columnPierModel)
        {
            if (columnPierModel == null)
            {
                LogerTool.Log("桥墩模型不合法,更新失败");
                return;
            }
            if (columnPierModel.BlockTableRecordId.IsNull)
            {
                LogerTool.Log("ID不合法,更新失败");
                return;
            }
            Editor editor = document.Editor;
            DBObjectCollection newEntities = columnPierModel.GetColumnPierEntities();
            BlockTableRecordTool.ChangeEntity(document, columnPierModel.BlockTableRecordId, newEntities);
            ResultBuffer resultBuffer = columnPierModel.GetColumnPierResultBuffer();
            BlockTableRecordTool.ChangeExtensionDictionary(document, columnPierModel.BlockTableRecordId, "INFO", resultBuffer);
            BridgeManagerTool.BridgeManagerUpdate(document);
            editor.Regen();
        }
        public static void AddColumnPierCommand(Document document)
        {
            ColumnPierModel columnPierModel = new ColumnPierModel();
            ColumnPierAddViewModel columnPierAddViewModel = new ColumnPierAddViewModel(columnPierModel);
            ColumnPierAddView columnPierAddView = new ColumnPierAddView(columnPierAddViewModel)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 250,
                Height = 500
            };
            Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowModalWindow(Autodesk.AutoCAD.ApplicationServices.Core.Application.MainWindow.Handle, columnPierAddView, false );
            if (columnPierAddViewModel.Result is false)
            {
                LogerTool.Log("用户取消操作,添加失败");
                return;
            }
            columnPierModel = columnPierAddViewModel.ColumnPierModel;
            columnPierModel.BlockTableRecordName = $"柱式墩_{DateTime.Now:yyyyMMddHHmmssfff}";
            Database database = document.Database;
            DBObjectCollection bridgePierEntities = columnPierModel.GetColumnPierEntities();
            Editor editor = document.Editor;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForWrite) as BlockTable;
                    if (blockTable == null)
                    {
                        LogerTool.Log("块表为空，添加失败");
                        return;
                    }
                    DrawJigInMove jigInMove = new DrawJigInMove(bridgePierEntities, Point3d.Origin);
                    PromptResult jigResult = editor.Drag(jigInMove);
                    if (jigResult.Status != PromptStatus.OK)
                    {
                        LogerTool.Log("用户取消操作,添加失败");
                        return;
                    }
                    BlockTableRecord columnPierBlockTableRecord = new BlockTableRecord();
                    columnPierBlockTableRecord.Name = columnPierModel.BlockTableRecordName;
                    columnPierBlockTableRecord.Origin = Point3d.Origin;
                    foreach (Entity entity in jigInMove.Entities)
                    {
                        entity.SetLayerId(LayerTool.GetLayerId(document, "MODEL_SPACE_3D"), true);
                        columnPierBlockTableRecord.AppendEntity(entity);
                    }
                    ObjectId columnPierBlockTableRecordId = blockTable.Add(columnPierBlockTableRecord);
                    BlockTableRecordTool.AddExtensionDictionary(document, columnPierBlockTableRecordId, "TYPE", new ResultBuffer(new TypedValue((int)DxfCode.Text, "COLUMN_PIER")));
                    BlockTableRecordTool.AddExtensionDictionary(document, columnPierBlockTableRecordId, "INFO", columnPierModel.GetColumnPierResultBuffer());
                    BlockTableRecord modelSpace = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (modelSpace == null)
                    {
                        LogerTool.Log("模型空间为空，添加失败");
                        return;
                    }
                    BlockReference blockReference = new BlockReference(new Point3d(jigInMove.Displacement.X, jigInMove.Displacement.Y, jigInMove.Displacement.Z), columnPierBlockTableRecordId);
                    blockReference.SetLayerId(LayerTool.GetLayerId(document, "MODEL_SPACE_3D"), true);
                    modelSpace.AppendEntity(blockReference);
                    transaction.AddNewlyCreatedDBObject(columnPierBlockTableRecord, true);
                    transaction.AddNewlyCreatedDBObject(blockReference, true);
                    transaction.Commit();
                    BridgeManagerTool.BridgeManagerUpdate(document);
                }
            }
        }
        public static void AddColumnPierInstance(this ColumnPierModel columnPierModel, Document document)
        {
            Database database = document.Database;
            DBObjectCollection pierEntities = columnPierModel.GetColumnPierEntities();
            Editor editor = document.Editor;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForWrite) as BlockTable;
                    if (blockTable == null)
                    {
                        LogerTool.Log("[AddColumnPierInstance]块表为空, 添加失败");
                        return;
                    }
                    DrawJigInMove jigInMove = new DrawJigInMove(pierEntities, Point3d.Origin);
                    PromptResult jigResult = editor.Drag(jigInMove);
                    if (jigResult.Status != PromptStatus.OK)
                    {
                        LogerTool.Log("[AddColumnPierInstance]用户取消操作, 添加失败");
                        return;
                    }
                    BlockTableRecord modelSpace = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (modelSpace == null)
                    {
                        LogerTool.Log("[AddColumnPierInstance]模型空间为空, 添加失败");
                        return;
                    }
                    BlockReference blockReference = new BlockReference( new Point3d(jigInMove.Displacement.X, jigInMove.Displacement.Y, jigInMove.Displacement.Z), columnPierModel.BlockTableRecordId);
                    blockReference.SetLayerId(LayerTool.GetLayerId(document, "MODEL_SPACE_3D"), true);
                    modelSpace.AppendEntity(blockReference);
                    transaction.AddNewlyCreatedDBObject(blockReference, true);
                    transaction.Commit();
                    BridgeManagerTool.BridgeManagerUpdate(document);
                }
            }
        }
    }
}