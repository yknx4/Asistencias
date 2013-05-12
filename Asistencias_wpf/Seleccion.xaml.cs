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
using System.Data;
using System.Data.SqlServerCe;
using System.Configuration;
using Asistencias_wpf.Properties;
namespace Asistencias_wpf
    {
    /// <summary>
    /// Interaction logic for Seleccion.xaml
    /// </summary>
    public partial class Seleccion : Window
        {
        SqlCeConnection conn = new SqlCeConnection(@"Data Source=|DataDirectory|\Alumnos.sdf");
        DataTable Clubes = new DataTable();
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
                    Club tmp = new Club(){
                        Id=id,Nombre=nombre,AsistenciasParaParcial=asistencias,Parciales=parciales
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
            for (int i = 0; i < seleccionado.Parciales;i++ )
                {
                cmbParcial.Items.Add(i+1);
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

            }

        
       
        }
    }
