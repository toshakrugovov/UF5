using AudioPlayer.ViewModel;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace AudioPlayer
{


  public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
           
        }

        private void SongsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataContext = new MainViewModel();
        }
    }
}
