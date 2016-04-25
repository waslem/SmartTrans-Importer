using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartTrans_Importer.Views
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Page
    {
        public Options()
        {
            InitializeComponent();

            lbl_Current.Content = Core.Settings.Default.CsvExportLocation;
            txt_ApiUrl.Text = Core.Settings.Default.ApiUrl;
            txt_eSolLogin.Text = Core.Settings.Default.eSolutionsLogin;
            txt_eSolPass.Text = Core.Settings.Default.eSolutionsPassword;
            txt_proxyAdd.Text = Core.Settings.Default.ProxyAddress;
            txt_ProxyPort.Text = Core.Settings.Default.ProxyPort.ToString();

        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new MainPage(null));
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            int newPort = 0;

            if (Int32.TryParse(txt_ProxyPort.Text, out newPort))
            {
                Core.Settings.Default.ApiUrl = txt_ApiUrl.Text;
                Core.Settings.Default.eSolutionsLogin = txt_eSolLogin.Text;
                Core.Settings.Default.eSolutionsPassword = txt_eSolPass.Text;
                Core.Settings.Default.ProxyAddress = txt_proxyAdd.Text;
                Core.Settings.Default.ProxyPort = newPort.ToString();

                base.NavigationService.Navigate(new MainPage(null));
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid Proxy port, please enter a valid port.");
            }     
        }

        private void btn_SetLocation_Click(object sender, RoutedEventArgs e)
        {
            var exportDialog = new FolderBrowserDialog();

            DialogResult result = exportDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                // save new location
                Core.Settings.Default.CsvExportLocation = exportDialog.SelectedPath;
                lbl_Current.Content = Core.Settings.Default.CsvExportLocation;
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new About());
        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
