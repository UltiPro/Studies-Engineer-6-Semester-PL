#pragma warning disable CS8602, CS0168

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FBD = System.Windows.Forms;
using WMPLib;
using MusicVibes.Models;
using MusicVibes.Pages.Infrastructure;

namespace MusicVibes.Pages;

public partial class MainPage : Page
{
    private AudioPlayer audioPlayer;
    private BitmapImage stopImage = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/Pause.png", UriKind.RelativeOrAbsolute));
    private BitmapImage startImage = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/Play.png", UriKind.RelativeOrAbsolute));
    private BitmapImage muteVolume = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/VolumeMute.png", UriKind.RelativeOrAbsolute));
    private BitmapImage unmuteVolume = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/Volume.png", UriKind.RelativeOrAbsolute));
    private Thread theardRefresher;
    private List<MusicFile> deletedMusicFiles;
    private string[] extensions = new string[] { "mp3", "wav" };
    private bool isPlaying, isMute, isDragging;
    public ObservableCollection<MusicFile> musicFiles { get; } = new ObservableCollection<MusicFile>();
    public int currentId;
    public bool stopApp;
    public MainPage()
    {
        audioPlayer = new AudioPlayer(this);
        theardRefresher = new Thread(() => UpdateTrackDuration());
        deletedMusicFiles = new List<MusicFile>();
        isPlaying = false;
        isMute = false;
        isDragging = false;
        currentId = -1;
        stopApp = false;
        InitializeComponent();
        MusicList.onMusicChange += ChangeMusic;
        theardRefresher.Start();
    }
    public bool LoadFilesFromDialog()
    {
        using (FBD.FolderBrowserDialog folderBrowserDialog = new FBD.FolderBrowserDialog())
        {
            try
            {
                folderBrowserDialog.ShowDialog();
                return LoadFiles(folderBrowserDialog.SelectedPath);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
    public bool LoadFilesFromPlaylists(string path) => LoadFiles(path);
    private bool LoadFiles(string path)
    {
        try
        {
            musicFiles.Clear();
            WindowsMediaPlayerClass WMPC = new WindowsMediaPlayerClass();
            IWMPMedia iWMPMedia;
            int i = 1;
            foreach (FileInfo musicFile in new DirectoryInfo(path).GetFiles("*.*").Where(file => extensions.Contains(Path.GetExtension(file.FullName).TrimStart('.').ToLowerInvariant())))
            {
                iWMPMedia = WMPC.newMedia(musicFile.FullName);
                musicFiles.Add(new MusicFile(i, musicFile.FullName, Path.GetFileNameWithoutExtension(musicFile.Name), iWMPMedia.duration, iWMPMedia.durationString));
                i++;
            }
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    private void UpdateTrackDuration()
    {
        while (!stopApp)
        {
            Dispatcher.Invoke(() =>
            {
                if (isPlaying || DurationSlider.Value != audioPlayer.CurrentTrackDuration)
                {
                    CurrentDurationInfo.Text = audioPlayer.CurrentTrackDurationString != string.Empty ? audioPlayer.CurrentTrackDurationString : "00:00";
                    if (!isDragging) DurationSlider.Value = audioPlayer.CurrentTrackDuration;
                }
            });
            Thread.Sleep(100);
        }
    }
    public void ChangeMusic(object sender, RoutedEventArgs e, int id)
    {
        if (musicFiles.Count == 0) return;
        isPlaying = true;
        currentId = id;
        audioPlayer.Load(musicFiles[currentId].FilePath);
        NowPlaying.Text = musicFiles[currentId].FileName;
        StartStopImage.Source = stopImage;
        DurationSlider.Maximum = musicFiles[currentId].FileDuration;
        DurationSlider.Value = 0.0d;
        DurationInfo.Text = musicFiles[currentId].FileDurationString;
    }
    private void SearchTextChanged(object sender, TextChangedEventArgs e)
    {
        foreach (MusicFile musicFile in deletedMusicFiles) musicFiles.Insert(musicFile.FileId - 1, musicFile);
        deletedMusicFiles = musicFiles.ToList().Where(e => !e.FileName.ToLower().Contains((sender as TextBox).Text.ToLower())).ToList();
        foreach (MusicFile musicFile in deletedMusicFiles) musicFiles.RemoveAt(musicFiles.IndexOf(musicFile));
    }
    private void SkipStart(object sender, RoutedEventArgs e) => ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : !audioPlayer.IsJustStarted() ? currentId : --currentId < 0 ? (musicFiles.Count - 1) : currentId);
    private void Skip10Start(object sender, RoutedEventArgs e) => audioPlayer.SkipTrack(currentId == -1 ? 0 : -10);
    private void StartPauseMusic(object sender, RoutedEventArgs e)
    {
        if (currentId == -1) ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : 0);
        else
        {
            isPlaying = !isPlaying;
            if (isPlaying)
            {
                audioPlayer.Play();
                StartStopImage.Source = stopImage;
            }
            else
            {
                audioPlayer.Pause();
                StartStopImage.Source = startImage;
            }
        }
    }
    private void Skip10End(object sender, RoutedEventArgs e) => audioPlayer.SkipTrack(currentId == -1 ? 0 : 10);
    public void SkipEnd(object sender, RoutedEventArgs e) => ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : ++currentId >= musicFiles.Count ? 0 : currentId);
    private void DurationChangedStart(object sender, DragStartedEventArgs e) => isDragging = true;
    private void DurationChangedEnd(object sender, DragCompletedEventArgs e)
    {
        isDragging = false;
        audioPlayer.CurrentTrackDuration = DurationSlider.Value;
    }
    private void MuteUnmuteVolume(object sender, RoutedEventArgs e)
    {
        isMute = !isMute;
        audioPlayer.Mute(isMute);
        if (isMute) VolumeImage.Source = muteVolume;
        else VolumeImage.Source = unmuteVolume;
    }
    private void VolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => audioPlayer.ChangeVolume(Convert.ToInt32((sender as Slider).Value));
    private void ControlMouseEnter(object sender, MouseEventArgs e) => (sender as Button).BeginAnimation(Button.OpacityProperty, new DoubleAnimation(1.0d, 0.65d, TimeSpan.FromSeconds(0.2d)));
    private void ControlMouseLeave(object sender, MouseEventArgs e) => (sender as Button).BeginAnimation(Button.OpacityProperty, new DoubleAnimation(0.65d, 1d, TimeSpan.FromSeconds(0.2d)));
}
