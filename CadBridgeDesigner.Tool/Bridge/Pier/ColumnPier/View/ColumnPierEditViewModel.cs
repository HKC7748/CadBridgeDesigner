using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Tool.Properties;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier.View
{
    public class ColumnPierEditViewModel : INotifyPropertyChanged
    {
        private ColumnPierModel _columnPierModel;
        public ColumnPierModel ColumnPierModel
        {
            get => _columnPierModel;
            private set
            {
                _columnPierModel = value;
                OnPropertyChanged();
            }
        }
        private void OnColumnPierModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            ColumnPierTool.ColumnPierUpdate(document, ColumnPierModel);
        }
        
        public ColumnPierEditViewModel(ColumnPierModel columnPierModel)
        {
            ColumnPierModel = columnPierModel;
            ColumnPierModel.PropertyChanged += OnColumnPierModelPropertyChanged;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}