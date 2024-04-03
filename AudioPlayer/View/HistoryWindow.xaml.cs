﻿using AudioPlayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>

    public partial class HistoryWindow : Window
    {
        public HistoryWindow(List<string> historyListNames)
        {
            InitializeComponent();
            DataContext = new HistoryViewModel(historyListNames);
        }
    }


}
