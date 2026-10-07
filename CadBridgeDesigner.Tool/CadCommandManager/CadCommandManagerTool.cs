using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Tool.Loger;

namespace CadBridgeDesigner.Tool.CadCommandManager
{
    public static class CadCommandManagerTool
    {
        public static void RegisterCadCommandHandler(Document document)
        {
            LogerTool.Log("[RegisterCadCommandHandler]注册CAD命令处理程序");
            Core.CadCommandManager.CadCommandManager.CadCommandManagerHandler = new CadCommandManagerHandler();
        }
    }
}


