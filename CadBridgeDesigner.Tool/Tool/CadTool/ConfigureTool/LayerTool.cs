using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Tool.Loger;

namespace CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool
{
    /// <summary>
    /// 图层工具类，提供CAD图层的增删查操作
    /// </summary>
    public static class LayerTool
    {
        /// <summary>
        /// 初始化图层
        /// </summary>
        /// <param name="document"></param>
        public static void InitLayer(Document document)
        {
            LogerTool.Log("[InitLayer]初始化图层");
            
            LayerTableRecord modelSpace3DLayer = new LayerTableRecord();
            modelSpace3DLayer.Name = "MODEL_SPACE_3D";
            modelSpace3DLayer.Color = Color.FromColorIndex(ColorMethod.ByAci, 9);
            modelSpace3DLayer.LinetypeObjectId = LineTypeTool.GetLineTypeId(document, "CONTINUOUS");
            
            LayerTableRecord structureContinuousLayer = new LayerTableRecord();
            structureContinuousLayer.Name = "STRUCTURE_CONTINUOUS";
            structureContinuousLayer.Color = Color.FromColorIndex(ColorMethod.ByAci, 2);
            structureContinuousLayer.LinetypeObjectId = LineTypeTool.GetLineTypeId(document, "CONTINUOUS");
            
            LayerTableRecord structureDashedLayer = new LayerTableRecord();
            structureDashedLayer.Name = "STRUCTURE_DASHED";
            structureDashedLayer.Color = Color.FromColorIndex(ColorMethod.ByAci, 2);
            structureDashedLayer.LinetypeObjectId = LineTypeTool.GetLineTypeId(document, "DASHED");
            
            LayerTableRecord structureFillLayer = new LayerTableRecord();
            structureFillLayer.Name = "STRUCTURE_FILL";
            structureFillLayer.Color = Color.FromColorIndex(ColorMethod.ByAci, 7);
            structureFillLayer.LinetypeObjectId = LineTypeTool.GetLineTypeId(document, "CONTINUOUS");
            
            LayerTableRecord centerLayer = new LayerTableRecord();
            centerLayer.Name = "CENTER";
            centerLayer.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);
            centerLayer.LinetypeObjectId = LineTypeTool.GetLineTypeId(document, "CENTER");
            
            LayerTableRecord dimensionLayer = new LayerTableRecord();
            dimensionLayer.Name = "DIM";
            dimensionLayer.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);
            dimensionLayer.LinetypeObjectId = LineTypeTool.GetLineTypeId(document, "CONTINUOUS");
            
            AddLayer(document, modelSpace3DLayer);
            AddLayer(document, structureContinuousLayer);
            AddLayer(document, structureDashedLayer);
            AddLayer(document, structureFillLayer);
            AddLayer(document, centerLayer);
            AddLayer(document, dimensionLayer);
        }
        
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="layerTableRecord"></param>
        private static void AddLayer(Document document, LayerTableRecord layerTableRecord)
        {
            if (QueryLayer(document, layerTableRecord.Name))
            {
                return;
            }
            Database database = document.Database;
            
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
                    if (layerTable == null)
                    {
                        return;
                    }
                    layerTable.UpgradeOpen();
                    layerTable.Add(layerTableRecord);
                    transaction.AddNewlyCreatedDBObject(layerTableRecord, true);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// 删除图层
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="layer">要删除的图层名称</param>
        public static void DeleteLayer(Document document, string layer)
        {
            Database database = document.Database;
            using (document.LockDocument())
            { 
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
                    if (layerTable == null)
                    {
                        return;
                    }
                    if (layerTable.Has(layer))
                    {
                        layerTable.UpgradeOpen();
                        ObjectId layerId = layerTable[layer];
                        LayerTableRecord layerTableRecord = transaction.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                        if (layerTableRecord == null)
                        {
                            return;
                        }
                        layerTableRecord.Erase();
                        transaction.AddNewlyCreatedDBObject(layerTableRecord, true);
                    }
                }
            }
        }

        /// <summary>
        /// 查询是否存在指定图层
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="layer">要查询的图层名称</param>
        /// <returns>存在返回true，不存在返回false</returns>
        private static Boolean QueryLayer(Document document, string layer)
        {
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction()) 
                {
                    LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
                    if (layerTable == null){ return false; }
                    if (layerTable.Has(layer)) { return true; }
                    return false;
                }
            }
        }
        /// <summary>
        /// 查询图层ID
        /// </summary>
        /// <param name="document"></param>
        /// <param name="layer"></param>
        /// <returns>成功返回图层ID，失败返回ObjectId.Null</returns>
        public static ObjectId GetLayerId(Document document, string layer)
        {
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
                    if (layerTable == null)
                    {
                        transaction.Commit();
                        return ObjectId.Null;
                    }

                    if (layerTable.Has(layer))
                    {
                        transaction.Commit();
                        return layerTable[layer];
                    }
                    transaction.Commit();
                    return ObjectId.Null;
                }
            }
        }
   }
}