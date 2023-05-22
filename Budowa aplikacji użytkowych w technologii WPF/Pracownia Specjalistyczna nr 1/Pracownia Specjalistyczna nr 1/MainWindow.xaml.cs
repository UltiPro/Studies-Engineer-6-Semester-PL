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
using static System.Net.Mime.MediaTypeNames;

namespace Pracownia_Specjalistyczna_nr_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Maszyna maszynka;
        public MainWindow()
        {
            InitializeComponent();

            maszynka = new Maszyna();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            maszynka.Dodaj(Text1.Text);
            Label1.Content = $"Dodano tekst: {Text1.Text}, \n{ListaKuponow()}";
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            string tekst;
            if (maszynka.Kupony())
            {
                tekst = maszynka.Losuj();
                Label1.Content = $"Wyjęto tekst: {tekst}, \n{ListaKuponow()}";
            }
            else Label1.Content = "Brak elementow";
        }

        private string ListaKuponow()
        {
            string lista = "";
            maszynka.ListKupony().ForEach(x => { lista += $"\n{x}"; });
            return lista;
        }
    }
}
