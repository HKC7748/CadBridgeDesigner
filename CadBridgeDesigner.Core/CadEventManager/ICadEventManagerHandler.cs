using Autodesk.AutoCAD.ApplicationServices;

namespace CadBridgeDesigner.Core.CadEventManager
{
    public interface ICadEventManagerHandler
    {
        public void OnSelectionUpdate(Document document);
        
        public void OnBridgeManagerUpdate(Document document);

        public void OnInitConfigure(Document document);
    }
}


