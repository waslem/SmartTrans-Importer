using SmartTrans_Importer.Core;
using SmartTrans_Importer.Core.Models;
using SmartTrans_Importer.Views;
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

namespace SmartTrans_Importer
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : NavigationWindow
    {
        public DriverDB db = new DriverDB();

        public Main()
        {
            InitializeComponent();
            base.NavigationService.Navigate(new MainPage(db));
        }
    }
}
