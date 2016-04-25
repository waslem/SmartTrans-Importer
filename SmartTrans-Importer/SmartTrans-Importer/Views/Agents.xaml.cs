using SmartTrans_Importer.Core;
using SmartTrans_Importer.Core.Models;
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

namespace SmartTrans_Importer.Views
{
    /// <summary>
    /// Interaction logic for Agents.xaml
    /// </summary>
    public partial class Agents : Page
    {
        private DriverDB _db;

        public Agents(DriverDB db)
        {
            InitializeComponent();

            if (db == null)
            {
                _db = new DriverDB();
            }
            else
            {
                _db = db;
            }

            var drivers = _db.GetDrivers();
            foreach (var d in drivers)
            {
                listBox.Items.Add(d.Name);
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            _db.SaveDrivers();
            base.NavigationService.Navigate(new MainPage(_db));
        }

        private void btn_AddDriver_Click(object sender, RoutedEventArgs e)
        {
            if (txt_CollectName.Text != "" && txt_eSolutionsName.Text != "")
            {
                var newDriver = new Driver();
                newDriver.AgentId = txt_CollectName.Text;
                newDriver.Name = txt_eSolutionsName.Text;

                listBox.Items.Add(newDriver.Name);

                _db.Add(txt_eSolutionsName.Text, txt_CollectName.Text);

                txt_eSolutionsName.Text = "";
                txt_CollectName.Text = "";

                listBox.Items.Refresh();

            }
        }

        private void btn_Remove_Click(object sender, RoutedEventArgs e)
        {

            List<String> dns = new List<String>();

            // need 2 loops as cant iterate while removing, just get all selected objects
            foreach (object listbox in listBox.SelectedItems)
            {
                dns.Add(listbox.ToString());
            }

            // remove all selected
            foreach (var dn in dns)
            {
                String initials = _db.GetCollectInitials(dn);

                _db.Remove(initials);
                listBox.Items.Remove(listBox.SelectedItem);
            }
        }

        private void Agents_Menu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new Options());
        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new About());
        }
    }
}
