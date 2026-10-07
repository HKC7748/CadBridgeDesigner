using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier;

namespace CadBridgeDesigner.Tool.BridgeManager
{
    public class BridgeManagerModel: INotifyPropertyChanged
    {
        private static ObservableCollection<ColumnPierModel> _columnPiers;

        public static ObservableCollection<ColumnPierModel> ColumnPiers
        {
            get => _columnPiers;
            set
            {
                _columnPiers = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(ColumnPiers)));
            }
        }
        
        private static ObservableCollection<TbeamModel> _tbeams;
        
        public static ObservableCollection<TbeamModel> Tbeams
        {
            get => _tbeams;
            set
            {
                _tbeams = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Tbeams)));
            }
        }

        private static ObservableCollection<Bridge.BridgeModel> _bridges;
        
        public static ObservableCollection<Bridge.BridgeModel> Bridges
        {
            get => _bridges;
            set
            {
                _bridges = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Bridges)));
            }
        }

        public BridgeManagerModel()
        {
            ColumnPiers = new ObservableCollection<ColumnPierModel>();
            Tbeams = new ObservableCollection<TbeamModel>();
            Bridges = new ObservableCollection<Bridge.BridgeModel>();
        }
        
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


