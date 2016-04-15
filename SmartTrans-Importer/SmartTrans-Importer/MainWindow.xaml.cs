using SmartTrans_Importer.Core;
using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();

            db = new DriverDB();

            comboAgents.ItemsSource = db.GetDrivers();
            comboAgents.SelectedValuePath = "AgentId";
            comboAgents.DisplayMemberPath = "Name";

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.SaveDrivers();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (AgentRunDate.SelectedDate != null)
            {
                calc = new Calculator((DateTime)AgentRunDate.SelectedDate, comboAgents.SelectedValue.ToString());
                calc.ComputeFields(db);
                Exporter.ExporttoCsv(calc);
            }
            else
            {
                MessageBox.Show("No Date", "Please select a Date");
            }
        }
    }
}
