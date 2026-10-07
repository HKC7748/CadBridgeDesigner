using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Tool.Loger;

namespace CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool
{
    public static class DimStyleTool
    {
        /// <summary>
        /// 初始化标注样式
        /// </summary>
        /// <param name="document"></param>
        public static void InitDimStyle(Document document)
        {
            LogerTool.Log("[InitDimStyle]初始化标注样式");
            DimStyleTableRecord dimStyleTableRecord = new DimStyleTableRecord();
            dimStyleTableRecord.Name = "标注样式";                                                // 样式名称
            dimStyleTableRecord.Dimexe = 5;                                                     // 尺寸界线超出尺寸线长度
            dimStyleTableRecord.DimfxlenOn = true;                                              // 固定长度尺寸界线：开启
            dimStyleTableRecord.Dimfxlen = 5;                                                   // 固定长度尺寸界线：长度
            dimStyleTableRecord.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, 3);   // 尺寸线颜色：ByLayer
            dimStyleTableRecord.Dimclre = Color.FromColorIndex(ColorMethod.ByAci, 3);   // 尺寸界线颜色：ByLayer

            dimStyleTableRecord.Dimasz = 5;                                                     // 箭头大小

            dimStyleTableRecord.Dimclrt = Color.FromColorIndex(ColorMethod.ByAci, 3);   // 标注文字颜色：ByLayer
            dimStyleTableRecord.Dimtxt = 10;                                                    // 文字高度
            dimStyleTableRecord.Dimtad = 1;                                                     // 文字垂直位置：1=上方（ISO标准）
            dimStyleTableRecord.Dimtih = false;                                                 // 界线内：false=与尺寸线对齐（ISO标准）
            dimStyleTableRecord.Dimtoh = true;                                                  // 界线外：true=水平排列（ISO标准）

            dimStyleTableRecord.Dimlunit = 2;                                                   // 单位格式：2=小数
            dimStyleTableRecord.Dimdec = 1;                                                     // 小数位数：1位
            dimStyleTableRecord.Dimzin = 8;                                                     // 8 = 抑制十进制尾随零

            AddDimStyle(document, dimStyleTableRecord);
        }
        
        /// <summary>
        /// 向数据库中添加标注样式
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="dimStyleTableRecord">标注样式记录对象</param>
        private static void AddDimStyle(Document document, DimStyleTableRecord dimStyleTableRecord)
        {
            if (QueryDimStyle(document, dimStyleTableRecord.Name))
            {
                return;
            }
            Database database = document.Database;

            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    DimStyleTable dimStyleTable = transaction.GetObject(database.DimStyleTableId, OpenMode.ForRead) as DimStyleTable;
                    if (dimStyleTable == null)
                    {
                        return;
                    }
                    dimStyleTable.UpgradeOpen();
                    dimStyleTable.Add(dimStyleTableRecord);
                    transaction.AddNewlyCreatedDBObject(dimStyleTableRecord, true);
                    transaction.Commit();
                }
            }
        }
        /// <summary>
        /// 查询是否存在指定标注样式
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="dimStyleName">要查询的标注样式名称</param>
        /// <returns>存在返回true，不存在返回false</returns>
        private static bool QueryDimStyle(Document document, string dimStyleName)
        {
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    DimStyleTable dimStyleTable = transaction.GetObject(database.DimStyleTableId, OpenMode.ForRead) as DimStyleTable;
                    if (dimStyleTable == null) { return false; }
                    if (dimStyleTable.Has(dimStyleName)) { return true; }
                    return false;
                }
            }
        }
        /// <summary>
        /// 获取标注样式ID
        /// </summary>
        /// <param name="document"></param>
        /// <param name="dimStyleName"></param>
        /// <returns>成功返回标注样式ID,失败返回ObjectId.Null</returns>
        public static ObjectId GetDimStyleId(Document document, string dimStyleName)
        {
            Database database = document.Database;
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    DimStyleTable dimStyleTable = transaction.GetObject(database.DimStyleTableId, OpenMode.ForRead) as DimStyleTable;
                    if (dimStyleTable == null)
                    {
                        transaction.Commit();
                        return ObjectId.Null;
                    }

                    if (dimStyleTable.Has(dimStyleName))
                    {
                        transaction.Commit();
                        return dimStyleTable[dimStyleName];
                    }
                    transaction.Commit();
                    return ObjectId.Null;
                }
            }
        }
    }
}


