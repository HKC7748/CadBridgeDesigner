using System;
using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Core.ElementHostManager;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier.View;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.BridgeManager.TreeNode.ColumnPier
{
    public class ColumnPierTreeNodeViewModel : BaseTreeNodeViewModel
    {
        private ColumnPierModel _columnPierModel;

        private ColumnPierModel ColumnPierModel
        {
            get => _columnPierModel;
            set
            {
                _columnPierModel = value;
                OnPropertyChanged();
            }
        }
        
        protected override void OnNodeClick()
        {
            ColumnPierEditViewModel columnPierEditViewModel = new ColumnPierEditViewModel(ColumnPierModel);
            ColumnPierEditView columnPierEditView = new ColumnPierEditView(columnPierEditViewModel);
            ElementHostManager.PropertiesEditorElementHost.Child = columnPierEditView;
        }
        
        private RelayCommand InsertInstanceCommand { get; set; }

        private void AddInstance()
        {
            try
            {
                LogerTool.Log("[AddInstance]添加柱式墩实例");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[AddInstance]文档为空，添加失败");
                    return;
                }
                ColumnPierModel.AddColumnPierInstance(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddInstance]" + exception.GetDetail());
            }
        }
        
        public ColumnPierTreeNodeViewModel(ColumnPierModel columnPierModel)
        {
            ColumnPierModel = columnPierModel;
            InsertInstanceCommand = new RelayCommand(AddInstance);
            MenuItems.Add(new MenuItem("添加实例", InsertInstanceCommand));
        }
    }
}
