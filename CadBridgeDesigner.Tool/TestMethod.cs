using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Tool.Bridge.Base;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam;
using CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool;
using CadBridgeDesigner.Tool.Tool.CadTool.EntityTool;

namespace CadBridgeDesigner.Tool
{
    public static class TestMethod
    {
        public static void TestMethod1(Document document)
        {
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(document.Database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    if (blockTable == null){return;}
                    BlockTableRecord modelSpace = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (modelSpace == null){return;}


                    TbeamModel tbeamModel = new TbeamModel();
                    BridgeComponentDiagram bridgeComponentDiagram = tbeamModel.GetTbeamMidspanCrossSectionDiagrams();
                    foreach (Polyline polyline in bridgeComponentDiagram.CenterCurves)
                    {
                        polyline.LayerId = LayerTool.GetLayerId(document, "CENTER");
                        modelSpace.AppendEntity(polyline);
                        transaction.AddNewlyCreatedDBObject(polyline, true);
                    }
                    foreach (Polyline polyline in bridgeComponentDiagram.StructureCurves)
                    {
                        polyline.LayerId = LayerTool.GetLayerId(document, "STRUCTURE_CONTINUOUS");
                        modelSpace.AppendEntity(polyline);
                        transaction.AddNewlyCreatedDBObject(polyline, true);
                    }
                    foreach (Polyline polyline in bridgeComponentDiagram.DashedCurves)
                    {
                        polyline.LayerId = LayerTool.GetLayerId(document, "STRUCTURE_DASHED");
                        modelSpace.AppendEntity(polyline);
                        transaction.AddNewlyCreatedDBObject(polyline, true);
                    }
                    foreach (Polyline polyline in bridgeComponentDiagram.HatchCurves)
                    {
                        polyline.LayerId = LayerTool.GetLayerId(document, "STRUCTURE_FILL");
                        Hatch hatch = polyline.TranslateToHatch();
                        modelSpace.AppendEntity(polyline);
                        transaction.AddNewlyCreatedDBObject(polyline, true);
                        modelSpace.AppendEntity(hatch);
                        transaction.AddNewlyCreatedDBObject(hatch, true);
                    }
                    foreach (RotatedDimension rotatedDimension in bridgeComponentDiagram.Dimensions)
                    {
                        rotatedDimension.LayerId = LayerTool.GetLayerId(document, "DIM");
                        rotatedDimension.DimensionStyle = DimStyleTool.GetDimStyleId(document, "标注样式");
                        modelSpace.AppendEntity(rotatedDimension);
                        transaction.AddNewlyCreatedDBObject(rotatedDimension, true);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}


