using SmartTrans_Importer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace SmartTrans_Importer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DriverDB db;
        public Calculator calc;
        private BackgroundWorker worker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            db = new DriverDB();
            calc = new Calculator();

            comboAgents.ItemsSource = db.GetDrivers();
            comboAgents.SelectedValuePath = "AgentId";
            comboAgents.DisplayMemberPath = "Name";

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            Submit.Click += (s, e) => Calculate();

            loading_Label.Visibility = Visibility.Hidden;
            loading_gif.Visibility = Visibility.Hidden;

        }

        private void Calculate()
        {
            loading_Label.Visibility = Visibility.Visible;
            loading_gif.Visibility = Visibility.Visible;

            if (AgentRunDate.SelectedDate != null)
            {
                worker.RunWorkerAsync();

                MessageBox.Show("File Exported to:" + Properties.Settings.Default.CsvExportLocation.ToString());
            }
            else
            {
                MessageBox.Show("No Date", "Please select a Date");
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loading_Label.Visibility = Visibility.Hidden;
            loading_gif.Visibility = Visibility.Hidden;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                calc = calc.Calculate((DateTime)AgentRunDate.SelectedDate, comboAgents.SelectedValue.ToString());
                calc.ComputeFields(db);

                Exporter.ExporttoCsv(calc);
            }));
        }

        //private void DoWork(object sender, DoWorkEventArgs e)
        //{
        //    this.Dispatcher.Invoke((Action)(() =>
        //    {
        //        calc = calc.Calculate((DateTime)AgentRunDate.SelectedDate, comboAgents.SelectedValue.ToString());
        //        calc.ComputeFields(db);

        //        Exporter.ExporttoCsv(calc);
        //    }));

        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.SaveDrivers();
        }

        //private void Submit_Click(object sender, RoutedEventArgs e)
        //{

        //    loading_Label.Visibility = Visibility.Visible;
        //    loading_gif.Visibility = Visibility.Visible;

        //    if (AgentRunDate.SelectedDate != null)
        //    {
        //        worker.RunWorkerAsync();
        //        //calc = new Calculator((DateTime)AgentRunDate.SelectedDate, comboAgents.SelectedValue.ToString());
        //        //calc.ComputeFields(db);
        //        //Exporter.ExporttoCsv(calc);

        //        MessageBox.Show("File Exported to:" + Properties.Settings.Default.CsvExportLocation.ToString());
        //    }
        //    else
        //    {
        //        MessageBox.Show("No Date", "Please select a Date");
        //    }
        //}
    }
}
