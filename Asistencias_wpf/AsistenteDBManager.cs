using System;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Windows;

namespace Asistencias_wpf
{
    internal class AsistenteDBManager
    {
        public enum ValoresModificables
        {
            Nombre, NumeroCuenta, Plantel
        }

        public AsistenteDBManager(SqlCeConnection conexion)
        {
            this.conexion = conexion;
        }

        private SqlCeConnection conexion;
        private Asistente Holder;

        public void setAsistente(Asistente Input)
        {
            Holder = Input;
        }

        public void Clear()
        {
            Holder = null;
        }

        private bool Active()
        {
            if (Holder == null) return false;
            return true;
        }

        public bool anadirAsistencia(int club, int parcial)
        {
            if (!Active()) return false;
            conexion.Open();
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Asistencias(idClub, idAlumno, parcial, date)VALUES(@club, @cuenta, @parcial, @date)", conexion);
            cmd.Parameters.AddWithValue("@club", club);
            cmd.Parameters.AddWithValue("@cuenta", Holder.numeroCuenta);
            cmd.Parameters.AddWithValue("@parcial", parcial);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);

            //MessageBox.Show(DateTime.Now.Ticks);
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

            //Holder.asistencias++;
            Holder.Asistencias.Add(new Asistencia()
            {
                Date = DateTime.Now,
                Parcial = parcial,
            });
            return true;
        }

        public bool modificarDato(ValoresModificables modificar)
        {
            //UPDATE Alumnos SET Nombre = N'Jorge Figueroa Perez' WHERE (Alumnos.NumeroCuenta = 20094894)//
            if (!Active()) return false;
            int numeroCuentaQuery=0;
            string valorAModificarQuery="";
            string valorNuevoQuery = "";
            switch (modificar)
            {
                case ValoresModificables.Nombre:
                    numeroCuentaQuery = Holder.numeroCuenta;
                    valorAModificarQuery="Nombre";
                    valorNuevoQuery = Holder.nombre;
                    break;
                case ValoresModificables.NumeroCuenta:
                    numeroCuentaQuery = Holder.CuentaOriginal();
                    valorAModificarQuery="NumeroCuenta";
                    valorNuevoQuery = Holder.numeroCuenta.ToString();
                    break;
                case ValoresModificables.Plantel:
                    numeroCuentaQuery = Holder.numeroCuenta;
                    valorAModificarQuery="Plantel";
                    valorNuevoQuery = Holder.plantel;
                    break;
                default:
                    throw new NotImplementedException();
            }
            conexion.Open();

            SqlCeCommand cmd = new SqlCeCommand("UPDATE Alumnos SET "+valorAModificarQuery+" = N'"+valorNuevoQuery+"' WHERE (Alumnos.NumeroCuenta = "+numeroCuentaQuery.ToString()+")", conexion);
          
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show(modificar.ToString() + " modificado por " + valorNuevoQuery + ".");
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBoxResult mes = MessageBox.Show("Query: "+cmd.CommandText+" - "+ex.ToString());
                conexion.Close();
                return false;
            }
            catch (SqlCeException ex)
            {
                MessageBoxResult mes = MessageBox.Show("Query: " + cmd.CommandText + " - " + ex.ToString());
                conexion.Close();
                return false;
            }
            conexion.Close();
            return true;
        }
        

        public bool AddToDB()
        {
            if (!Active()) return false;
            conexion.Open();
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Alumnos(NumeroCuenta, Nombre, Plantel)VALUES(@cuenta, @nombre, @plantel)", conexion);
            cmd.Parameters.AddWithValue("@cuenta", Holder.numeroCuenta);
            cmd.Parameters.AddWithValue("@nombre", Holder.nombre);
            cmd.Parameters.AddWithValue("@plantel", Holder.plantel);
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
        }
        public void cuentaModificada(object sender, PropertyChangedEventArgs e)
        {
            Holder = (Asistente)sender;
            switch (e.PropertyName)
                {
                case "Nombre":
                        modificarDato(ValoresModificables.Nombre);
                    break;

                case "Cuenta":
                    modificarDato(ValoresModificables.NumeroCuenta);
                    break;

                case "Plantel":
                    modificarDato(ValoresModificables.Plantel);
                    break;
                }
            Clear();
        }
    }
}