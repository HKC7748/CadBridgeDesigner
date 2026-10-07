using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace CadBridgeDesigner.Tool.BridgeManager
{
    public partial class BridgeManagerView : UserControl
    {
        public BridgeManagerView(BridgeManagerViewModel bridgeManagerViewModel)
        {
            InitializeComponent();
            DataContext = bridgeManagerViewModel;
        }
    }
}

