using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Tool.Properties;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CadBridgeDesigner.Tool.Bridge.Beam.Tbeam.View
{
    public class TbeamEditViewModel : INotifyPropertyChanged
    {
        private TbeamModel _tbeamModel;
        public TbeamModel TbeamModel
        {
            get => _tbeamModel;
            private set
            {
                _tbeamModel = value;
                OnPropertyChanged();
            }
        }
        private void OnTbeamModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            document.TbeamUpdate(TbeamModel);
        }
        public TbeamEditViewModel(TbeamModel tbeamModel)
        {
            TbeamModel = tbeamModel;
            TbeamModel.PropertyChanged += OnTbeamModelPropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
