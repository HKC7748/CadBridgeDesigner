using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Core.CadCommandManager
{
    public static class CadCommandManager
    {
        public static ICadCommandManagerHandler CadCommandManagerHandler { get; set; }
        
        
        [CommandMethod("SHOW_VIEWS")]
        public static void ShowViews()
        {
            ElementHostManager.ElementHostManager.ShowViews();
        }
        
        [CommandMethod("ADD_COLUMN_PIER")]
        public static void InsertPierCommand()
        {
            Document document = Application.DocumentManager.CurrentDocument;
            CadCommandManagerHandler?.AddColumnPierCommand(document);
        }
        [CommandMethod("ADD_TBEAM")]
        public static void InsertTbeamCommand()
        {
            Document document = Application.DocumentManager.CurrentDocument;
            CadCommandManagerHandler?.AddTbeamCommand(document);
        }
        [CommandMethod("TEST_METHOD")]
        public static void TestMethodCommand()
        {
            Document document = Application.DocumentManager.CurrentDocument;
            CadCommandManagerHandler?.TestMethod1(document);
        }
    }
}


