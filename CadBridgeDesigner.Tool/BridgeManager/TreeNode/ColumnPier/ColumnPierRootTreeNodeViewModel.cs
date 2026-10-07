using System;
using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.BridgeManager.TreeNode.ColumnPier
{
    public class ColumnPierRootTreeNodeViewModel: BaseTreeNodeViewModel
    {
        private RelayCommand InsertColumnPierCommand { get; set; }
        private void AddColumnPier()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            try
            {
                LogerTool.Log("[AddColumnPierCommand]添加柱式墩");
                if (document == null)
                {
                    LogerTool.Log("[AddColumnPierCommand]文档为空，添加失败");
                    return;
                }
                ColumnPierTool.AddColumnPierCommand(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddColumnPierCommand]添加失败: " + exception.GetDetail());
            }
        }
        
        public ColumnPierRootTreeNodeViewModel()
        {
            InsertColumnPierCommand = new RelayCommand(AddColumnPier);
            MenuItems.Add(new MenuItem("添加柱式墩", InsertColumnPierCommand));
        }
    }
}


