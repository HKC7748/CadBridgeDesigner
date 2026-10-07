using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Tool.Loger;

namespace CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool
{
    public static class LineTypeTool
    {
        /// <summary>
        /// 初始化线型
        /// </summary>
        /// <param name="document"></param>
        public static void InitLineType(Document document)
        {
            LogerTool.Log("[InitLineType]初始化线型");
            AddLineType(document, "CONTINUOUS");
            AddLineType(document, "CENTER");
            AddLineType(document, "DASHED");
        }
        /// <summary>
        /// 添加线型
        /// </summary>
        /// <param name="document"></param>
        /// <param name="lineTypeName"></param>
        private static void AddLineType(Document document, string lineTypeName)
        {
            if (QueryLineType(document, lineTypeName))
            {
                return;
            }
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LinetypeTable lineTypeTable = transaction.GetObject(database.LinetypeTableId, OpenMode.ForWrite) as LinetypeTable;

                    if (lineTypeTable == null)
                    {
                        return;
                    }
                    database.LoadLineTypeFile(lineTypeName, "acadiso.lin");
                    transaction.Commit();
                }
            }
        }
        /// <summary>
        /// 查询是否存在指定的线型
        /// </summary>
        /// <param name="document"></param>
        /// <param name="lineTypeName"></param>
        /// <returns>存在返回true，不存在返回false</returns>
        private static bool QueryLineType(Document document, string lineTypeName)
        {
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LinetypeTable lineTypeTable = transaction.GetObject(database.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;

                    if (lineTypeTable == null)
                    {
                        return false;
                    }
                        

                    if (lineTypeTable.Has(lineTypeName))
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
        /// <summary>
        /// 查询线型ID
        /// </summary>
        /// <param name="document"></param>
        /// <param name="lineTypeName"></param>
        /// <returns>成功返回线型ID，失败返回ObjectId.Null</returns>
        public static ObjectId GetLineTypeId(Document document, string lineTypeName)
        {
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LinetypeTable lineTypeTable = transaction.GetObject(database.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;

                    if (lineTypeTable == null)
                    {
                        transaction.Commit();
                        return ObjectId.Null;
                    }
                    if (lineTypeTable.Has(lineTypeName))
                    {
                        transaction.Commit();
                        return lineTypeTable[lineTypeName];
                    }
                    transaction.Commit();
                    return ObjectId.Null;
                }
            }
        }
    }
}


