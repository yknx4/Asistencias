using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
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
    /// Interaction logic for ModificarClub.xaml
    /// </summary>
    public partial class ModificarClub : Window
    {
        Seleccion Sender;
        private SqlCeConnection conn = new SqlCeConnection(@"Data Source=|DataDirectory|\Alumnos.sdf");
        ClubDBManager clubManager;
        Club selected;
        public ModificarClub(Seleccion Sender, Club selected)
        {
            this.Sender = Sender;
            clubManager = new ClubDBManager(conn);
            selected.PropertyChanged += clubManager.clubModificado;
            this.selected = selected;
            clubManager.setClub(this.selected);
            InitializeComponent();
            sldAssist.Value = this.selected.AsistenciasParaParcial;
            sldParciales.Value = this.selected.Parciales;
            txtNombre.Text = this.selected.Nombre;
            this.Title = "Modificar club: " + this.selected.Nombre;
           // MessageBox.Show(selected.Equals(this.selected).ToString());
            
        }

        private void alCerrar(object sender, EventArgs e)
        {
            Sender.Show();
        }
        private void sliderChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string source = ((Slider)sender).Name;
            Label targetLabel = null;
            switch (source)
            {
                case "sldParciales":
                    targetLabel = lblParciales;
                    break;
                case "sldAssist":
                    targetLabel = lblAsist;
                    break;
            }
            if (targetLabel != null) targetLabel.Content = e.NewValue;
        }

        private void clickEdit(object sender, RoutedEventArgs e)
        {
            if(txtNombre.Text=="")
            {
                txtNombre.Focus();
                return;
            }
            if (!selected.Nombre.Equals(txtNombre.Text)) selected.Nombre = txtNombre.Text;
            if(selected.Parciales!=(int)sldParciales.Value)selected.Parciales = (int)sldParciales.Value;
            if (selected.AsistenciasParaParcial != (int)sldAssist.Value) selected.AsistenciasParaParcial = (int)sldAssist.Value;
            Sender.Show();
            Sender.Refresh();
            this.Close();
            
        }

        private void clickEliminar(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Estas seguro que deseas eliminar el club +"+selected.Nombre+"?", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //MessageBox.Show("SI");
                if (clubManager.RemoveFromDB())
                {
                    Sender.cmbClub.SelectedIndex = 0;
                    Sender.cmbClub.Items.Remove(selected);
                    Sender.Show();
                    Sender.Refresh();
                    this.Close();
                }
                else
                {
                    throw new Exception("WTF!!");
                }
            }
        }

    }
}
