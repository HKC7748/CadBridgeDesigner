using System;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using CadBridgeDesigner.Tool.BridgeManager;
using CadBridgeDesigner.Tool.CadCommandManager;
using CadBridgeDesigner.Tool.CadEventManager;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.PropertiesEditor;
using CadBridgeDesigner.Tool.Tool;
using CadBridgeDesigner.Tool.Tool.CadTool.ConfigureTool;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = System.Exception;


namespace CadBridgeDesigner.Tool
{
    public class Main : IExtensionApplication
    {
        public void Initialize()
        {
            Application.Idle += OnApplicationIdle;
        }
        
        private void OnApplicationIdle(object sender, EventArgs e)
        {
            Application.Idle -= OnApplicationIdle;
            Document document = Application.DocumentManager.CurrentDocument;
            
            CadCommandManagerTool.RegisterCadCommandHandler(document);
                
            CadEventManagerTool.RegisterCadEventHandler(document);
            
            BridgeManagerTool.InitBridgeManagerView();
                
            PropertiesEditorTool.InitPropertiesEditView();
                
            LogerTool.InitLogerView();
                
            LineTypeTool.InitLineType(document);
                
            LayerTool.InitLayer(document);
            
            DimStyleTool.InitDimStyle(document);
        }
        public void Terminate()
        {
        }
        
        
    }
}