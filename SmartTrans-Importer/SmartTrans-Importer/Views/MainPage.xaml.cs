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
        private DriverDB db;
        public Calculator calc;
        private BackgroundWorker worker = new BackgroundWorker();

        public MainPage(DriverDB DB)
        {
            InitializeComponent();

            if (db == null)
            {
                db = new DriverDB();
            }
            else
            {
                db = DB;
            }

            calc = new Calculator();

            comboAgents.ItemsSource = db.GetDrivers();
            comboAgents.SelectedValuePath = "AgentId";
            comboAgents.DisplayMemberPath = "Name";

            Submit.Click += (s, e) => Submit_Click(s, e);

            lbl_Result.Visibility = Visibility.Hidden;
            groupBox1.Visibility = Visibility.Hidden;
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new Options());
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<string>(s => lbl_Result.Content = s);
            await Task.Factory.StartNew(() => Calculate(progress),
                                        TaskCreationOptions.LongRunning);

        }

        private void Calculate(IProgress<string> progress)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (AgentRunDate.SelectedDate != null)
                {

                    calc = calc.Calculate((DateTime)AgentRunDate.SelectedDate, comboAgents.SelectedValue.ToString());

                    calc.ComputeFields(db);

                    Exporter.ExporttoCsv(calc);

                    lbl_Result.Content = "Success. File Saved to:";
                    lbl_Result3.Content =
                        Core.Exporter.GetFileName(calc.CollectRecords[0].Driver, calc.CollectRecords[0].Date, Core.Settings.Default.CsvExportLocation);

                    lbl_Result.Visibility = Visibility.Visible;
                    groupBox1.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("No Date", "Please select a Date");
                    lbl_Result.Content = "Failed!, No Date Selected.";
                    lbl_Result.Visibility = Visibility.Visible;
                }
            }));

        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new About());
        }

        private void Agents_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new Agents(db));
        }
    }
}
