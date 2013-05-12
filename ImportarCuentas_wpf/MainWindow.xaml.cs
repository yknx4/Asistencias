using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Data;
namespace ImportarCuentas_wpf
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
//        SqlCeConnection conn = new SqlCeConnection(@"Data Source=D:\Source Codes\Asistencias\Asistencias_wpf\bin\Release\Alumnos.sdf");
        //Data Source="D:\Source Codes\Asistencias\Asistencias_wpf\Alumnos.sdf"
        SqlCeConnection conn = new SqlCeConnection(@"Data Source=D:\Source Codes\Asistencias\Asistencias_wpf\Alumnos.sdf");
        List<int> cuentas = new List<int>();
        DataTable Cuentas = new DataTable();
        public MainWindow()
            {
            //this.Title = "lol";
            //dialog.ShowDialog();
            InitializeComponent();
            dialog.Multiselect = true;
            dialog.DefaultExt = "config";
            }
        private void seleccionarArchivos(object sender, RoutedEventArgs e)
            {
            dialog.ShowDialog();
            if (dialog.SafeFileNames.Length > 0)
                {
                conn.Open();
                string[] filenames = dialog.FileNames;
                foreach (string account in filenames)
                    {
                    System.Configuration.Configuration config;
                    ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                    configMap.ExeConfigFilename = account;
                    config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                    KeyValueConfigurationCollection datosCuenta = config.AppSettings.Settings;
                    //datosCuenta[parcial].Value;
                    int accountValue = Convert.ToInt32(account.Substring(account.Length - 15, 8));
                    string nmb = datosCuenta["nombre"].Value;
                    string ptl = datosCuenta["plantel"].Value;
                    int ast = Convert.ToInt32(datosCuenta["_3Parcial"].Value);
                    //this.Title = accountValue.ToString();

                   //bool cuentaExiste = cuentas.Contains(accountValue);

                  //  SqlCeCom  mand cmdInsert = conn.CreateCommand();
                    SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Alumnos(NumeroCuenta, Nombre, Plantel)VALUES(@cuenta, @nombre, @plantel)", conn);
                    cmd.Parameters.AddWithValue("@cuenta", accountValue);
                    cmd.Parameters.AddWithValue("@nombre", nmb);
                    cmd.Parameters.AddWithValue("@plantel", ptl);
//                    cmd.CommandText = "INSERT TO Alumnos (NumeroCuenta, Nombre, Plantel) VALUES ('" + accountValue.ToString() + "', '" + nmb + "', '" + ptl + "')";
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (System.InvalidOperationException)
                    {

                    }
                    catch (SqlCeException) { }
                    
                    for (int i = 0; i < ast; i++)
                        {
  //                      cmd.CommandText = "INSERT TO Asistencias (idClub, idAlumno, parcial) VALUES ('1', '" + accountValue.ToString() + "', '3')";
                        SqlCeCommand cmd2 = new SqlCeCommand("INSERT INTO Asistencias(idClub, idAlumno, parcial)VALUES(@club, @cuenta, @parcial)", conn);
                        cmd2.Parameters.AddWithValue("@club", 2);
                        cmd2.Parameters.AddWithValue("@cuenta", accountValue);
                        cmd2.Parameters.AddWithValue("@parcial", 3);
                        cmd2.ExecuteNonQuery();
                        }
                        
                    }
                conn.Close();

                }
            }
        }
    }
