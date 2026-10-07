using System;
using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.BridgeManager.TreeNode.Tbeam
{
    public class TbeamRootTreeNodeViewModel: BaseTreeNodeViewModel
    {
        private RelayCommand InsertTbeamCommand { get; set; }
        private void AddTbeam()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            try
            {
                LogerTool.Log("[AddTbeamCommand]添加T梁");
                if (document == null)
                {
                    LogerTool.Log("[AddTbeamCommand]文档为空，添加失败");
                    return;
                }
                TbeamTool.AddTbeamCommand(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddTbeamCommand]添加失败: " + exception.GetDetail());
            }
        }
        
        public TbeamRootTreeNodeViewModel()
        {
            InsertTbeamCommand = new RelayCommand(AddTbeam);
            MenuItems.Add(new MenuItem("添加T梁", InsertTbeamCommand));
        }
    }
}
