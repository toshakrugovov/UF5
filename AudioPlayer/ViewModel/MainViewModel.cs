using AudioPlayer.ViewModel.Helpers;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Timers;


namespace AudioPlayer.ViewModel
{
    class MainViewModel : BindingHelper
    {
        
        private TimeSpan position;


        public BindableCommand Play { get; set; }
        public BindableCommand Pause { get; set; }
        public BindableCommand Next { get; set; }
        public BindableCommand Back { get; set; }
        public BindableCommand VolumeChange { get; set; }
        public BindableCommand ChoiceSong { get; set; }
        public BindableCommand SoundKill { get; set; }
        public BindableCommand SoundRestart { get; set; }
        public BindableCommand Mix { get; set; }
        public BindableCommand HistoryButton { get; set; }

        public BindableCommand PlayOrPause_Click { get; set; }


         public BindableCommand volumeSlider_ValueChanged { get; set; }

        public BindableCommand MediaSlider_ValueChanged_1 { get; set; }




        public MediaPlayer mediaPlayer = new MediaPlayer();

        public Slider VolumeSlider;

        public Slider MediaSlider;

        public ListBox SongsList;

        public string nameSong;

        public Button PlayOrPause;

        public ListBox HistoryList = new ListBox();


        private int songIndex;

        public int SongIndex
        {
            get { return songIndex; }
            set
            {
                if (songIndex != value)
                {
                    songIndex = value;
                    OnPropertyChanged();
                }
            }
        }



        public string NameSong
        {
            get { return nameSong; }
            set { nameSong = value; OnPropertyChanged(); }
        }

        public List<string> historyListNames = new List<string>();




        public MainViewModel()
        {
            Play = new BindableCommand(_ => play());
            Pause = new BindableCommand(_ => pause());
            Next = new BindableCommand(_ => nextSong());
            Back = new BindableCommand(_ => backSong());
            VolumeChange = new BindableCommand(_ => volumeChange());
            ChoiceSong = new BindableCommand(_ => choiceFolder());
            SoundKill = new BindableCommand(_ => soundKill());
            SoundRestart = new BindableCommand(_ => soundRestart());
            Mix = new BindableCommand(_ => mixer());
            HistoryButton = new BindableCommand(_ => historyButton());
            volumeSlider_ValueChanged = new BindableCommand(_ => VolumeSlider_ValueChanged());
            PlayOrPause_Click = new BindableCommand(_ => playOrPause_Click());
            MediaSlider_ValueChanged_1 = new BindableCommand(_ => mediaSlider_ValueChanged_1());
           
            PlayOrPause = new Button();
            SongsList = new ListBox();
            PlayOrPause = new Button();
            VolumeSlider = new Slider();
            MediaSlider = new Slider();
            SongsList.SelectionChanged += SongsList_SelectionChanged;

            // Присоединение обработчика события MediaOpened к методу MediaPlayer_MediaOpened
            mediaPlayer.MediaOpened += (sender, e) => MediaPlayer_MediaOpened();
        }

        static int volumeLevel = 0;
        static int songindex = 0;
        static bool mix = false;
        static bool restart = false;
        static bool playOrStop;
        static string folderPath = "";
        List<string> fileNames = new List<string>();
        List<string> songsListNames = new List<string>();
        List<string> songsListNamesCopy = new List<string>();
        List<string> fileNamesCopy = new List<string>();
        string[] files;
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        Random rng = new Random();
        public WindowState WindowState;
        private ElapsedEventHandler Timer_Elapsed;


        public void MediaPlayer_MediaOpened()
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
                MediaSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.Ticks;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            // Add the current song to the history list
            historyListNames.Add(NameSong); // Assuming NameSong is a string property
            HistoryList.ItemsSource = historyListNames;
        }

        public void playOrPause_Click()
        {
            if (playOrStop == true)
            {
                pause();
            }
            else
            {
                play();
            }
        }


        private void soundKill()
        {
            NameSong = "";
            mediaPlayer.Stop();
            mediaPlayer.Close();
        }
        public void play()
        {
            playOrStop = true;
            mediaPlayer.Position = position;
            mediaPlayer.Play();
            var icon = new PackIcon { Kind = PackIconKind.Pause };
            if (PlayOrPause != null)
            {
                PlayOrPause.Content = icon;
            }
            timer.Enabled = true;
        }

        public void pause()
        {
            playOrStop = false;
            position = mediaPlayer.Position;
            mediaPlayer.Stop();
            var icon = new PackIcon { Kind = PackIconKind.Play };
            PlayOrPause.Content = icon;
            timer.Enabled = false;
        }

