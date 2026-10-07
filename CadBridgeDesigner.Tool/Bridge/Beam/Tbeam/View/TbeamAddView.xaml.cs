using System.ComponentModel;
using System.Windows;

namespace CadBridgeDesigner.Tool.Bridge.Beam.Tbeam.View
{
    public partial class TbeamAddView : Window
    {
        public TbeamAddView(TbeamAddViewModel tbeamAddViewModel)
        {
            InitializeComponent();
            DataContext = tbeamAddViewModel;
            tbeamAddViewModel.PropertyChanged += OnViewModelResultChanged;
        }
        private void OnViewModelResultChanged(object sender, PropertyChangedEventArgs e)
        {
            DialogResult = ((TbeamAddViewModel)DataContext).Result;
        }
    }
}