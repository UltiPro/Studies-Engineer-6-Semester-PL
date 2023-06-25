#pragma warning disable CS8618

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicVibes.UserControls;

public partial class MusicList : UserControl
{
    public delegate void OnMusicChange(object sender, RoutedEventArgs e, int id);
    public event OnMusicChange onMusicChange;
    private int? selectedMusic = null;

    public MusicList() => InitializeComponent();

    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (selectedMusic == null) return;
        onMusicChange(sender, e, (int)selectedMusic);
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListView? listView = sender as ListView;
        if (listView != null) selectedMusic = listView.SelectedIndex;
    }
}
