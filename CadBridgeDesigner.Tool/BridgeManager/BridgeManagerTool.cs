using System;
using System.Collections.ObjectModel;
using System.Windows.Forms.Integration;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Core.ElementHostManager;
using CadBridgeDesigner.Tool.Bridge;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.ColumnPier;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Instance;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Tbeam;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool.CadTool;
using CadBridgeDesigner.Tool.Tool.CadTool.EntityTool;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.BridgeManager
{
    public static class BridgeManagerTool
    {
        public static event Action TreeNodesRefreshRequested;

        public static void BridgeManagerUpdate(Document document)
        {
            BridgeManagerModel.Tbeams.Clear();
            BridgeManagerModel.ColumnPiers.Clear();
            BridgeManagerModel.Bridges.Clear();
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(document.Database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    if (blockTable == null)
                    {
                        LogerTool.Log("[BridgeManagerUpdate]块表不合法,更新失败");
                        return;
                    }

                    foreach (ObjectId blockTableRecordId in blockTable)
                    {
                        BlockTableRecord blockTableRecord = (BlockTableRecord)transaction.GetObject(blockTableRecordId, OpenMode.ForRead);
                        ResultBuffer typeResultBuffer = BlockTableRecordTool.GetExtensionDictionaryValue(document, blockTableRecord.Id, "TYPE");
                        if (typeResultBuffer == null)
                        {
                            continue;
                        }

                        string type = (string)typeResultBuffer.AsArray()[0].Value;
                        switch (type)
                        {
                            case "COLUMN_PIER":
                                ColumnPierModel columnPierModel = ColumnPierTool.GetColumnPierModel(document, blockTableRecordId);
                                BridgeManagerModel.ColumnPiers.Add(columnPierModel);
                                break;
                            case "TBEAM":
                                TbeamModel tbeamModel = TbeamTool.GetTbeamModel(document, blockTableRecordId);
                                BridgeManagerModel.Tbeams.Add(tbeamModel);
                                break;
                        }
                    }
                }
            }
            TreeNodesRefreshRequested?.Invoke();
        }
        
        public static void InitBridgeManagerView()
        {
            LogerTool.Log("[InitBridgeManagerView]初始化桥管理器视图");
            BridgeManagerModel bridgeManagerModel = new BridgeManagerModel();
            BridgeManagerViewModel bridgeManagerViewModel = new BridgeManagerViewModel(bridgeManagerModel);
            BridgeManagerView bridgeManagerView = new BridgeManagerView(bridgeManagerViewModel);
            ElementHost elementHost = ElementHostManager.BridgeManagerElementHost;
            elementHost.Child = bridgeManagerView;
            Document document = Application.DocumentManager.CurrentDocument;
            BridgeManagerUpdate(document);
        }
        
        public static ObservableCollection<BaseTreeNodeViewModel> GetTreeNodes()
        {
            ObservableCollection<BaseTreeNodeViewModel> treeNodes = new ObservableCollection<BaseTreeNodeViewModel>();
            ColumnPierRootTreeNodeViewModel pierManagerNodeModel = new ColumnPierRootTreeNodeViewModel()
            {
                Name = "柱式墩",
                IsSelected = false,
                IsExpanded = true,
                Children = new ObservableCollection<BaseTreeNodeViewModel>()
            };
            foreach (ColumnPierModel pierModel in BridgeManagerModel.ColumnPiers)
            {
                ColumnPierTreeNodeViewModel pierNodeModel = new ColumnPierTreeNodeViewModel(pierModel)
                {
                    Name = pierModel.Name,
                    IsSelected = false,
                    IsExpanded = true,
                    Children = new ObservableCollection<BaseTreeNodeViewModel>()
                };
                pierManagerNodeModel.Children.Add(pierNodeModel);

                for (int i = 0; i <pierModel.BlockReferenceIdCollection.Count; i++)
                {
                    InstanceTreeNodeViewModel blockReferenceNodeModel = new InstanceTreeNodeViewModel(pierModel.BlockReferenceIdCollection[i])
                    {
                        Name = $"实例{i}",
                        IsSelected = false,
                        IsExpanded = true,
                        Children = new ObservableCollection<BaseTreeNodeViewModel>()
                    };
                    pierNodeModel.Children.Add(blockReferenceNodeModel);
                }
                
            }
            treeNodes.Add(pierManagerNodeModel);
            TbeamRootTreeNodeViewModel tbeamManagerNodeModel = new TbeamRootTreeNodeViewModel()
            {
                Name = "T梁",
                IsSelected = false,
                IsExpanded = true,
                Children = new ObservableCollection<BaseTreeNodeViewModel>()
            };
            foreach (TbeamModel tbeamModel in BridgeManagerModel.Tbeams)
            {
                TbeamTreeNodeViewModel tbeamNodeModel = new TbeamTreeNodeViewModel(tbeamModel)
                {
                    Name = tbeamModel.Name,
                    IsSelected = false,
                    IsExpanded = true,
                    Children = new ObservableCollection<BaseTreeNodeViewModel>()
                };
                tbeamManagerNodeModel.Children.Add(tbeamNodeModel);
                
                for (int i = 0; i < tbeamModel.BlockReferenceIdCollection.Count; i++)
                {
                    InstanceTreeNodeViewModel blockReferenceNodeModel = new InstanceTreeNodeViewModel(tbeamModel.BlockReferenceIdCollection[i])
                    {
                        Name = $"实例{i}",
                        IsSelected = false,
                        IsExpanded = true,
                        Children = new ObservableCollection<BaseTreeNodeViewModel>()
                    };
                    tbeamNodeModel.Children.Add(blockReferenceNodeModel);
                }
            }
            treeNodes.Add(tbeamManagerNodeModel);
            BaseTreeNodeViewModel bridgeManagerNodeModel = new BaseTreeNodeViewModel()
            {
                Name = "桥梁",
                IsSelected = false,
                IsExpanded = true,
                Children = new ObservableCollection<BaseTreeNodeViewModel>()
            };
            foreach (BridgeModel bridgeModel in BridgeManagerModel.Bridges)
            {
                BaseTreeNodeViewModel bridgeNodeModel = new BaseTreeNodeViewModel()
                {
                    Name = bridgeModel.Name,
                    IsSelected = false,
                    IsExpanded = true,
                    Children = new ObservableCollection<BaseTreeNodeViewModel>()
                };
                foreach (ColumnPierModel pierModel in bridgeModel.Piers)
                {
                    BaseTreeNodeViewModel pierNodeModel = new BaseTreeNodeViewModel()
                    {
                        Name = pierModel.Name,
                        IsSelected = false,
                        IsExpanded = true,
                        Children = new ObservableCollection<BaseTreeNodeViewModel>()
                    };
                    bridgeNodeModel.Children.Add(pierNodeModel);
                }
                bridgeManagerNodeModel.Children.Add(bridgeNodeModel);
            }
            treeNodes.Add(bridgeManagerNodeModel);
            return treeNodes;
        }
    }
}