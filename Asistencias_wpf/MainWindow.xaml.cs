using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Text.RegularExpressions;


namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
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
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
            
            asistentes = populator.Asistentes;
            lblEstado.Content = "Cargados " + asistentes.Count + " alumnos.";

            this.Title = seleccionado.Nombre;
            tbLog.Focus();
            
        }

        private void acreditadosClk(object sender, RoutedEventArgs e)
        {
            if (this.Width<1000) this.Width= 1000;
            if (this.Height < 550) this.Height = 550;
            
            populator.generarLista();
            populator.generarAcreditados();
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
        private void anadirAsistencia(int numeroCuenta)
        {
            Asistente ResultHolder;
            
            // int tmpCuenta;

            ResultHolder = buscarAsistente(numeroCuenta);
            if (ResultHolder != null)
            {
                asistenteManager.setItem(ResultHolder);
                if (asistenteManager.anadirAsistencia(clubSeleccionado.Id, parcial))
                {
                    lblEstado.Content = ResultHolder.Nombre + " tiene " + ResultHolder.asistencias + " asistencias.";
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
        private void anadirAsistencia()
        {
            int noCuenta;
            if (txtCuenta.Text == "" || !int.TryParse(txtCuenta.Text,out noCuenta))
            {
                txtCuenta.Focus();
                return;
            }
            anadirAsistencia(noCuenta);

        }

        private void anadirAsistencia(string cuenta)
        {
            int noCuenta;
            if (!int.TryParse(cuenta, out noCuenta))
            {
                MessageBox.Show(cuenta + " no es un número válido.");    
                return;
            }
            anadirAsistencia(noCuenta);
            
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
            
            return buscarAsistente(noCuenta);
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
                Nombre = txtNombre.Text,
                numeroCuenta = Convert.ToInt32(txtCuentaR.Text),
                plantel = txtPlantel.Text,
            };
            actual.PropertyChanged += populator.cuentaModificada;
            asistenteManager.setItem(actual);

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
                lblEstado.Content = busquedaAsist.Nombre + " tiene " + busquedaAsist.asistencias + " asistencias.";
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
            //MessageBox.Show(btnSett.ActualWidth.ToString() + " w  y h " + btnSett.ActualHeight.ToString());
        }

        private void clickEdit(object sender, RoutedEventArgs e)
        {
            ModificarClub ventanaModificar = new ModificarClub(this,clubSeleccionado);
            ventanaModificar.ShowDialog();
            
        }

        private void btnLoad_click(object sender, RoutedEventArgs e)
        {
            List<String> cuentas = new List<String>();
            bool cargar;
            char[] separadores = {'\t','\n',' ','\r' };
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            bool? userClickedOK = fileDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = fileDialog.OpenFile();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    cuentas.AddRange(reader.ReadToEnd().Split(separadores));
                }
                for (int i = 0; i < cuentas.Count; i++)
                {
                    //return new string(input.Where(c => char.IsDigit(c)).ToArray());
                    cuentas[i] = Regex.Replace(cuentas[i], "[^0-9]", "");
                    if (cuentas[i].Trim().Trim(separadores).Length == 0)
                    {
                        cuentas.Remove(cuentas[i]);
                    }
                }
                fileStream.Close();
                BatchLoadForm load = new BatchLoadForm(cuentas.ToArray());
                load.ShowDialog();
                cargar = load.Ok;
                load.Close();
                if (cargar)
                {
                    foreach (string cuenta in cuentas)
                    {
                        anadirAsistencia(cuenta);
                    }
                }
            }

            
            
        }

        private void status_labelUptade(object sender, TextCompositionEventArgs e)
        {
            lstLog.Items.Add(e.Text);
        }
    }
}