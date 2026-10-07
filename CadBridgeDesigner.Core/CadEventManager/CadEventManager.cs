using System;
using Autodesk.AutoCAD.ApplicationServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Core.CadEventManager
{
    public static class CadEventManager
    {
        public static ICadEventManagerHandler CadEventManagerHandler { get; set; }
        
        public static void RegisterEvents()
        {
            foreach (Document document in Application.DocumentManager)
            {
                document.ImpliedSelectionChanged += OnImpliedSelectionChanged;
                document.CommandEnded += OnCommandEnded;
            }

            Application.DocumentManager.DocumentCreated += OnDocumentCreated;
            Application.DocumentManager.DocumentActivated += OnDocumentActivated;
        }
        private static void OnImpliedSelectionChanged(object sender, EventArgs e)
        {
            Document document = Application.DocumentManager.CurrentDocument;
            CadEventManagerHandler?.OnSelectionUpdate(document);
        }
        private static void OnDocumentCreated(object sender, DocumentCollectionEventArgs e)
        {
            Document document = Application.DocumentManager.CurrentDocument;
            document.ImpliedSelectionChanged += OnImpliedSelectionChanged;
            document.CommandEnded += OnCommandEnded;
        }
        private static void OnDocumentActivated(object sender, DocumentCollectionEventArgs e)
        {
            Document document = Application.DocumentManager.CurrentDocument;
            CadEventManagerHandler?.OnInitConfigure(document);
        }

        private static void OnCommandEnded(object sender, CommandEventArgs e)
        {
            Document document = Application.DocumentManager.CurrentDocument;
            CadEventManagerHandler?.OnBridgeManagerUpdate(document);
        }
    }
}


