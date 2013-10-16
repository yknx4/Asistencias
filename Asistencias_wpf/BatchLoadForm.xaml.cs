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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for BatchLoadForm.xaml
    /// </summary>
    public partial class BatchLoadForm : MetroWindow
    {
        public BatchLoadForm(string[] cuentas)
        {
            InitializeComponent();
            this.cuentas = cuentas;
            lstCuentas.ItemsSource = cuentas;
        }
        string[] cuentas;
        public bool Ok;

        private void btnClick_click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Ok = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
            this.Hide();
            Ok = false;
        }
    }
}
