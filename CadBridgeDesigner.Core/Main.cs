using System;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Core
{
    public class Main: IExtensionApplication
    {
        public void Initialize()
        {
            Application.Idle += OnApplicationIdle;
        }
        private void OnApplicationIdle(object sender, EventArgs e)
        {
            Application.Idle -= OnApplicationIdle;
            CadEventManager.CadEventManager.RegisterEvents();
            ElementHostManager.ElementHostManager.InitLogerView();
            ElementHostManager.ElementHostManager.InitBridgeManagerView();
            ElementHostManager.ElementHostManager.InitPropertiesEditView();
        }
        public void Terminate()
        {
        }
    }
}
