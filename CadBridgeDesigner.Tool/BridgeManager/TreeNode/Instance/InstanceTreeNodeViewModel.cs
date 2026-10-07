using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool;
using CadBridgeDesigner.Tool.Tool.CadTool;
using CadBridgeDesigner.Tool.Tool.CadTool.EntityTool;
using CadBridgeDesigner.Tool.Tool.CadTool.InteractTool;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.BridgeManager.TreeNode.Instance
{
    public class InstanceTreeNodeViewModel: BaseTreeNodeViewModel
    {
        private ObjectId _blockReferenceId;
        public ObjectId BlockReferenceId
        {
            get => _blockReferenceId;
            set
            {
                _blockReferenceId = value; 
                OnPropertyChanged();
            }
        }

        private RelayCommand ZoomInstanceCommand { get; set; }

        private void ZoomInstance()
        {
            try
            {
                LogerTool.Log("[ZoomInstance]缩放实例");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[ZoomInstance]文档为空，缩放失败");
                    return;
                }
                EditorTool.SetSelectionByObjectIds(document, new ObjectIdCollection(){ BlockReferenceId });
                ViewTool.ZoomBlockReference(document, BlockReferenceId);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[ZoomInstance]" + exception.GetDetail());
            }
            
        }
        
        private RelayCommand RemoveInstanceCommand { get; set; }
        private void RemoveInstance()
        {
            try
            {
                LogerTool.Log("[RemoveInstance]删除实例");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[RemoveInstance]文档为空，删除失败");
                    return;
                }
                DataBaseTool.RemoveDbObjectByObjectId(document, BlockReferenceId);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[RemoveInstance]" + exception.GetDetail());
            }
        }
        
        public InstanceTreeNodeViewModel(ObjectId blockReferenceId)
        {
            BlockReferenceId = blockReferenceId;
            ZoomInstanceCommand = new RelayCommand(ZoomInstance);
            MenuItems.Add(new MenuItem("定位实例", ZoomInstanceCommand));
            RemoveInstanceCommand = new RelayCommand(RemoveInstance);
            MenuItems.Add(new MenuItem("删除实例", RemoveInstanceCommand));
        }
    }
}


