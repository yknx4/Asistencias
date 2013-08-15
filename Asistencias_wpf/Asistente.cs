using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Windows;

namespace Asistencias_wpf
    {
    public class Asistente : INotifyPropertyChanged
        {
       /* public Asistente(SqlCeConnection conn)
            {
            conexion = conn;
            }*/
       // public static SqlCeConnection conexion;
        private int _cuentaEstatica;
        private string _nombre;
        private int _numeroCuenta;
        private string _plantel;
        private int _asistencias;
        public string nombre
            {
            get { return _nombre; }
            set
                {
                _nombre = value;
                NotifyPropertyChanged("Nombre");
                modificaciones++;
                }
            }
        public int numeroCuenta
            {
            get { return _numeroCuenta; }
            set
                {
                _numeroCuenta = value;
                if (!(modificaciones > 1)) _cuentaEstatica = value;
                NotifyPropertyChanged("Cuenta");
                modificaciones++;
                }
            }
        public string plantel
            {
            get { return _plantel; }
            set
                {
                _plantel = value;
                NotifyPropertyChanged("Plantel");
                modificaciones++;
                }
            }
        public int asistencias
            {
            get { return _asistencias; }
            set
                {
                _asistencias = value;
                NotifyPropertyChanged("Asistencias");
                modificaciones++;
                }
            }

        public int modificaciones = 0;
        private void resetCuenta()
        {
            _cuentaEstatica = _numeroCuenta;
        }
       /* public bool anadirAsistencia(int club, int parcial)
            {

            conexion.Open();
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Asistencias(idClub, idAlumno, parcial)VALUES(@club, @cuenta, @parcial)", conexion);
            cmd.Parameters.AddWithValue("@club", club);
            cmd.Parameters.AddWithValue("@cuenta", numeroCuenta);
            cmd.Parameters.AddWithValue("@parcial", parcial);

            try
                {
                cmd.ExecuteNonQuery();
                }
            catch (System.InvalidOperationException ex)
                {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                conexion.Close();
                return false;
                }
            catch (SqlCeException ex)
                {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                conexion.Close();
                return false;
                }
            conexion.Close();
            asistencias++;
            return true;
            }*/

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
            {
            if (PropertyChanged != null)
                {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }


        #endregion

        /*public bool AddToDB()
            {
            conexion.Open();
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Alumnos(NumeroCuenta, Nombre, Plantel)VALUES(@cuenta, @nombre, @plantel)", conexion);
            cmd.Parameters.AddWithValue("@cuenta", numeroCuenta);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@plantel", plantel);
            try
                {
                cmd.ExecuteNonQuery();
                }
            catch (System.InvalidOperationException ex)
                {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                conexion.Close();
                return false;
                }
            catch (SqlCeException ex)
                {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                conexion.Close();
                return false;
                }
            conexion.Close();
            return true;

            }*/
      /*  public bool saveData(string Modifier)
            {
            SqlCeCommand cmd;
            
            //UPDATE Alumnos SET Nombre = N'Jorge Figueroa Perez' WHERE (Alumnos.NumeroCuenta = 20094894)//
            switch (Modifier)
                {
                case "Nombre":
                    cmd = new SqlCeCommand("UPDATE Alumnos SET Nombre = N'@nombre' WHERE (Alumnos.NumeroCuenta = @cuenta)", conexion);
                    cmd.Parameters.AddWithValue("@cuenta", numeroCuenta);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    break;
                case "Cuenta":
                    
                    cmd = new SqlCeCommand("UPDATE Alumnos SET NumeroCuenta = @cuenta WHERE (Alumnos.Nombre = N'@nombre')", conexion);
                    cmd.Parameters.AddWithValue("@cuenta", numeroCuenta);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    break;
                case "Plantel":
                    
                     cmd = new SqlCeCommand("UPDATE Alumnos SET Plantel = N'@plantel' WHERE (Alumnos.NumeroCuenta = @cuenta)", conexion);
                    cmd.Parameters.AddWithValue("@cuenta", numeroCuenta);
                    cmd.Parameters.AddWithValue("@plantel", plantel);
                    break;
                default:
                    MessageBoxResult mes = MessageBox.Show("Modificador Incorrecto");
                    return false;
                }
            conexion.Open();
            try
                {
                cmd.ExecuteNonQuery();
                }
            catch (System.InvalidOperationException ex)
                {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                conexion.Close();
                return false;
                }
            catch (SqlCeException ex)
                {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                conexion.Close();
                return false;
                }
            conexion.Close();
            return true;

            }*/
        }

    }
