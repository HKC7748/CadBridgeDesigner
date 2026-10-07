using System.ComponentModel;
using System.Runtime.CompilerServices;
using CadBridgeDesigner.Tool.Properties;
using CadBridgeDesigner.Tool.Tool;

namespace CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier.View
{
    public sealed class ColumnPierAddViewModel : INotifyPropertyChanged
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

        private bool _result;
        public bool Result
        {
            get => _result;
            private set
            {
                _result = value;
                OnPropertyChanged();
            }
        }
        
        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        
        public ColumnPierAddViewModel(ColumnPierModel columnPierModel)
        {
            ColumnPierModel = columnPierModel;
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