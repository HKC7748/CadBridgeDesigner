using System.ComponentModel;
using System.Runtime.CompilerServices;
using CadBridgeDesigner.Tool.Properties;
using CadBridgeDesigner.Tool.Tool;

namespace CadBridgeDesigner.Tool.Bridge.Beam.Tbeam.View
{
    public sealed class TbeamAddViewModel : INotifyPropertyChanged
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

        private bool _result;
        public bool Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }
        
        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        
        public TbeamAddViewModel(TbeamModel tbeamModel)
        {
            TbeamModel = tbeamModel;
            Result = false;
            OkCommand = new RelayCommand(() => Result = true);
            CancelCommand = new RelayCommand(() => Result = false);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
