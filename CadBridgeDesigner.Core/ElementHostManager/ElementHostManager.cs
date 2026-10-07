using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Autodesk.AutoCAD.Windows;

namespace CadBridgeDesigner.Core.ElementHostManager
{
    public static class ElementHostManager
    {
        private static PaletteSet BridgeManagerPaletteSet { get; set; }
        private static PaletteSet PropertiesEditorPaletteSet { get; set; }
        private static PaletteSet LogerPaletteSet { get; set; }
        
        public static ElementHost BridgeManagerElementHost { get; set; }
        public static ElementHost PropertiesEditorElementHost { get; set; }
        
        public static ElementHost LogerElementHost { get; set; }
        
        
        public static void InitBridgeManagerView()
        {
            BridgeManagerPaletteSet = new PaletteSet("项目管理器");
            BridgeManagerPaletteSet.Visible = true;
            BridgeManagerElementHost = new ElementHost();
            BridgeManagerElementHost.Dock = DockStyle.Fill;
            BridgeManagerPaletteSet.Add("项目管理器", BridgeManagerElementHost);
            BridgeManagerPaletteSet.Dock = DockSides.Left;
        }
        public static void InitPropertiesEditView()
        {
            PropertiesEditorPaletteSet = new PaletteSet("属性编辑器");
            PropertiesEditorPaletteSet.Visible = true;
            PropertiesEditorElementHost = new ElementHost();
            PropertiesEditorElementHost.Dock = DockStyle.Fill;
            PropertiesEditorPaletteSet.Add("属性编辑器", PropertiesEditorElementHost);
            PropertiesEditorPaletteSet.Dock = DockSides.Right;
            
        }
        public static void InitLogerView()
        {
            LogerPaletteSet = new PaletteSet("日志");
            LogerPaletteSet.Visible = true;
            LogerElementHost = new ElementHost();
            LogerElementHost.Dock = DockStyle.Fill;
            LogerPaletteSet.Add("日志", LogerElementHost);
            LogerPaletteSet.Dock = DockSides.Left;
        }

        public static void ShowViews()
        {
            BridgeManagerPaletteSet.Visible = true;
            PropertiesEditorPaletteSet.Visible = true;
            LogerPaletteSet.Visible = true;
        }
    }
}
