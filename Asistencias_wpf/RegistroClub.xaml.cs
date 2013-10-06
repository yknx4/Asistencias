using System;
using System.Data.SqlServerCe;
using System.Windows;
using System.Windows.Controls;

namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for RegistroClub.xaml
    /// </summary>
    public partial class RegistroClub : Window
    {
        private Seleccion Sender;
        private SqlCeConnection conn = new SqlCeConnection(@"Data Source=|DataDirectory|\Alumnos.sdf");
        private ClubDBManager clubManager;

        public RegistroClub(Seleccion Sender)
        {
            this.Sender = Sender;
            clubManager = new ClubDBManager(conn);
            InitializeComponent();
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

        private Club registrarClub()
        {
            Club tmpClub = new Club()
            {
                AsistenciasParaParcial = (int)sldAssist.Value,
                Parciales = (int)sldParciales.Value,
                Nombre = txtNombre.Text
            };
            clubManager.setItem(tmpClub);
            if (clubManager.AddToDB())
            {
                MessageBox.Show(tmpClub.Nombre + " ha sido registrado.");
            }
            else
            {
                throw new Exception();
            }
            clubManager.Clear();
            return tmpClub;
        }

        private void clickRegistro(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text == "")
            {
                txtNombre.Focus();
                return;
            }
            registrarClub();
            this.Close();
            Sender.Refresh();
            Sender.enableEntrar();
            Sender.Show();
        }
    }
}