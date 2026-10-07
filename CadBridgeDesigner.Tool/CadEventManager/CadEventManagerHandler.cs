using System;
using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Core.CadEventManager;
using CadBridgeDesigner.Tool.BridgeManager;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.PropertiesEditor;
using CadBridgeDesigner.Tool.Tool;
using CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool;

namespace CadBridgeDesigner.Tool.CadEventManager
{
    public class CadEventManagerHandler: ICadEventManagerHandler
    {
        public void OnSelectionUpdate(Document document)
        {
            try
            {
                LogerTool.Log("[OnSelectionUpdate]更新属性编辑器");
                if (document == null)
                {
                    LogerTool.Log("[OnSelectionUpdate]文档为空，更新失败");
                    return;
                }
                PropertiesEditorTool.PropertiesEditorUpdate(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[OnSelectionUpdate]更新失败: " + exception.GetDetail());
            }
            
        }

        public void OnBridgeManagerUpdate(Document document)
        {
            try
            {
                LogerTool.Log("[OnBridgeManagerUpdate]更新桥梁管理器");
                if (document == null)
                {
                    LogerTool.Log("[OnBridgeManagerUpdate]文档为空，更新失败");
                    return;
                }
                BridgeManagerTool.BridgeManagerUpdate(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[OnBridgeManagerUpdate]更新失败: " + exception.GetDetail());
            }
        }

        public void OnInitConfigure(Document document)
        {
            try
            {
                LogerTool.Log("[OnInitConfigure]更新配置");
                if (document == null)
                {
                    LogerTool.Log("[OnInitConfigure]文档为空，更新失败");
                    return;
                }
                
                BridgeManagerTool.InitBridgeManagerView();
                
                PropertiesEditorTool.InitPropertiesEditView();
                
                LogerTool.InitLogerView();
                
                LineTypeTool.InitLineType(document);
                
                LayerTool.InitLayer(document);
                
                DimStyleTool.InitDimStyle(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[OnInitConfigure]更新失败: " + exception.GetDetail());
            }
        }
    }
}


