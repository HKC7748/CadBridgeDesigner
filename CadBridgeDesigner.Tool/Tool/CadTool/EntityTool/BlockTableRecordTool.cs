using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace CadBridgeDesigner.Tool.Tool.CadTool.EntityTool
{
    public static class BlockTableRecordTool
    {
        public static BlockTableRecord GetBlockTableRecordByBlockReferenceId(Document document, ObjectId blockReferenceId)
        {
            if (blockReferenceId.IsNull)
            {
                return null;
            }
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockReference blockReference = transaction.GetObject(blockReferenceId, OpenMode.ForRead) as BlockReference;
                    if (blockReference == null)
                    {
                        return null;
                    }
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockReference.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                    if (blockTableRecord == null)
                    {
                        return null;
                    }
                    return blockTableRecord;
                }
            }
        }
        
        public static BlockTableRecord GetBlockTableRecordByBlockTableRecordId(Document document, ObjectId blockTableRecordId)
        {
            if (blockTableRecordId.IsNull)
            {
                return null;
            }
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTableRecordId, OpenMode.ForRead) as BlockTableRecord;
                    if (blockTableRecord == null)
                    {
                        return null;
                    }
                    return blockTableRecord;
                }
            }
        }

        public static void ChangeEntity(Document document, ObjectId blockTableRecordId, DBObjectCollection entities)
        {
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTableRecordId, OpenMode.ForWrite) as BlockTableRecord;
                    if (blockTableRecord == null)
                    {
                        return;
                    }

                    DBObjectCollection oldEntities = new DBObjectCollection();
                    foreach (ObjectId objectId in blockTableRecord)
                    {
                        DBObject dbObject = transaction.GetObject(objectId, OpenMode.ForWrite);
                        if (dbObject is BlockBegin)
                            continue;
                        if (dbObject is BlockEnd)
                            continue;
                        if (dbObject is AttributeDefinition)
                            continue;
                        Entity entity = dbObject as Entity;
                        if (entity != null)
                        {
                            oldEntities.Add(entity);
                        }
                    }
                    foreach (Entity oldEntity in oldEntities)
                    {
                        oldEntity.Erase();
                    }
                    foreach (Entity entity in entities)
                    {
                        blockTableRecord.AppendEntity(entity);
                        transaction.AddNewlyCreatedDBObject(entity, true);
                    }

                    ObjectIdCollection blockReferenceIds = blockTableRecord.GetBlockReferenceIds(true, true);
                    foreach (ObjectId blockReferenceId in blockReferenceIds)
                    {
                        BlockReference blockReference = transaction.GetObject(blockReferenceId, OpenMode.ForWrite) as BlockReference;
                        if (blockReference == null)
                            continue;
                        blockReference.RecordGraphicsModified(true);
                    }
                    transaction.Commit();
                }
                document.Editor.Regen();
            }
        }
        
        public static void AddExtensionDictionary(Document document, ObjectId blockTableRecordId, string dictionaryName, ResultBuffer resultBuffer)
        {
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTableRecordId, OpenMode.ForWrite) as BlockTableRecord;
                    if (blockTableRecord == null)
                    {
                        return;
                    }
                    if (blockTableRecord.ExtensionDictionary.IsNull)
                    {
                        blockTableRecord.CreateExtensionDictionary();
                    }
                    DBDictionary extensionDictionary = transaction.GetObject(blockTableRecord.ExtensionDictionary, OpenMode.ForWrite) as DBDictionary;
                    if (extensionDictionary == null)
                    {
                        return;
                    }
                    Xrecord xrecord = new Xrecord();
                    xrecord.Data = resultBuffer;
                    extensionDictionary.SetAt(dictionaryName, xrecord);
                    transaction.AddNewlyCreatedDBObject(xrecord, true);
                    transaction.Commit();
                }
            }
        }
        
        public static ResultBuffer GetExtensionDictionaryValue(Document document, ObjectId blockTableRecordId, string dictionaryName)
        {
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTableRecordId, OpenMode.ForWrite) as BlockTableRecord;
                    if (blockTableRecord == null)
                    {
                        return null;
                    }
                    if (blockTableRecord.ExtensionDictionary.IsNull)
                    {
                        return null;
                    }
                    DBDictionary extensionDictionary = transaction.GetObject(blockTableRecord.ExtensionDictionary, OpenMode.ForWrite) as DBDictionary;
                    if (extensionDictionary == null)
                    {
                        return null;
                    }
                    if (extensionDictionary.Contains(dictionaryName))
                    {
                        Xrecord xrecord = transaction.GetObject(extensionDictionary.GetAt(dictionaryName), OpenMode.ForRead) as Xrecord;
                        if (xrecord == null)
                        {
                            return null;
                        }
                        return xrecord.Data;
                    }
                    return null;
                }
            }
        }

        public static void ChangeExtensionDictionary(Document document, ObjectId blockTableRecordId, string dictionaryName, ResultBuffer resultBuffer)
        {
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTableRecordId, OpenMode.ForWrite) as BlockTableRecord;
                    if (blockTableRecord == null)
                    {
                        return;
                    }
                    if (blockTableRecord.ExtensionDictionary.IsNull)
                    {
                        return;
                    }
                    DBDictionary extensionDictionary = transaction.GetObject(blockTableRecord.ExtensionDictionary, OpenMode.ForWrite) as DBDictionary;
                    if (extensionDictionary == null)
                    {
                        return;
                    }
                    if (extensionDictionary.Contains(dictionaryName))
                    {
                        Xrecord xrecord = transaction.GetObject(extensionDictionary.GetAt(dictionaryName), OpenMode.ForWrite) as Xrecord;
                        if (xrecord == null)
                        {
                            return;
                        }
                        xrecord.Data = resultBuffer;
                        transaction.Commit();
                    }
                }   
            }
        }

        public static ObjectIdCollection GerBlockReferenceIdCollection(Document document, ObjectId blockTableRecordId)
        {
            if (blockTableRecordId.IsNull)
            {
                return null;
            }
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTableRecordId, OpenMode.ForRead) as BlockTableRecord;
                    if (blockTableRecord == null) { return null; }
                    ObjectIdCollection blockReferenceIdCollection = blockTableRecord.GetBlockReferenceIds(true, true);
                    return blockReferenceIdCollection;
                }
            }
        }
    }
}
