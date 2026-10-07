using System.Windows.Forms.Integration;
using CadBridgeDesigner.Core.ElementHostManager;

namespace CadBridgeDesigner.Tool.Loger
{
    public static class LogerTool
    {
        public static void Log(string message)
        {
            LogerModel.LogMessages += message + "\n";
        }
        
        public static void InitLogerView()
        {
            Log("[InitLogerView]初始化日志视图");
            LogerModel logerModel = new LogerModel();
            LogerViewModel logerViewModel = new LogerViewModel(logerModel);
            LogerView logerView = new LogerView(logerViewModel);
            ElementHost elementHost = ElementHostManager.LogerElementHost;
            elementHost.Child = logerView;
        }
    }
}


