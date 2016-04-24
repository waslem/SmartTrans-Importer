using SmartTrans_Importer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SmartTrans_Importer.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public DriverDB db;
        public Calculator calc;
        private BackgroundWorker worker = new BackgroundWorker();

        public MainPage()
        {
            InitializeComponent();

            db = new DriverDB();
            calc = new Calculator();

            comboAgents.ItemsSource = db.GetDrivers();
            comboAgents.SelectedValuePath = "AgentId";
            comboAgents.DisplayMemberPath = "Name";

            Submit.Click += (s, e) => Submit_Click(s, e);

            lbl_dl.Visibility = Visibility.Hidden;
            lbl_calc.Visibility = Visibility.Hidden;
            lbl_export.Visibility = Visibility.Hidden;
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new Options());
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<string>(s => lbl_dl.Content = s);
            await Task.Factory.StartNew(() => Calculate(progress),
                                        TaskCreationOptions.LongRunning);
            lbl_dl.Content = "completed";
        }

        private void Calculate(IProgress<string> progress)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (AgentRunDate.SelectedDate != null)
                {
                    progress.Report("Downloading data...");
                    calc = calc.Calculate((DateTime)AgentRunDate.SelectedDate, comboAgents.SelectedValue.ToString());
                    progress.Report("Calculating fields...");
                    calc.ComputeFields(db);
                    progress.Report("Exporting data...");
                    Exporter.ExporttoCsv(calc);

                    //MessageBox.Show("File Exported to:" + Properties.Settings.Default.CsvExportLocation.ToString());
                }
                else
                {
                    MessageBox.Show("No Date", "Please select a Date");
                }
            }));

        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new About());
        }
    }
}
