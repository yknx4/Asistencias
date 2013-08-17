using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool cellHasBeenSelected = false;
        private static SqlCeConnection conn = new SqlCeConnection(@"Data Source=|DataDirectory|\Alumnos.sdf");
        private List<Asistente> acreditados;
        private DataTable Alumnos = new DataTable();
        private AsistenteDBManager asistenteManager = new AsistenteDBManager(conn);
        private List<Asistente> asistentes;
        // private Asistente ResultHolder;
        private Club clubSeleccionado;

        //private string parcial = ConfigurationManager.AppSettings["parcial"];
        private int parcial;

        private AsistentesPopulator populator;
        private Window Sender;
        public MainWindow(Club seleccionado, int parcial, Window Sender)
        {
            this.Sender = Sender;
            this.parcial = parcial;
            this.clubSeleccionado = seleccionado;
            populator = new AsistentesPopulator(conn, parcial, seleccionado);
            InitializeComponent();
            asistentes = populator.Asistentes;
            lblEstado.Content = "Cargados " + asistentes.Count + " alumnos.";

            this.Title = seleccionado.Nombre;
        }

        private void acreditadosClk(object sender, RoutedEventArgs e)
        {
            acreditados = populator.Acreditados;
            if (chkTodos.IsChecked != null && (bool)chkTodos.IsChecked == true)
            {
                acreditados = populator.Asistentes;
            }
            DataContext = new MainWindowViewModel(acreditados);

            //gdAsistencias.AutoGenerateColumns = true;
            gdAsistencias.ItemsSource = acreditados;
            gdAsistencias.Visibility = Visibility.Visible;
            cellHasBeenSelected = false;
            /*gdAsistencias.Width = 200;
            gdAsistencias.Height = 200;*/
        }

        private void alCerrar(object sender, EventArgs e)
        {
            Sender.Show();
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

        private void btnAnadirAsis(object sender, RoutedEventArgs e)
        {
            anadirAsistencia();
        }

        private void btnLookupClick(object sender, RoutedEventArgs e)
        {
            string noCuenta = txtCuenta.Text;
            PopupAsistenciasPersonal(noCuenta);
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
            int noCuenta = 0;

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
        private void generatingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Asistencias":
                    e.Cancel = true;
                    break;

                case "nombre":
                    e.Column.Header = "Nombre";
                    break;

                case "numeroCuenta":
                    e.Column.Header = @"Número de Cuenta";
                    break;

                case "plantel":
                    e.Column.Header = "Plantel";
                    break;

                case "asistencias":
                    e.Column.Header = "Asistencias";
                    e.Column.IsReadOnly = true;
                    break;

                default:
                    break;
            }
        }

        private void lostFocusInRow(object sender, RoutedEventArgs e)
        {
            PopupLookup.IsOpen = false;
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

        private void PopupAsistenciasPersonal(string noCuenta)
        {
            PopupLookup.IsOpen = false;
            Asistente busquedaAsist = buscarAsistente(noCuenta);

            //if (lblEstado != null) lblEstado.Content = "";
            if (busquedaAsist != null)
            {
                //lblEstado.Content = busquedaAsist.nombre;
                gdAsistenciasPorAlumno.ItemsSource = busquedaAsist.Asistencias;

                PopupLookup.IsOpen = true;
            }
            else
            {
                if (gdAsistenciasPorAlumno != null)
                {
                    gdAsistenciasPorAlumno.ItemsSource = null;
                }
            }
        }

        private void PopupLostFocus(object sender, MouseEventArgs e)
        {
            PopupLookup.IsOpen = false;
        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            registrarAsistente();
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
                
            };
            actual.PropertyChanged += populator.cuentaModificada;
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
        private void selectedCellsChange(object sender, SelectedCellsChangedEventArgs e)
        {
            PopupLookup.IsOpen = false;

            //MessageBox.Show(e.AddedCells.Count.ToString() + " - " + e.RemovedCells.Count.ToString());
            Asistente selectedAsistente;
            string noCuenta;
            if ((e.AddedCells.Count == 4 && e.RemovedCells.Count != 0) || !cellHasBeenSelected)
            {
                selectedAsistente = (Asistente)e.AddedCells[0].Item;
                noCuenta = selectedAsistente.numeroCuenta.ToString();
                cellHasBeenSelected = true;
            }
            else return;
            if (selectedAsistente.asistencias > 0) PopupAsistenciasPersonal(noCuenta);
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
            string noCuenta = ((TextBox)sender).Text;
            Asistente busquedaAsist = buscarAsistente(noCuenta);

            //if (lblEstado != null) lblEstado.Content = "";
            if (busquedaAsist != null)
            {
                lblEstado.Content = busquedaAsist.nombre + " tiene " + busquedaAsist.asistencias + " asistencias.";
            }
            else
            {
                if (PopupLookup != null)
                {
                    PopupLookup.IsOpen = false;
                }
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // System.Windows.Data.CollectionViewSource asistenteViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("asistenteViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // asistenteViewSource.Source = [generic data source]
        }
    }
}