using System;
using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Core.ElementHostManager;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam.View;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.BridgeManager.TreeNode.Tbeam
{
    public class TbeamTreeNodeViewModel: BaseTreeNodeViewModel
    {
        private TbeamModel _tbeamModel;

        private TbeamModel TbeamModel
        {
            get => _tbeamModel;
            set
            {
                _tbeamModel = value;
                OnPropertyChanged();
            }
        }
        
        protected override void OnNodeClick()
        {
            TbeamEditViewModel tbeamEditViewModel = new TbeamEditViewModel(TbeamModel);
            TbeamEditView tbeamEditView = new TbeamEditView(tbeamEditViewModel);
            ElementHostManager.PropertiesEditorElementHost.Child = tbeamEditView;
        }
        
        
        private RelayCommand InsertInstanceCommand { get; set; }

        private void AddInstance()
        {
            try
            {
                LogerTool.Log("[AddInstance]添加实例");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[AddInstance]文档为空，添加失败");
                    return;
                }
                TbeamModel.AddTbeamInstance(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddInstance]" + exception.GetDetail());
            }
        }
        
        private RelayCommand AddTbeamPointCrossSectionDiagramsCommand { get; set; }
        private void AddTbeamPointCrossSectionDiagrams()
        {
            try
            {
                LogerTool.Log("[AddTbeamPointCrossSectionDiagrams]绘制中梁支点截面");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[AddTbeamPointCrossSectionDiagrams]文档为空，添加失败");
                    return;
                }

                TbeamModel.AddTbeamPointCrossSectionDiagrams(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddTbeamPointCrossSectionDiagrams]" + exception.GetDetail());
            }
        }

        private RelayCommand AddTbeamMidspanCrossSectionDiagramsCommand { get; set; }
        private void AddTbeamMidspanCrossSectionDiagrams()
        {
            try
            {
                LogerTool.Log("[AddTbeamMidspanCrossSectionDiagrams]绘制中梁跨中截面");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[AddTbeamMidspanCrossSectionDiagrams]文档为空，添加失败");
                    return;
                }
                TbeamModel.AddTbeamMidspanCrossSectionDiagrams(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddTbeamMidspanCrossSectionDiagrams]" + exception.GetDetail());
            }
        }
        
        private RelayCommand AddTbeamPointEdgeSectionDiagramsCommand { get; set; }
        private void AddTbeamPointEdgeSectionDiagrams()
        {
            try
            {
                LogerTool.Log("[AddTbeamPointEdgeSectionDiagrams]绘制边梁支点截面");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[AddTbeamPointEdgeSectionDiagrams]文档为空，添加失败");
                    return;
                }
                TbeamModel.AddTbeamPointEdgeSectionDiagrams(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddTbeamPointEdgeSectionDiagrams]" + exception.GetDetail());
            }
        }
        
        private RelayCommand AddTbeamPointMiddleDiagramsCommand { get; set; }
        private void AddTbeamPointMiddleDiagrams()
        {
            try
            {
                LogerTool.Log("[AddTbeamPointMiddleDiagrams]绘制中梁支点截面");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[AddTbeamPointMiddleDiagrams]文档为空，添加失败");
                    return;
                }

                TbeamModel.AddTbeamPointMiddleSectionDiagrams(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddTbeamPointMiddleDiagrams]" + exception.GetDetail());
            }
        }
        
        
        private RelayCommand AddTbeamMidspanEdgeDiagramsCommand { get; set; }
        private void AddTbeamMidSpanEdgeDiagrams()
        {
            try
            {
                LogerTool.Log("[AddTbeamMidspanEdgeDiagramsCommand]绘制边梁跨中截面");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[AddTbeamMidspanEdgeDiagramsCommand]文档为空，添加失败");
                    return;
                }
                TbeamModel.AddTbeamMidspanEdgeSectionDiagrams(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddTbeamMidspanEdgeDiagramsCommand]" + exception.GetDetail());
            }
        }
        
        
        private RelayCommand AddTbeamMidspanMiddleDiagramsCommand { get; set; }
        private void AddTbeamMidSpanMiddleDiagrams()
        {
            try
            {
                LogerTool.Log("[AddTbeamMidspanMiddleDiagramsCommand]绘制中梁跨中截面");
                Document document = Application.DocumentManager.CurrentDocument;
                if (document == null)
                {
                    LogerTool.Log("[AddTbeamMidspanMiddleDiagramsCommand]文档为空，添加失败");
                    return;
                }
                TbeamModel.AddTbeamMidspanMiddleSectionDiagrams(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddTbeamMidspanMiddleDiagramsCommand]" + exception.GetDetail());
            }
        }
        
        
        public TbeamTreeNodeViewModel(TbeamModel tbeamModel)
        {
            TbeamModel = tbeamModel;
            InsertInstanceCommand = new RelayCommand(AddInstance);
            MenuItems.Add(new MenuItem("添加实例", InsertInstanceCommand));
            AddTbeamPointCrossSectionDiagramsCommand = new RelayCommand(AddTbeamPointCrossSectionDiagrams);
            MenuItems.Add(new MenuItem("绘制支点横断面", AddTbeamPointCrossSectionDiagramsCommand));
            AddTbeamPointEdgeSectionDiagramsCommand = new RelayCommand(AddTbeamPointEdgeSectionDiagrams);
            MenuItems.Add(new MenuItem("绘制支点边梁截面", AddTbeamPointEdgeSectionDiagramsCommand));
            AddTbeamPointMiddleDiagramsCommand = new RelayCommand(AddTbeamPointMiddleDiagrams);
            MenuItems.Add(new MenuItem("绘制支点中梁截面", AddTbeamPointMiddleDiagramsCommand));
            AddTbeamMidspanCrossSectionDiagramsCommand = new RelayCommand(AddTbeamMidspanCrossSectionDiagrams);
            MenuItems.Add(new MenuItem("绘制跨中横断面", AddTbeamMidspanCrossSectionDiagramsCommand));
            AddTbeamMidspanEdgeDiagramsCommand = new RelayCommand(AddTbeamMidSpanEdgeDiagrams);
            MenuItems.Add(new MenuItem("绘制跨中边梁截面", AddTbeamMidspanEdgeDiagramsCommand));
            AddTbeamMidspanMiddleDiagramsCommand = new RelayCommand(AddTbeamMidSpanMiddleDiagrams);
            MenuItems.Add(new MenuItem("绘制跨中中梁截面", AddTbeamMidspanMiddleDiagramsCommand));
        }
    }
}


