using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace MusicVibes
{
    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e)
        {     
            var langCode = MusicVibes.Properties.Settings.Default.languageCode;
            LoadThemeColorsFromFile();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(langCode);
            base.OnStartup(e);
        }

        private void LoadThemeColorsFromFile()
        {
            string filePath = "theme.txt";

            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);

                    if (lines.Length >= 7)
                    {
                        Color backgroundColor1 = (Color)ColorConverter.ConvertFromString(lines[0]);
                        Color backgroundColor2 = (Color)ColorConverter.ConvertFromString(lines[1]);
                        Color backgroundColor3 = (Color)ColorConverter.ConvertFromString(lines[2]);
                        Color whiteSpecial = (Color)ColorConverter.ConvertFromString(lines[3]);
                        Color fontColor1 = (Color)ColorConverter.ConvertFromString(lines[4]);
                        Color fontColor2 = (Color)ColorConverter.ConvertFromString(lines[5]);
                        Color borderColor1 = (Color)ColorConverter.ConvertFromString(lines[6]);

                        Application.Current.Resources["BackgroundColor1"] = new SolidColorBrush(backgroundColor1);
                        Application.Current.Resources["BackgroundColor2"] = new SolidColorBrush(backgroundColor2);
                        Application.Current.Resources["BackgroundColor3"] = new SolidColorBrush(backgroundColor3);
                        Application.Current.Resources["WhiteSpecial"] = new SolidColorBrush(whiteSpecial);
                        Application.Current.Resources["FontColor1"] = new SolidColorBrush(fontColor1);
                        Application.Current.Resources["FontColor2"] = new SolidColorBrush(fontColor2);
                        Application.Current.Resources["BorderColor1"] = new SolidColorBrush(borderColor1);

                        Console.WriteLine("Udało się odczytać kolory");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nie udało się odczytać kolorów");
            }
        }


    }
}
