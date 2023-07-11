using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace zad3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double bottomPrice = 0.2;
        private double basePrice;
        private double price;
        private int format = 5;
        private int amount = 0;
        private string grammageString;
        private double grammage;
        private int discount;
        private int delivery = 0;
        private bool isColorPaper = false;
        private string colorPaper;
        private string printOptionColor = "";
        private string printOption2 = "";
        private string printOptionUV = "";
        private double printOption = 1;
        private string summaryString;

        public MainWindow()
        {
            InitializeComponent();
            basePrice = bottomPrice;
            format = 5;
            myFirstGrammage.IsChecked = true;
            ChangeFormatLabel();
        }

        private void createSummaryString()
        {
            CalcPrice();
            this.summaryString = $"Przedmiot zamówienia: {amount}szt, format: A{format}, gramatura: {grammageString}g/m, {printOptionColor}{printOption2}{printOptionUV}{colorPaper}\n" +
                $"Dostawa: {delivery}zł\n" +
                $"Cena przed rabatem: {price}zł \n" +
                $"Naliczony rabat: {discount}% \n" +
                $"Cena po rabacie: {Math.Round(price - price * discount / 100, 2)}zł ";
            SummaryText.Text = summaryString;
        }

        private void CalcPrice()
        {
            double allPrice = basePrice * amount;
            if (isColorPaper) allPrice *= 1.5;
            allPrice *= grammage;
            allPrice *= printOption;
            allPrice += delivery;
            this.price = allPrice;
            this.discount = amount / 100;
            if (discount > 10) discount = 10;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void onSliderChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double tempPrice = this.bottomPrice;
            for (int i = 0; i < e.NewValue; i++) tempPrice *= 2.5;
            this.format = 5 - (int)e.NewValue;
            this.basePrice = tempPrice;
            ChangeFormatLabel();
            createSummaryString();
        }

        private void ChangeFormatLabel()
        {
            myFormatLabel.Content = $"A{format} - cena {Math.Round(basePrice, 2)}zł/szt";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (myAmount.Text != "")
            {
                this.amount = int.Parse(myAmount.Text);
                createSummaryString();
            }
        }

        private void Color_Checked(object sender, RoutedEventArgs e)
        {
            this.isColorPaper = true;
            myComboBox.IsEnabled = true;
            myComboBox.SelectedIndex = 0;
            createSummaryString();
        }

        private void Color_Unchecked(object sender, RoutedEventArgs e)
        {
            this.isColorPaper = false;
            myComboBox.IsEnabled = false;
            myComboBox.SelectedIndex = -1;
            this.colorPaper = $"";
            createSummaryString();
        }

        private void ComboBox_Changed(object sender, RoutedEventArgs e)
        {
            string color = "";
            if ((ComboBoxItem)myComboBox.SelectedItem != null)
            {
                color = ((ComboBoxItem)myComboBox.SelectedItem).Content.ToString();
            }
            this.colorPaper = $"papier w kolorze: {color}";
            createSummaryString();
        }

        private void Gramage80_Checked(object sender, RoutedEventArgs e)
        {
            this.grammage = 1;
            this.grammageString = "80";
            createSummaryString();
        }

        private void Gramage120_Checked(object sender, RoutedEventArgs e)
        {
            this.grammage = 2;
            this.grammageString = "120";
            createSummaryString();
        }

        private void Gramage200_Checked(object sender, RoutedEventArgs e)
        {
            this.grammage = 2.5;
            this.grammageString = "200";
            createSummaryString();
        }

        private void Gramage240_Checked(object sender, RoutedEventArgs e)
        {
            this.grammage = 3;
            this.grammageString = "240";
            createSummaryString();
        }

        private void PrintColor_Checked(object sender, RoutedEventArgs e)
        {
            this.printOptionColor = "Druk w kolorze, ";
            this.printOption += 0.3;
            createSummaryString();
        }

        private void PrintColor_Unchecked(object sender, RoutedEventArgs e)
        {
            this.printOptionColor = "";
            this.printOption -= 0.3;
            createSummaryString();
        }

        private void Print2_Checked(object sender, RoutedEventArgs e)
        {
            this.printOption2 = "Druk dwustronny, ";
            this.printOption += 0.5;
            createSummaryString();
        }

        private void Print2_Unchecked(object sender, RoutedEventArgs e)
        {
            this.printOption2 = "";
            this.printOption -= 0.5;
            createSummaryString();
        }

        private void PrintUV_Checked(object sender, RoutedEventArgs e)
        {
            this.printOptionUV = "lakier UV, ";
            this.printOption += 0.2;
            createSummaryString();
        }

        private void PrintUV_Unchecked(object sender, RoutedEventArgs e)
        {
            this.printOptionUV = "";
            this.printOption -= 0.2;
            createSummaryString();
        }

        private void ExpressDelivery_Checked(object sender, RoutedEventArgs e)
        {
            this.delivery += 15;
            createSummaryString();
        }

        private void ExpressDelivery_Unchecked(object sender, RoutedEventArgs e)
        {
            this.delivery -= 15;
            createSummaryString();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Zamówienie złożone");
            ResetWindow();
        }

        private void ResetWindow()
        {
            myAmount.Text = "0";
            mySlider.Value = 0;
            myFirstGrammage.IsChecked = true;
            myPrint1.IsChecked = false;
            myPrint2.IsChecked = false;
            myPrint3.IsChecked = false;
            myDeliverStandard.IsChecked = true;
            myColorPaper.IsChecked = false;
        }
    }
}