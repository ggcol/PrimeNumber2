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
using BL;
using DAL;
using CsvHelper;
using System.Globalization;

namespace PrimeNumber2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Models.PrimeNumber n = new Models.PrimeNumber();
        Op op = new Op();
        DbGnome gnome = new DbGnome();

        public MainWindow()
        {
            InitializeComponent();
            BgCalcAsync();
        }

        /// <summary>
        /// chiama il calcolo continuo di nuovi numeri primi in maniera asincrona
        /// </summary>
        public async void BgCalcAsync()
        {
            await Task.Run(new Action(() => op.ContinuosCalc()));
        }

        /// <summary>
        /// mostra attuale avanzamento calcolo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lastCalculated_LayoutUpdatedAsync(object sender, EventArgs e)
        {
            long lastPrime = 0;
            Action a = new Action(() =>
            {
                lastPrime = gnome.RetrieveLastPrimeCalc().IDN;
            });

            await Task.Run(a);

            lastCalculated.Content = $"Last prime number calculated: {lastPrime}"; 
        }


        #region viewMethods

        private void CheckPrime_Click(object sender, RoutedEventArgs e)
        {
            CollapseOtherViews("checkPrimeGrid");
        }

        private void nthPrimeNumber_Click(object sender, RoutedEventArgs e)
        {
            CollapseOtherViews("nthPrimeNumberGrid");
        }

        private void ListPrime_Click(object sender, RoutedEventArgs e)
        {
            CollapseOtherViews("insertGridLP");
        }

        private void toMain_Click(object sender, RoutedEventArgs e)
        {
            CollapseOtherViews(default);
        }

        #endregion viewMethods

        //used with button to show a result
        #region goMethods
        private async void GOlistPrime_ClickAsync(object sender, RoutedEventArgs e)
        {

            CollapseOtherViews("showGridLP");

            long lowerBound = Convert.ToInt64(insertLowerBound.Text);
            long upperBound = Convert.ToInt64(insertUpperBound.Text);

            List<PrimeNumber> read = new List<PrimeNumber>();

            showLP.Text = "ready... steady...";

            string msg = "";

            await Task.Run(new Action(() =>
            {
                read = gnome.ListOfPrimeMod(lowerBound, upperBound, out msg);

            }));

            if (msg == null)
            {
                showLP.Text = string.Join(", ", read.Select(p => p.IDN));
            } else
            {
                showLP.Text = msg;
            }
            


        }

        private void GOcheckPrime_Click(object sender, RoutedEventArgs e)
        {
            n.IDN = Convert.ToInt64(insertPrime.Text);

            if (op.IsPrime(n.IDN))
            {
                showIsPrime.Text = "Is Prime!";
            }
            else
            {
                showIsPrime.Text = "Not Prime!";
            }
        }

        private async void GOcheckNth_ClickAsync(object sender, RoutedEventArgs e)
        {
            long userNth = Convert.ToInt32(insertNthPrime.Text);
            long max = gnome.RetrieveLastPrimeCalc().IDN;

            if (userNth < 1)
            {
                showNthPrime.Text = "Try with n >= 1";
            }
            else if (userNth > max)
            {
                showNthPrime.Text = $"Try with n < {max}";
            }
            else
            {
                showNthPrime.Text = "ready... steady...";

                string retrieved = null;

                await Task.Run(new Action(() =>
                {
                    retrieved = gnome.RetrieveNthPrime(userNth).IDN.ToString();
                }));

                showNthPrime.Text = retrieved;
            }

        }
        #endregion goMethods

        #region savingMethods
        private void SAVEcheckPrime_Click(object sender, RoutedEventArgs e)
        {
            string toSave = $"{insertPrime.Text}, {showIsPrime.Text}";

            SaveTxtWithDialog("MyPrimeNumber", toSave, showIsPrime);

        }

        private void SAVElistPrime_ClickAsTxt(object sender, RoutedEventArgs e)
        {
            string toSave = $"{showLP.Text}";

            SaveTxtWithDialog("MyPrimeNumberList", toSave, showLP);

        }

        private void SAVElistPrimeAsCsv_Click(object sender, RoutedEventArgs e)
        {
            long upperBound = Convert.ToInt64(insertUpperBound.Text);
            long lowerBound = Convert.ToInt64(insertLowerBound.Text);

            string msg;

            List<PrimeNumber> list = gnome.ListOfPrimeMod(lowerBound, upperBound, out msg);
            SaveCsvWithDialog("MyPrimeNumberList", list, showLP);
        }

        private void SAVEnthPrime_Click(object sender, RoutedEventArgs e)
        {
            string toSave = $"{insertNthPrime.Text}th prime number is: {showNthPrime.Text}";

            SaveTxtWithDialog("MyN-thPrimeNumber", toSave, showNthPrime);
        }

        private void SaventhPrimeAsCsv_Click(object sender, RoutedEventArgs e)
        {
            long userNth = Convert.ToInt32(insertNthPrime.Text);
            List<PrimeNumber> list = new List<PrimeNumber>();
            list.Add(gnome.RetrieveNthPrime(userNth));
            SaveCsvWithDialog("MyN-thPrimeNumber", list, showNthPrime);
        }
        #endregion savingMethods 

        #region helpers
        private void SaveTxtWithDialog(string defaultName, string toSave, TextBlock t)
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

        private void SaveCsvWithDialog(string defaultName, List<PrimeNumber> list, TextBlock t)
        {
            //definisce finestra dialogo Win
            Microsoft.Win32.SaveFileDialog sdlg = new Microsoft.Win32.SaveFileDialog();
            sdlg.FileName = defaultName;
            sdlg.DefaultExt = ".csv";
            sdlg.Filter = "CSV file (.csv)|*.csv";

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
                using (var writer = new StreamWriter(fileName))

                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<PrimeNumber>();
                    csv.NextRecord();
                    csv.WriteRecords(list);
                }
            }
            else
            {
                t.Text = "Impossibile salvare";
            }
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

        #endregion helpers


    }
}
