using System.Windows.Forms.Integration;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using CadBridgeDesigner.Core.ElementHostManager;
using CadBridgeDesigner.Tool.Bridge.Base;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam.View;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier.View;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool.CadTool;
using CadBridgeDesigner.Tool.Tool.CadTool.EntityTool;
using UserControl = System.Windows.Controls.UserControl;

namespace CadBridgeDesigner.Tool.PropertiesEditor
{
    public static class PropertiesEditorTool
    {
        private static BridgeComponentModel GetBridgeComponent(Document document, ObjectId blockTableReferenceId)
        {
            if (blockTableReferenceId.IsNull)
            {
                return null;
            }
            BlockTableRecord blockTableRecord = BlockTableRecordTool.GetBlockTableRecordByBlockReferenceId(document, blockTableReferenceId);
            if (blockTableRecord == null)
            {
                return null;
            }
            ResultBuffer typeResultBuffer = BlockTableRecordTool.GetExtensionDictionaryValue(document, blockTableRecord.Id, "TYPE");
            if (typeResultBuffer == null)
            {
                return null;
            }
            string type = (string)typeResultBuffer.AsArray()[0].Value;
            if (type == null)
            {
                return null;
            }
            switch (type)
            {
                case "COLUMN_PIER":
                    return ColumnPierTool.GetColumnPierModel(document, blockTableRecord.Id);
                case "TBEAM":
                    return TbeamTool.GetTbeamModel(document, blockTableRecord.Id);
                default:
                    return null;
            }
        }
        public static void PropertiesEditorUpdate(Document document)
        {
            Editor editor = document.Editor;
            PromptSelectionResult promptSelectionResult = editor.SelectImplied();
            UserControl view;
            if (promptSelectionResult.Value == null)
            {
                view = new NullView();
            }
            else
            {
                ObjectId[] entityIds = promptSelectionResult.Value.GetObjectIds();
                if (entityIds.Length == 1)
                { 
                    BridgeComponentModel bridgeComponentModel = GetBridgeComponent(document, entityIds[0]);
                    switch (bridgeComponentModel)
                    {
                        case ColumnPierModel pierModel:
                            ColumnPierEditViewModel columnPierEditViewModel = new ColumnPierEditViewModel(pierModel);
                            ColumnPierEditView columnPierEditView = new ColumnPierEditView(columnPierEditViewModel);
                            view = columnPierEditView;
                            break;
                        case TbeamModel tbeamModel:
                            TbeamEditViewModel tbeamEditViewModel = new TbeamEditViewModel(tbeamModel);
                            TbeamEditView tbeamEditView = new TbeamEditView(tbeamEditViewModel);
                            view = tbeamEditView;
                            break;
                        default:
                            NullView nullView = new NullView();
                            view = nullView;
                            break;
                    }
                }
                else
                {
                    view = new NullView();
                }
            }
            ElementHostManager.PropertiesEditorElementHost.Child = view;
        }
        
        public static void InitPropertiesEditView()
        {
            LogerTool.Log("[InitPropertiesEditView]初始化属性编辑器视图");
            NullView nullView = new NullView();
            ElementHost elementHost = ElementHostManager.PropertiesEditorElementHost;
            elementHost.Child = nullView;
        }
    }
}


