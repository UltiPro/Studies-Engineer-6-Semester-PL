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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace MusicVibes.Pages
{
    public partial class SettingsPage : Page
    {

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void LanguageButton_Pl(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "pl-PL";
            Properties.Settings.Default.Save();
            System.Windows.MessageBox.Show("Aby zmienić język, proszę ponownie uruchomić aplikację.", "Zmiana języka", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LanguageButton_En(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "en-US";
            Properties.Settings.Default.Save();
            System.Windows.MessageBox.Show("To change the language, please restart the application.", "Language Change", MessageBoxButton.OK, MessageBoxImage.Information);          
        }

        private void ThemeButton_Light(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Resources["BackgroundColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0b1cb"));
            System.Windows.Application.Current.Resources["BackgroundColor2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#be95c4"));
            System.Windows.Application.Current.Resources["BackgroundColor3"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9f86c0"));
            System.Windows.Application.Current.Resources["WhiteSpecial"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ecf8f8"));

            System.Windows.Application.Current.Resources["FontColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#231942"));
            System.Windows.Application.Current.Resources["FontColor2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f72585"));

            System.Windows.Application.Current.Resources["BorderColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#231942"));
            SaveThemeColorsToFile();
        }

        private void ThemeButton_Dark(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Resources["BackgroundColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0C134F"));
            System.Windows.Application.Current.Resources["BackgroundColor2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D267D"));
            System.Windows.Application.Current.Resources["BackgroundColor3"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5C469C"));
            System.Windows.Application.Current.Resources["WhiteSpecial"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#635985"));

            System.Windows.Application.Current.Resources["FontColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#bb86fc"));
            System.Windows.Application.Current.Resources["FontColor2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6200ee"));

            System.Windows.Application.Current.Resources["BorderColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#bb86fc"));
            SaveThemeColorsToFile();
        }

        private void SaveThemeColorsToFile()
        {
            string filePath = "theme.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(((SolidColorBrush)System.Windows.Application.Current.Resources["BackgroundColor1"]).Color.ToString());
                    writer.WriteLine(((SolidColorBrush)System.Windows.Application.Current.Resources["BackgroundColor2"]).Color.ToString());
                    writer.WriteLine(((SolidColorBrush)System.Windows.Application.Current.Resources["BackgroundColor3"]).Color.ToString());
                    writer.WriteLine(((SolidColorBrush)System.Windows.Application.Current.Resources["WhiteSpecial"]).Color.ToString());
                    writer.WriteLine(((SolidColorBrush)System.Windows.Application.Current.Resources["FontColor1"]).Color.ToString());
                    writer.WriteLine(((SolidColorBrush)System.Windows.Application.Current.Resources["FontColor2"]).Color.ToString());
                    writer.WriteLine(((SolidColorBrush)System.Windows.Application.Current.Resources["BorderColor1"]).Color.ToString());
                }

                Console.WriteLine("Udało się zapisać kolory");
            }
            catch (Exception ex)
            {
                Console.WriteLine("nie udało się zapisać kolorów");
            }
        }
    }
}

