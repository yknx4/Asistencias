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

        private void DespuesDeCargar()
        {
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

            //the adapter will open and close the connection for you.
            adap.Fill(Clubes);
        }

        private void llenarParcial(object sender, SelectionChangedEventArgs e)
        {
            cmbParcial.SelectedIndex = 0;
            cmbParcial.Items.Clear();
            ComboBox comboClub = (ComboBox)sender;

            Club seleccionado = (Club)comboClub.SelectedItem;
            for (int i = 0; i < seleccionado.Parciales; i++)
            {
                cmbParcial.Items.Add(i + 1);
            }
            cmbParcial.SelectedIndex = Settings.Default.UltimaParcial;
        }

        private void guardarParcial(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.UltimaParcial = cmbParcial.SelectedIndex;
            Settings.Default.Save();
        }

        private void clickEntrar(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow((Club)cmbClub.SelectedItem, (cmbParcial.SelectedIndex + 1), this);
            main.Show();
            this.Hide();
        }
    }
}