        private void nextSong()
        {
            if (songindex != fileNames.Count - 1)
            {
                songindex++;
                SongsList.SelectedIndex = songindex;
                mediaPlayer.Open(new Uri(folderPath + "/" + fileNames[songindex]));
                mediaPlayer.Play();
            }
            else
            {
                songindex = 0;
                SongsList.SelectedIndex = songindex;
                mediaPlayer.Open(new Uri(folderPath + "/" + fileNames[songindex]));
                mediaPlayer.Play();
            }
        }



      

        private void backSong()
        {
            if (songindex != 0)
            {
                songindex--;
                SongsList.SelectedIndex = songindex;
                mediaPlayer.Open(new Uri(folderPath + "/" + fileNames[songindex]));
                mediaPlayer.Play();
            }
            else
            {
                songindex = fileNames.Count - 1;
                SongsList.SelectedIndex = songindex;
                mediaPlayer.Open(new Uri(folderPath + "/" + fileNames[songindex]));
                mediaPlayer.Play();
            }
        }


        

        private void volumeChange()
        {
            switch (volumeLevel)
            {
                case 0:
                    volumeLevel = 1;
                    VolumeSlider.Value = 0;
                    mediaPlayer.Volume = VolumeSlider.Value / 100;
                    break;
                case 1:
                    volumeLevel = 2;
                    VolumeSlider.Value = 25;
                    mediaPlayer.Volume = VolumeSlider.Value / 100;
                    break;
                case 2:
                    volumeLevel = 3;
                    VolumeSlider.Value = 50;
                    mediaPlayer.Volume = VolumeSlider.Value / 100;
                    break;
                case 3:
                    volumeLevel = 4;
                    VolumeSlider.Value = 75;
                    mediaPlayer.Volume = VolumeSlider.Value / 100;
                    break;
                case 4:
                    volumeLevel = 0;
                    VolumeSlider.Value = 100;
                    mediaPlayer.Volume = VolumeSlider.Value / 100;
                    break;
            }
        }
        private void choiceFolder()
        {
            string extensionFilter = "*.mp3|*.m4a|*.flac|*.wav";
            clearInfo();
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                folderPath = dialog.FileName;
                files = Directory.GetFiles(dialog.FileName).Where(f => extensionFilter.Contains(System.IO.Path.GetExtension(f).ToLower())).ToArray();
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(file);
                    string fullName = System.IO.Path.GetFileName(file);
                    songsListNames.Add(name);
                    fileNames.Add(fullName);
                }

               
                foreach (string name in songsListNames)
                {
                    SongsList.Items.Add(name);
                }

                play();
            }
        }






        private void clearInfo()
        {
            folderPath = "";
            fileNames.Clear();

            if (SongsList != null)
            {
                SongsList.Items.Clear();
            }
        }



        private void soundRestart()
        {
            if (restart == false) { restart = true; SoundRestart.Foreground = new SolidColorBrush(Colors.PaleVioletRed); } else { restart = false; SoundRestart.Foreground = new SolidColorBrush(Colors.Black); }
        }

        private void mixer()
        {
            if (mix == false)
            {
                Mix.Foreground = new SolidColorBrush(Colors.PaleVioletRed);
                mix = true;
                foreach (var name in fileNames)
                {
                    fileNamesCopy.Add(name);
                }

                foreach (var name in songsListNames)
                {
                    songsListNamesCopy.Add(name);
                }

                int n = fileNamesCopy.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    string value = fileNames[k];
                    fileNames[k] = fileNames[n];
                    fileNames[n] = value;
                    value = songsListNames[k];
                    songsListNames[k] = songsListNames[n];
                    songsListNames[n] = value;
                }
                SongsList.Items.Clear();
                foreach (var name in songsListNames)
                {
                    SongsList.Items.Add(name);
                }
            }
            else
            {
                Mix.Foreground = new SolidColorBrush(Colors.Black);
                mix = false;
                fileNames.Clear();
                foreach (var name in fileNamesCopy)
                {
                    fileNames.Add(name);
                }
                SongsList.Items.Clear();
                songsListNames.Clear();
                foreach (var name in songsListNamesCopy)
                {
                    songsListNames.Add(name);
                    SongsList.Items.Add(name);
                }
                songsListNamesCopy.Clear();
                fileNamesCopy.Clear();
            }
        }

        public void historyButton()
        {
            HistoryWindow historyWindow = new HistoryWindow(historyListNames);
            historyWindow.ShowDialog();
        }


        private void SongsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SongsList.SelectedIndex != -1)
            {
                songindex = SongsList.SelectedIndex;
                play();
            }
        }




        public void mediaSlider_ValueChanged_1()
        {
            position = new TimeSpan(Convert.ToInt64(MediaSlider.Value));
            mediaPlayer.Position = new TimeSpan(Convert.ToInt64(MediaSlider.Value));
        }


      
        public void VolumeSlider_ValueChanged()
        {
            mediaPlayer.Volume = VolumeSlider.Value / 100;
        }

    




       


    }
}
 