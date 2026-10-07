using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace CadBridgeDesigner.Tool.Tool.CadTool.EntityTool
{
    /// <summary>
    /// 数据库工具类，提供CAD数据库相关的通用操作方法
    /// </summary>
    public static class DataBaseTool
    {
        /// <summary>
        /// 根据ObjectId获取数据库对象的克隆副本
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="objectId">要克隆的数据库对象的ObjectId</param>
        /// <returns>克隆后的数据库对象副本，失败则返回null</returns>
        public static DBObject GetDbObjectCloneByObjectId(Document document, ObjectId objectId)
        {
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    DBObject dbObject = transaction.GetObject(objectId, OpenMode.ForRead);
                    if (dbObject != null)
                    {
                        DBObject dbObjectClone = dbObject.Clone() as DBObject;
                        return dbObjectClone;
                    }
                    return null;
                }
            }
        }
        
        public static void RemoveDbObjectByObjectId(Document document, ObjectId objectId)
        {
            if (!objectId.IsValid || objectId.IsErased) return;
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    DBObject dbObject = transaction.GetObject(objectId, OpenMode.ForWrite);
                    if (dbObject != null)
                    {
                        dbObject.Erase();
                    }
                    transaction.Commit();
                }
            }
        }
    }
}