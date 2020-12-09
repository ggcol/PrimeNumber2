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
using PrimeNumber2.Models;
using Microsoft.Win32;
using System.IO;

namespace PrimeNumber2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Number n = new Number();
            //Task.Run(new Action(() => n.ContinuosCalc()));
            BgCalcAsync();
        }

        public async void BgCalcAsync()
        {
            Number n = new Number();
            await Task.Run(new Action(() => n.ContinuosCalc()));
        }

        /// <summary>
        /// Collapse other views
        /// </summary>
        /// <param name="except">The View to show</param>
        private void CollapseOtherViews(string except)
        {
            switch (except)
            {
                default:
                    Welcome.Visibility = Visibility.Visible;
                    checkPrimeGrid.Visibility = Visibility.Collapsed;
                    insertGridLP.Visibility = Visibility.Collapsed;
                    showGridLP.Visibility = Visibility.Collapsed;
                    nthPrimeNumberGrid.Visibility = Visibility.Collapsed;
                    break;
                case "checkPrimeGrid":
                    Welcome.Visibility = Visibility.Collapsed;
                    checkPrimeGrid.Visibility = Visibility.Visible;
                    insertGridLP.Visibility = Visibility.Collapsed;
                    showGridLP.Visibility = Visibility.Collapsed;
                    nthPrimeNumberGrid.Visibility = Visibility.Collapsed;
                    break;
                case "insertGridLP":
                    Welcome.Visibility = Visibility.Collapsed;
                    checkPrimeGrid.Visibility = Visibility.Collapsed;
                    insertGridLP.Visibility = Visibility.Visible;
                    showGridLP.Visibility = Visibility.Collapsed;
                    nthPrimeNumberGrid.Visibility = Visibility.Collapsed;
                    break;
                case "showGridLP":
                    Welcome.Visibility = Visibility.Collapsed;
                    checkPrimeGrid.Visibility = Visibility.Collapsed;
                    insertGridLP.Visibility = Visibility.Collapsed;
                    showGridLP.Visibility = Visibility.Visible;
                    nthPrimeNumberGrid.Visibility = Visibility.Collapsed;
                    break;
                case "nthPrimeNumberGrid":
                    Welcome.Visibility = Visibility.Collapsed;
                    checkPrimeGrid.Visibility = Visibility.Collapsed;
                    insertGridLP.Visibility = Visibility.Collapsed;
                    showGridLP.Visibility = Visibility.Collapsed;
                    nthPrimeNumberGrid.Visibility = Visibility.Visible;
                    break;

            }
        }

        private void CheckPrime_Click(object sender, RoutedEventArgs e)
        {
            
            CollapseOtherViews("checkPrimeGrid");
           
        }

        private void GOcheckPrime_Click(object sender, RoutedEventArgs e)
        {
            Number p = new Number
            {
                IDN = Convert.ToInt64(insertPrime.Text)
            };

            if (p.IsPrime(p.IDN))
            {
                showIsPrime.Text = "Is Prime!";
            } else
            {
                showIsPrime.Text = "Not Prime!";
            }
        }

        private void SAVEcheckPrime_Click(object sender, RoutedEventArgs e)
        {
            string toSave = $"{insertPrime.Text}, {showIsPrime.Text}";

            Save("MyPrimeNumber", toSave, showIsPrime);
            
        }

        private void ListPrime_Click(object sender, RoutedEventArgs e)
        {
            CollapseOtherViews("insertGridLP");
        }


        private void GOlistPrime_Click(object sender, RoutedEventArgs e)
        {
            
            CollapseOtherViews("showGridLP");
            
            long lowerBound = Convert.ToInt64(insertLowerBound.Text);
            long upperBound = Convert.ToInt64(insertUpperBound.Text);

            Number p = new Number();

            List<string> read = p.ListOfPrime(lowerBound, upperBound);
        
            showLP.Text = string.Join(", ", read);
        }

        private void SAVElistPrime_Click(object sender, RoutedEventArgs e)
        {
            string toSave = $"{showLP.Text}";

            Save("MyPrimeNumberList", toSave, showLP);

        }

        private void toMain_Click(object sender, RoutedEventArgs e)
        {
            CollapseOtherViews(default);
        }

        private async void lastCalculated_LayoutUpdatedAsync(object sender, EventArgs e)
        {
            long lastPrime = 0; 
            Number n = new Number();
            Action a = new Action(() => {
                lastPrime = n.RetrieveLastPrimeCalc();
            });
                
            //Task retrieve = new Task(a);
            await Task.Run(a);

            lastCalculated.Content = $"Last prime number calculated: {lastPrime}";
        }

        private void nthPrimeNumber_Click(object sender, RoutedEventArgs e)
        {
            CollapseOtherViews("nthPrimeNumberGrid");
        }

        private void GOcheckNth_Click(object sender, RoutedEventArgs e)
        {
            //do sth
        }

        private void SAVEnthPrime_Click(object sender, RoutedEventArgs e)
        {
            //do sth to save
        }

        private void Save(string defaultName, string toSave, TextBlock t)
        {
            //definisce finestra dialogo Win
            Microsoft.Win32.SaveFileDialog sdlg = new Microsoft.Win32.SaveFileDialog();
            sdlg.FileName = defaultName;
            sdlg.DefaultExt = ".txt";
            sdlg.Filter = "Text documents(.txt)|*.txt";

            //mostra finestra
            Nullable<bool> show = sdlg.ShowDialog();

            string fileName = null;

            if (show == true)
            {
                //ottiene path completa di filename da usr
                fileName = sdlg.FileName;
            }

            //se path != null salva file
            if (fileName != null)
            {
                File.WriteAllText(fileName, $"{toSave} ");
            }
            else
            {
                t.Text = "Impossibile salvare";
            }
        }
    }
}
