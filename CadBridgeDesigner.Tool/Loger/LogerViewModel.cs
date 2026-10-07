using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CadBridgeDesigner.Tool.Loger
{
    public sealed class LogerViewModel: INotifyPropertyChanged
    {
        private LogerModel _logerModel;
        public LogerModel LogerModel
        {
            get => _logerModel;
            set
            {
                _logerModel = value;
                OnPropertyChanged();
            }
        }

        public LogerViewModel(LogerModel logerModel)
        {
            _logerModel = logerModel;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


