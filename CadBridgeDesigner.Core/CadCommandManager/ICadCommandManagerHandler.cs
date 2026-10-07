using Autodesk.AutoCAD.ApplicationServices;

namespace CadBridgeDesigner.Core.CadCommandManager
{
    public interface ICadCommandManagerHandler
    {
        public void AddColumnPierCommand(Document document);
        public void AddTbeamCommand(Document document);
        public void TestMethod1(Document document);
    }
}


