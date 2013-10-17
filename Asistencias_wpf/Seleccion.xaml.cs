using Asistencias_wpf.Properties;
using System.Data;
using System.Data.SqlServerCe;
using System.Windows;
using System.Windows.Controls;

namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for Seleccion.xaml
    /// </summary>
    public partial class Seleccion : Window
    {
        private SqlCeConnection conn = new SqlCeConnection(@"Data Source=|DataDirectory|\Alumnos.sdf");
        private DataTable Clubes = new DataTable();

        // List<Club> Clubs = new List<Club>();

        public Seleccion()
        {
            InitializeComponent();
            AlIniciar();
            DespuesDeCargar();
        }

        public void Refresh()
        {
            AlIniciar();
            DespuesDeCargar();
            llenarParcial(cmbClub, null);
        }

        private void DespuesDeCargar()
        {
            cmbClub.SelectedIndex = -1;
            cmbClub.Items.Clear();
            foreach (DataRow Row in Clubes.Rows)
            {
                int id = (int)Row["id"];
                string nombre = Row["nombre"].ToString();
                int asistencias = (int)Row["asisForAssist"];
                int parciales = (int)Row["parciales"];
                Club tmp = new Club()
                {
                    Id = id,
                    Nombre = nombre,
                    AsistenciasParaParcial = asistencias,
                    Parciales = parciales
                };
                cmbClub.Items.Add(tmp);
            }
            cmbClub.SelectedIndex = 0;

            // cmbParcial.SelectedIndex = Settings.Default.UltimaParcial;
        }

        private void AlIniciar()
        {
            SqlCeDataAdapter adap = new SqlCeDataAdapter("SELECT Clubes.* FROM Clubes", conn);
            Clubes.Clear();

            //the adapter will open and close the connection for you.
            adap.Fill(Clubes);
        }

        private void llenarParcial(object sender, SelectionChangedEventArgs e)
        {
            cmbParcial.SelectedIndex = 0;
            cmbParcial.Items.Clear();
            ComboBox comboClub = (ComboBox)sender;

            Club seleccionado = (Club)comboClub.SelectedItem;
            if (seleccionado == null) return;
            for (int i = 0; i < seleccionado.Parciales; i++)
            {
                cmbParcial.Items.Add(i + 1);
            }
            if (Settings.Default.UltimaParcial <= seleccionado.Parciales) cmbParcial.SelectedIndex = Settings.Default.UltimaParcial;
            else cmbParcial.SelectedIndex = 0;
        }

        private void guardarParcial(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.UltimaParcial = cmbParcial.SelectedIndex;
            Settings.Default.Save();
        }

        private void clickEntrar(object sender, RoutedEventArgs e)
        {
            if (cmbClub.Items.Count == 0)
            {
                ((Button)sender).IsEnabled = false;
                return;
            }
            if (cmbParcial.SelectedIndex <= -1)
            {
                cmbParcial.SelectedIndex = 0;
            }
            MainWindow main = new MainWindow((Club)cmbClub.SelectedItem, (cmbParcial.SelectedIndex + 1), this);
            main.Show();
            this.Hide();
        }

        public void enableEntrar()
        {
            btnEntrar.IsEnabled = true;
        }

        private void clickClubes(object sender, RoutedEventArgs e)
        {
            RegistroClub ventanaRegistro = new RegistroClub(this);
            ventanaRegistro.Show();
            this.Hide();
        }

        private void hideOrShows(object sender, DependencyPropertyChangedEventArgs e)
        {
            Refresh();
        }

        
    }
}