using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.AutoCAD.DatabaseServices;
using CadBridgeDesigner.Core.Properties;

namespace CadBridgeDesigner.Tool.Bridge.Base
{
    public enum BridgeComponentType
    {
        ColumnPier = 0,
        Tbeam = 1
    }
    public abstract class BridgeComponentModel: INotifyPropertyChanged
    {
        private BridgeComponentType _type;
        public BridgeComponentType Type 
        { 
            get => _type;
            set 
            { _type = value; 
                OnPropertyChanged(); 
            } 
        }
        
        private string _name;
        public string Name 
        { 
            get => _name;
            set 
            { _name = value; 
                OnPropertyChanged(); 
            } 
        }
        
        private double _mileage;
        public double Mileage 
        { 
            get => _mileage;
            set 
            { _mileage = value; 
                OnPropertyChanged(); 
            } 
        }
        
        private ObjectId _blockTableRecordId;
        public ObjectId BlockTableRecordId 
        { 
            get => _blockTableRecordId;
            set 
            { _blockTableRecordId = value; 
                OnPropertyChanged(); 
            } 
        }

        private string _blockTableRecordName;
        public string BlockTableRecordName 
        { 
            get => _blockTableRecordName;
            set 
            { _blockTableRecordName = value; 
                OnPropertyChanged(); 
            } 
        }

        private ObjectIdCollection _blockReferenceIdCollection;
        public ObjectIdCollection BlockReferenceIdCollection 
        { 
            get => _blockReferenceIdCollection;
            set 
            { _blockReferenceIdCollection = value; 
                OnPropertyChanged(); 
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

