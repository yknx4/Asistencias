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

namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for RegistroClub.xaml
    /// </summary>
    public partial class RegistroClub : Window
    {
        Window Sender;
        public RegistroClub(Window Sender)
        {
            this.Sender = Sender;
            InitializeComponent();
        }

        private void sliderChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string source = ((Slider)sender).Name;
            Label targetLabel=null;
            switch (source) { 
                case "sldParciales":
                    targetLabel = lblParciales;
                    break;
                case "sldAssist":
                    targetLabel = lblAsist;
                    break;
            }
            if(targetLabel!=null)targetLabel.Content = e.NewValue;
        }

        private void clickRegistro(object sender, RoutedEventArgs e)
        {
            if(txtNombre.Text=="")
            {
                txtNombre.Focus();
                return;
            }
            this.Hide();
            Sender.Show();
        }

       
    }
}
