using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.CompilerServices;


namespace AudioPlayer.ViewModel
{
    internal class HistoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Change([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public HistoryViewModel(List<string> history)
        {
            History = history;
        }

        private List<string> history;

        public List<string> History
        {
            get { return history; }
            set { history = value; }
        }


    }
}
