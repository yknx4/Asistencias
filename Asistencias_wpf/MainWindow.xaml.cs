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

namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Asistente ResultHolder;
        private string parcial = ConfigurationManager.AppSettings["parcial"];
        List<Asistente> asistentes;
        List<Asistente> acreditados;
        int parcialN;
        Club seleccionado;
        public MainWindow(Club seleccionado,int parcial)
        {
        this.parcialN = parcial;
        this.seleccionado = seleccionado;
            InitializeComponent();
            generarLista();
            lblEstado.Content = "Cargados " + asistentes.Count + " alumnos.";
            // gdAsistencias.ItemsSource = acreditados;
            this.Title = ConfigurationManager.AppSettings["club"];
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            //gdAsistencias.AutoGenerateColumns=true;
        }

        private void generarLista()
        {
            asistentes = new List<Asistente>();
            string directorioCuentas = Environment.CurrentDirectory + @"\Accounts";
            string[] files = Directory.GetFiles(directorioCuentas, "*.config", SearchOption.TopDirectoryOnly);
            foreach (string account in files)
            {
                // lblEstado.Content = account.Substring(account.Length-15,8); 
                System.Configuration.Configuration config;
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                configMap.ExeConfigFilename = account;
                config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                KeyValueConfigurationCollection datosCuenta = config.AppSettings.Settings;
                //datosCuenta[parcial].Value;
                int accountValue = Convert.ToInt32(account.Substring(account.Length - 15, 8));
                string nmb = datosCuenta["nombre"].Value;
                string ptl = datosCuenta["plantel"].Value;
                int ast = Convert.ToInt32(datosCuenta[parcial].Value);
                Asistente actual = new Asistente
                {
                    nombre = nmb,
                    numeroCuenta = accountValue,
                    plantel = ptl,
                    asistencias = ast,

                };
                actual.PropertyChanged += cuentaModificada;
                asistentes.Add(actual);
            }
            
            //generarAcreditados();
        }

        private void generarAcreditados()
        {
            acreditados = new List<Asistente>();
            foreach (Asistente Alumno in asistentes)
            {
                if (Alumno.asistencias >= 2) acreditados.Add(Alumno);
            }
        }
        private void btnAnadirAsis(object sender, RoutedEventArgs e)
        {
            if (txtCuenta.Text == "")
            {
                txtCuenta.Focus();
            }
            int tmpCuenta;
            try
            {
                tmpCuenta = Convert.ToInt32(txtCuenta.Text);
            }
            catch (System.FormatException)
            {
                tmpCuenta = 123456789;
            }
            ResultHolder = asistentes.Find(delegate(Asistente bq)
            {
                return bq.numeroCuenta == tmpCuenta;
            });
            if (ResultHolder != null)
            {
                ResultHolder.asistencias++;
                ResultHolder = null;
            }
            else
            {
                lblEstado.Content = "Esa cuenta no existe.";
            }

        }

        private void txtCuenta_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void onFocus(object sender, RoutedEventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            if (Sender.Text == "Numero de Cuenta")
            {
                Sender.Text = "";
            }
        }

        private void onEnter(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            // your event handler here
            e.Handled = true;
            btnAnadirAsis(btnAsist, new RoutedEventArgs());
        }

        private void textBoxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            e.Handled = !TextBoxTextAllowed(e.Text);

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



        private Boolean TextBoxTextAllowed(String Text2)
        {

            return Array.TrueForAll<Char>(Text2.ToCharArray(),

                delegate(Char c) { return Char.IsDigit(c) || Char.IsControl(c); });

        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
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
            string res = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<configuration>\n<appSettings>\n<add key=\"nombre\" value=\"" + nombre + "\"/>\n<add key=\"plantel\" value=\"" + plantel + "\"/>\n<add key=\"_1Parcial\" value=\"0\"/>\n<add key=\"_2Parcial\" value=\"0\"/>\n<add key=\"_3Parcial\" value=\"0\"/>\n</appSettings>\n</configuration>";
            try
            {
                System.IO.File.WriteAllText(@"Accounts\" + numeroCuenta + ".config", res);
            }
            catch (System.Exception)
            {
                lblEstado.Content = "Error al guardar el registo de " + nombre;
            }
            Asistente actual = new Asistente
            {
                nombre = txtNombre.Text,
                numeroCuenta = Convert.ToInt32(txtCuentaR.Text),
                plantel = txtPlantel.Text,
                asistencias = 0,

            };
            actual.PropertyChanged += cuentaModificada;
            asistentes.Add(actual);
            lblEstado.Content = nombre + " ha sido registrado.";
            txtCuentaR.Text = "";
            txtNombre.Text = "";
            txtCuenta.Text = txtCuentaR.Text;

        }

        private void onEnterR(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            // your event handler here
            e.Handled = true;
            Registrar_Click(btnRegistrar, new RoutedEventArgs());
        }

        private void acreditadosClk(object sender, RoutedEventArgs e)
        {
            generarAcreditados();
            DataContext = new MainWindowViewModel(acreditados);
            //gdAsistencias.AutoGenerateColumns = true;
            gdAsistencias.ItemsSource = acreditados;
            gdAsistencias.Visibility = Visibility.Visible;
            /*gdAsistencias.Width = 200;
            gdAsistencias.Height = 200;*/
        }

        /*  private void GetSelectedCells(object sender, RoutedEventArgs e)
          {
              string selectedData = "";
              foreach (var dataGridCellInfo in selectedCellsGrid.SelectedCells)
              {
                  PropertyInfo pi = dataGridCellInfo.Item.GetType().GetProperty(dataGridCellInfo.Column.Header.ToString());
                  var value = pi.GetValue(dataGridCellInfo.Item, null);
                  selectedData += dataGridCellInfo.Column.Header + ": " + value.ToString() + "\n";
              }
              MessageBox.Show(selectedData);
          }*/

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            // System.Windows.Data.CollectionViewSource asistenteViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("asistenteViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // asistenteViewSource.Source = [generic data source]
        }
        /* static void Main(string[] args)
    {
        Counter c = new Counter();
        c.ThresholdReached += c_ThresholdReached;

        // provide remaining implementation for the class
    }

    static void c_ThresholdReached(object sender, EventArgs e)
    {
        Console.WriteLine("The threshold was reached.");
    }*/
        private void cuentaModificada(object sender, PropertyChangedEventArgs e)
        {
            Asistente source = (Asistente)sender;
            System.Configuration.Configuration config;
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = @"Accounts/" + source.numeroCuenta + ".config";
            config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection datosCuenta = config.AppSettings.Settings;
            switch (e.PropertyName)
            {
                case "Nombre":
                    datosCuenta["nombre"].Value = source.nombre;
                    config.Save();
                    lblEstado.Content = "Nombre de " + source.nombre + " Modificado.";
                    break;
                case "Cuenta":
                    if (source.numeroCuenta.ToString().Length != 8 || File.Exists(@"Accounts/" + source.numeroCuenta + ".config"))
                    {
                        lblEstado.Content = "Cuenta Incorrecta, " + source.nombre + " no se modifico.";
                    } 
                    else
                    {
                        lblEstado.Content = "Cuenta de " + source.nombre + " Modificada.";
                        File.Copy(@"Accounts/" + source._cuentaEstatica + ".config", @"Accounts/" + source.numeroCuenta + ".config");
                        File.Delete(@"Accounts/" + source._cuentaEstatica + ".config");
                        source._cuentaEstatica = source.numeroCuenta;
                        generarLista();
                    }
                    break;
                case "Plantel":
                    datosCuenta["plantel"].Value = source.plantel;
                    config.Save();
                    lblEstado.Content = "Plantel de " + source.nombre + " Modificado.";
                    break;
                case "Asistencias":
                    datosCuenta[parcial].Value = source.asistencias.ToString();
                    config.Save();
                    lblEstado.Content = datosCuenta["nombre"].Value + " tiene " + datosCuenta[parcial].Value + " asistencias.";
                    txtCuenta.Text = "";
                    break;
            }
        }
    }
}
