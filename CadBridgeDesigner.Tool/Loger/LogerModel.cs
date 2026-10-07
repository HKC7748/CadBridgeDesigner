using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CadBridgeDesigner.Tool.Loger
{
    public class LogerModel: INotifyPropertyChanged
    {
        private static string _logMessages;

        public static string LogMessages
        {
            get => _logMessages;
            set
            {
                _logMessages = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(LogMessages)));
            }
        }

        static LogerModel()
        {
            LogMessages = "";
        }
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


