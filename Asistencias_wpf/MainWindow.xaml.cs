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
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;

namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Asistente> acreditados;
        DataTable Alumnos = new DataTable();
        List<Asistente> asistentes;
        static SqlCeConnection conn = new SqlCeConnection(@"Data Source=|DataDirectory|\Alumnos.sdf");
        AsistentesPopulator populator;
        //private string parcial = ConfigurationManager.AppSettings["parcial"];
        int parcial;
       // private Asistente ResultHolder;
        Club clubSeleccionado;
        Window Sender;
        AsistenteDBManager asistenteManager= new AsistenteDBManager(conn);
        public MainWindow(Club seleccionado, int parcial, Window Sender)
        {
            this.Sender = Sender;
            this.parcial = parcial;
            this.clubSeleccionado = seleccionado;
            populator = new AsistentesPopulator(conn,parcial,seleccionado);
            InitializeComponent();
            asistentes = populator.Asistentes;
            lblEstado.Content = "Cargados " + asistentes.Count + " alumnos.";
            
            this.Title = seleccionado.Nombre;
  
        }

        private void acreditadosClk(object sender, RoutedEventArgs e)
        {
            acreditados = populator.Acreditados;
            DataContext = new MainWindowViewModel(acreditados);
            //gdAsistencias.AutoGenerateColumns = true;
            gdAsistencias.ItemsSource = acreditados;
            gdAsistencias.Visibility = Visibility.Visible;
            /*gdAsistencias.Width = 200;
            gdAsistencias.Height = 200;*/
        }

        private void alCerrar(object sender, EventArgs e)
        {
            Sender.Show();
        }

        private void btnAnadirAsis(object sender, RoutedEventArgs e)
        {
            anadirAsistencia();

        }

        private Asistente buscarAsistente(int NumeroCuenta)
        {
            //ANADIR CODIGO!!!!
            Asistente _result = asistentes.Find(delegate(Asistente bq)
            {
                return bq.numeroCuenta == NumeroCuenta;
            });
            return _result;
        }
        private Asistente buscarAsistente(string NumeroCuenta)
        {
            int noCuenta=0;
            //ANADIR CODIGO!!!!
            try
            {
                noCuenta = Convert.ToInt32(NumeroCuenta);
            }
            catch (System.FormatException)
            {
                return null;
            }
            Asistente _result = asistentes.Find(delegate(Asistente bq)
            {
                return bq.numeroCuenta == noCuenta;
            });
            return _result;
        }

        private void anadirAsistencia()
        {
            Asistente ResultHolder;

            if (txtCuenta.Text == "")
            {
                txtCuenta.Focus();
                return;
            }
           // int tmpCuenta;
            
            ResultHolder = buscarAsistente(txtCuenta.Text);
            if (ResultHolder != null)
            {
                asistenteManager.setAsistente(ResultHolder);
                if (asistenteManager.anadirAsistencia(clubSeleccionado.Id, parcial))
                {
                    lblEstado.Content = ResultHolder.nombre + " tiene " + ResultHolder.asistencias + " asistencias.";
                    txtCuenta.Text = "";
                }
                else
                {
                    lblEstado.Content = "Error al añadir asistencia.";
                }
                ResultHolder = null;
                asistenteManager.Clear();
            }
            else
            {
                lblEstado.Content = "Esa cuenta no existe.";
            }
        }
        

       
        private void onEnter(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            // your event handler here
            e.Handled = true;
            anadirAsistencia();
        }

        private void onEnterR(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            // your event handler here
            e.Handled = true;
            registrarAsistente();
        }

        private void onFocus(object sender, RoutedEventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            if (Sender.Text == "Numero de Cuenta")
            {
                Sender.Text = "";
            }
        }

        private void registrarAsistente()
        {
            if (txtCuentaR.Text == "" || txtCuentaR.Text.Length != 8)
            {
                txtCuentaR.Focus();
                return;
            }
            if (txtNombre.Text == "")
            {
                txtNombre.Focus();
                return;
            }
            if (txtPlantel.Text == "")
            {
                txtPlantel.Focus();
                return;
            }
            string nombre = txtNombre.Text;
            string numeroCuenta = txtCuentaR.Text;
            string plantel = txtPlantel.Text;

            Asistente actual = new Asistente()
            {
                nombre = txtNombre.Text,
                numeroCuenta = Convert.ToInt32(txtCuentaR.Text),
                plantel = txtPlantel.Text,
                asistencias = 0,


            };
            // actual.PropertyChanged += cuentaModificada;
            asistenteManager.setAsistente(actual);

            if (asistenteManager.AddToDB())
            {
                asistentes.Add(actual);
                lblEstado.Content = nombre + " ha sido registrado.";
                txtCuentaR.Text = "";
                txtNombre.Text = "";
                txtCuenta.Text = txtCuentaR.Text;
            }
            else
            {
                lblEstado.Content = "Error registrando " + nombre + ".";
            }
            asistenteManager.Clear();

        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            registrarAsistente();

        }

        private Boolean TextBoxTextAllowed(String Text2)
        {

            return Array.TrueForAll<Char>(Text2.ToCharArray(),

                delegate(Char c) { return Char.IsDigit(c) || Char.IsControl(c); });

        }

        private void textBoxValue_Pasting(object sender, DataObjectPastingEventArgs e)
        {

            if (e.DataObject.GetDataPresent(typeof(String)))
            {

                String Text1 = (String)e.DataObject.GetData(typeof(String));

                if (!TextBoxTextAllowed(Text1)) e.CancelCommand();

            }

            else e.CancelCommand();

        }

        private void textBoxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            e.Handled = !TextBoxTextAllowed(e.Text);

        }

        private void txtCuenta_TextChanged(object sender, TextChangedEventArgs e)
        {
            

        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            // System.Windows.Data.CollectionViewSource asistenteViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("asistenteViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // asistenteViewSource.Source = [generic data source]
        }
    }
}
