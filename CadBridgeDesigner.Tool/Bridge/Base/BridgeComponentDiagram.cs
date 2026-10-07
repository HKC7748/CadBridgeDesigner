using Autodesk.AutoCAD.DatabaseServices;

namespace CadBridgeDesigner.Tool.Bridge.Base
{
    public class BridgeComponentDiagram
    {
        public DBObjectCollection Dimensions;

        public DBObjectCollection StructureCurves;

        public DBObjectCollection CenterCurves;

        public DBObjectCollection DashedCurves;

        public DBObjectCollection HatchCurves;
        
        public BridgeComponentDiagram()
        {
            Dimensions = new DBObjectCollection();
            StructureCurves = new DBObjectCollection();
            CenterCurves = new DBObjectCollection();
            DashedCurves = new DBObjectCollection();
            HatchCurves = new DBObjectCollection();
        }
    }
}


