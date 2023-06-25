using System.Windows;
using System.Windows.Input;
using MusicVibes.Pages;

namespace MusicVibes;

public partial class MainWindow : Window
{
    public MainPage mainPage;
    public PlaylistsPage playlistsPage;
    public SettingsPage settingsPage;

    public MainWindow()
    {
        mainPage = new MainPage();
        playlistsPage = new PlaylistsPage();
        settingsPage = new SettingsPage();
        InitializeComponent();
        MainFrame.Content = mainPage;
        playlistsPage.onPlaylistChange += OpenFolderFromPlaylists;
    }

    private void App_MouseDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) DragMove(); }

    private void MainButton_Click(object sender, RoutedEventArgs e) => MainFrame.Content = mainPage;

    private void OpenFolderButton_Click(object sender, RoutedEventArgs e) { if (mainPage.LoadFilesFromDialog()) MainFrame.Content = mainPage; }

    private void PlaylistsButton_Click(object sender, RoutedEventArgs e) => MainFrame.Content = playlistsPage;

    private void SettingsButton_Click(object sender, RoutedEventArgs e) => MainFrame.Content = settingsPage;

    private void QuitButton_Click(object sender, RoutedEventArgs e)
    {
        mainPage.stopApp = true;
        Close();
    }
    public void OpenFolderFromPlaylists(object sender, RoutedEventArgs e, string path) { if (mainPage.LoadFilesFromPlaylists(path)) MainFrame.Content = mainPage; }
}
