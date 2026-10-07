using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Tool.Loger;

namespace CadBridgeDesigner.Tool.CadEventManager
{
    public static class CadEventManagerTool
    {
        public static void RegisterCadEventHandler(Document document)
        {
            LogerTool.Log("[RegisterCadEventHandler]注册CAD事件处理程序");
            Core.CadEventManager.CadEventManager.CadEventManagerHandler = new CadEventManagerHandler();
        }
    }
}


