using System;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using CadBridgeDesigner.Tool.Bridge.Base;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam.View;
using CadBridgeDesigner.Tool.BridgeManager;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool;
using CadBridgeDesigner.Tool.Tool.CadTool.EntityTool;
using CadBridgeDesigner.Tool.Tool.CadTool.InteractTool;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.Bridge.Beam.Tbeam
{
    public static class TbeamTool
    {
        private static ResultBuffer GetTbeamResultBuffer(this TbeamModel tbeamModel)
        {
            ResultBuffer resultBuffer = new ResultBuffer();
            resultBuffer.Add(new TypedValue((int)DxfCode.Text, tbeamModel.Name));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.Mileage));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.BeamEndGap));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.SpanLength));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.Height));
            resultBuffer.Add(new TypedValue((int)DxfCode.Int16, tbeamModel.NumberOfTbeam));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.WidthOfFlange));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.HeightOfFlange));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.WidthOfFlangeHaunch));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.HeightOfFlangeHaunch));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.WidthOfWeb));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.WidthOfButtonBulb));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.HeightOfButtonBulb));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.HeightOfButtonBulbSlope));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.DistanceOfStartOfCrossSection));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.DistanceOfEndOfCrossSection));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.SpaceOfDiaphragm));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.ThicknessOfDiaphragm));
            resultBuffer.Add(new TypedValue((int)DxfCode.Int16, tbeamModel.NumberOfMiddleDiaphragm));
            resultBuffer.Add(new TypedValue((int)DxfCode.Real, tbeamModel.DistanceOfEndDiaphragm));
            
    
            return resultBuffer;
        }
        public static TbeamModel GetTbeamModel(Document document, ObjectId blockTableRecordId)
        {
            ResultBuffer typeXrecord = BlockTableRecordTool.GetExtensionDictionaryValue(document, blockTableRecordId, "TYPE");
            if (typeXrecord == null)
            {
                LogerTool.Log("[GetTbeamModel]块定义不存在TYPE,返回null");
                return null;
            }
            if ((string)typeXrecord.AsArray()[0].Value != "TBEAM")
            {
                LogerTool.Log("[GetTbeamModel]块定义不是TBEAM,返回null");
                return null;
            }
            ResultBuffer infoResultBuffer = BlockTableRecordTool.GetExtensionDictionaryValue(document, blockTableRecordId, "INFO");
            if (infoResultBuffer == null)
            {
                LogerTool.Log("[GetTbeamModel]块定义不存在INFO,返回null");
                return null;
            }
            TypedValue[] dataArray = infoResultBuffer.AsArray();
            int index = 0;

            string name = Convert.ToString(dataArray[index].Value); index++;
            double mileage = Convert.ToDouble(dataArray[index].Value); index++;
            double beamEndGap = Convert.ToDouble(dataArray[index].Value); index++;
            double length = Convert.ToDouble(dataArray[index].Value); index++;
            double height = Convert.ToDouble(dataArray[index].Value); index++;
            int numberOfTbeam = Convert.ToInt32(dataArray[index].Value); index++;
            double widthOfFlange = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfFlange = Convert.ToDouble(dataArray[index].Value); index++;
            double widthOfFlangeHaunch = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfFlangeHaunch = Convert.ToDouble(dataArray[index].Value); index++;
            double widthOfWeb = Convert.ToDouble(dataArray[index].Value); index++;
            double widthOfButtonBulb = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfButtonBulb = Convert.ToDouble(dataArray[index].Value); index++;
            double heightOfButtonBulbSlope = Convert.ToDouble(dataArray[index].Value); index++;
            double distanceOfStartOfCrossSection = Convert.ToDouble(dataArray[index].Value); index++;
            double distanceOfEndOfCrossSection = Convert.ToDouble(dataArray[index].Value); index++;
            double spaceOfDiaphragm = Convert.ToDouble(dataArray[index].Value); index++;
            double thicknessOfDiaphragm = Convert.ToDouble(dataArray[index].Value); index++;
            int numberOfMiddleDiaphragm = Convert.ToInt32(dataArray[index].Value); index++;
            double distanceOfEndDiaphragm = Convert.ToDouble(dataArray[index].Value);

            TbeamModel tbeamModel = new TbeamModel
            {
                Type = BridgeComponentType.Tbeam,
                Name = name,
                Mileage = mileage,
                BlockTableRecordId = blockTableRecordId,
                BlockTableRecordName = BlockTableRecordTool.GetBlockTableRecordByBlockTableRecordId(document, blockTableRecordId).Name,
                
                BeamEndGap = beamEndGap,
                SpanLength = length,
                Height = height,
                NumberOfTbeam = numberOfTbeam,
                WidthOfFlange = widthOfFlange,
                HeightOfFlange = heightOfFlange,
                WidthOfFlangeHaunch = widthOfFlangeHaunch,
                HeightOfFlangeHaunch = heightOfFlangeHaunch,
                WidthOfWeb = widthOfWeb,
                WidthOfButtonBulb = widthOfButtonBulb,
                HeightOfButtonBulb = heightOfButtonBulb,
                HeightOfButtonBulbSlope = heightOfButtonBulbSlope,
                DistanceOfStartOfCrossSection = distanceOfStartOfCrossSection,
                DistanceOfEndOfCrossSection = distanceOfEndOfCrossSection,
                SpaceOfDiaphragm = spaceOfDiaphragm,
                ThicknessOfDiaphragm = thicknessOfDiaphragm,
                NumberOfMiddleDiaphragm = numberOfMiddleDiaphragm,
                DistanceOfEndDiaphragm = distanceOfEndDiaphragm,
                BlockReferenceIdCollection = BlockTableRecordTool.GerBlockReferenceIdCollection(document, blockTableRecordId)
            };
            return tbeamModel;
        }
        public static void TbeamUpdate(this Document document, TbeamModel tbeamModel)
        {
            if (tbeamModel == null)
            {
                LogerTool.Log("[TbeamUpdate]T梁模型不合法, 更新失败");
                return;
            }
            if (tbeamModel.BlockTableRecordId.IsNull)
            {
                LogerTool.Log("[TbeamUpdate]ID不合法, 更新失败");
                return;
            }
            Editor editor = document.Editor;
            DBObjectCollection newEntities = tbeamModel.GetTbeamSolids();
            BlockTableRecordTool.ChangeEntity(document, tbeamModel.BlockTableRecordId, newEntities);
            ResultBuffer resultBuffer = tbeamModel.GetTbeamResultBuffer();
            BlockTableRecordTool.ChangeExtensionDictionary(document, tbeamModel.BlockTableRecordId, "INFO", resultBuffer);
            editor.Regen();
            BridgeManagerTool.BridgeManagerUpdate(document);
        }
        public static void AddTbeamCommand(Document document)
        {
            TbeamModel tbeamModel = new TbeamModel();
            TbeamAddViewModel tbeamAddViewModel = new TbeamAddViewModel(tbeamModel);
            TbeamAddView tbeamAddView = new TbeamAddView(tbeamAddViewModel)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 250,
                Height = 500
            };
            Application.ShowModalWindow(Application.MainWindow.Handle, tbeamAddView, false);
            if (tbeamAddViewModel.Result is false)
            {
                LogerTool.Log("[AddTbeamCommand]用户取消操作, 添加失败");
                return;
            }
            tbeamModel = tbeamAddViewModel.TbeamModel;
            tbeamModel.BlockTableRecordName = $"T梁_{DateTime.Now:yyyyMMddHHmmssfff}";
            Database database = document.Database;
            DBObjectCollection tbeamEntities = tbeamModel.GetTbeamSolids();
            Editor editor = document.Editor;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForWrite) as BlockTable;
                    if (blockTable == null)
                    {
                        LogerTool.Log("[AddTbeamCommand]块表为空, 添加失败");
                        return;
                    }
                    DrawJigInMove jigInMove = new DrawJigInMove(tbeamEntities, Point3d.Origin);
                    PromptResult jigResult = editor.Drag(jigInMove);
                    if (jigResult.Status != PromptStatus.OK)
                    {
                        LogerTool.Log("[AddTbeamCommand]用户取消操作, 添加失败");
                        return;
                    }
                    BlockTableRecord tbeamBlockTableRecord = new BlockTableRecord();
                    tbeamBlockTableRecord.Name = tbeamModel.BlockTableRecordName;
                    tbeamBlockTableRecord.Origin = Point3d.Origin;
                    foreach (Entity entity in jigInMove.Entities)
                    {
                        entity.SetLayerId(LayerTool.GetLayerId(document, "MODEL_SPACE_3D"), true);
                        tbeamBlockTableRecord.AppendEntity(entity);
                    }
                    ObjectId tbeamBlockTableRecordId = blockTable.Add(tbeamBlockTableRecord);
                    BlockTableRecordTool.AddExtensionDictionary(document, tbeamBlockTableRecordId, "TYPE", new ResultBuffer(new TypedValue((int)DxfCode.Text, "TBEAM")));
                    BlockTableRecordTool.AddExtensionDictionary(document, tbeamBlockTableRecordId, "INFO", tbeamModel.GetTbeamResultBuffer());
                    BlockTableRecord modelSpace = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (modelSpace == null)
                    {
                        LogerTool.Log("[AddTbeamCommand]模型空间为空, 添加失败");
                        return;
                    }
                    BlockReference blockReference = new BlockReference(new Point3d(jigInMove.Displacement.X, jigInMove.Displacement.Y, jigInMove.Displacement.Z), tbeamBlockTableRecordId);
                    blockReference.SetLayerId(LayerTool.GetLayerId(document, "MODEL_SPACE_3D"), true);
                    modelSpace.AppendEntity(blockReference);
                    transaction.AddNewlyCreatedDBObject(tbeamBlockTableRecord, true);
                    transaction.AddNewlyCreatedDBObject(blockReference, true);
                    transaction.Commit();
                    BridgeManagerTool.BridgeManagerUpdate(document);
                }
            }
        }
        public static void AddTbeamInstance(this TbeamModel tbeamModel, Document document)
        {
            Database database = document.Database;
            DBObjectCollection tbeamEntities = tbeamModel.GetTbeamSolids();
            Editor editor = document.Editor;
            using (document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForWrite) as BlockTable;
                    if (blockTable == null)
                    {
                        LogerTool.Log("[AddTbeamCommand]块表为空, 添加失败");
                        return;
                    }
                    DrawJigInMove jigInMove = new DrawJigInMove(tbeamEntities, Point3d.Origin);
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
                    BlockReference blockReference = new BlockReference(new Point3d(jigInMove.Displacement.X, jigInMove.Displacement.Y, jigInMove.Displacement.Z), tbeamModel.BlockTableRecordId);
                    blockReference.SetLayerId(LayerTool.GetLayerId(document, "MODEL_SPACE_3D"), true);
                    modelSpace.AppendEntity(blockReference);
                    transaction.AddNewlyCreatedDBObject(blockReference, true);
                    transaction.Commit();
                    BridgeManagerTool.BridgeManagerUpdate(document);
                }
            }
        }
        
        
        public static void AddTbeamPointEdgeSectionDiagrams(this TbeamModel tbeamModel, Document document)
        {
            BridgeComponentDiagram bridgeComponentDiagram = tbeamModel.GetTbeamPointEdgeSectionDiagrams();
            JigTool.AddBridgeComponentDiagramInJigInMove(document,bridgeComponentDiagram);
        }
        
        public static void AddTbeamPointMiddleSectionDiagrams(this TbeamModel tbeamModel, Document document)
        {
            BridgeComponentDiagram bridgeComponentDiagram = tbeamModel.GetTbeamPointMiddleSectionDiagrams();
            JigTool.AddBridgeComponentDiagramInJigInMove(document,bridgeComponentDiagram);
        }
        
        public static void AddTbeamMidspanEdgeSectionDiagrams(this TbeamModel tbeamModel, Document document)
        {
            BridgeComponentDiagram bridgeComponentDiagram = tbeamModel.GetTbeamMidspanEdgeSectionDiagrams();
            JigTool.AddBridgeComponentDiagramInJigInMove(document,bridgeComponentDiagram);
        }
        
        public static void AddTbeamMidspanMiddleSectionDiagrams(this TbeamModel tbeamModel, Document document)
        {
            BridgeComponentDiagram bridgeComponentDiagram = tbeamModel.GetTbeamMidspanMiddleSectionDiagrams();
            JigTool.AddBridgeComponentDiagramInJigInMove(document,bridgeComponentDiagram);
        }
        
        public static void AddTbeamPointCrossSectionDiagrams(this TbeamModel tbeamModel, Document document)
        {
            BridgeComponentDiagram bridgeComponentDiagram = tbeamModel.GetTbeamPointCrossSectionDiagrams();
            JigTool.AddBridgeComponentDiagramInJigInMove(document,bridgeComponentDiagram);
        }
        
        public static void AddTbeamMidspanCrossSectionDiagrams(this TbeamModel tbeamModel, Document document)
        {
            BridgeComponentDiagram bridgeComponentDiagram = tbeamModel.GetTbeamMidspanCrossSectionDiagrams();
            JigTool.AddBridgeComponentDiagramInJigInMove(document,bridgeComponentDiagram);
        }
    }
}