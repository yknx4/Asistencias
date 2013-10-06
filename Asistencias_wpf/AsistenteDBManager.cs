using System;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Windows;

namespace Asistencias_wpf
{
    internal class AsistenteDBManager : DBManager<Asistente>
    {
       

        public AsistenteDBManager(SqlCeConnection conexion):base(conexion)
        {
            
        }

        //private SqlCeConnection connection;
       // private Asistente heldItem;

       

        public bool anadirAsistencia(int club, int parcial)
        {
            if (!Active()) return false;
            connection.Open();
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Asistencias(idClub, idAlumno, parcial, date)VALUES(@club, @cuenta, @parcial, @date)", connection);
            cmd.Parameters.AddWithValue("@club", club);
            cmd.Parameters.AddWithValue("@cuenta", heldItem.numeroCuenta);
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
                connection.Close();
                return false;
            }
            catch (SqlCeException ex)
            {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                connection.Close();
                return false;
            }
            connection.Close();

            //Holder.asistencias++;
            heldItem.Asistencias.Add(new Asistencia()
            {
                Date = DateTime.Now,
                Parcial = parcial,
            });
            return true;
        }

        public override bool modificarDato(UInt32 modificar)
        {
            //UPDATE Alumnos SET Nombre = N'Jorge Figueroa Perez' WHERE (Alumnos.NumeroCuenta = 20094894)//
            if (!Active()) return false;
            int numeroCuentaQuery = 0;
            string valorAModificarQuery = "";
            string valorNuevoQuery = "";
            switch (modificar)
            {
                case ModifiableValues.Name:
                    numeroCuentaQuery = heldItem.numeroCuenta;
                    valorAModificarQuery = "Nombre";
                    valorNuevoQuery = heldItem.Nombre;
                    break;

                case ModifiableValues.ID:
                    numeroCuentaQuery = heldItem.CuentaOriginal();
                    valorAModificarQuery = "NumeroCuenta";
                    valorNuevoQuery = heldItem.numeroCuenta.ToString();
                    break;

                case ModifiableValues.Campus:
                    numeroCuentaQuery = heldItem.numeroCuenta;
                    valorAModificarQuery = "Plantel";
                    valorNuevoQuery = heldItem.plantel;
                    break;

                default:
                    throw new NotImplementedException();
            }
            connection.Open();

            SqlCeCommand cmd = new SqlCeCommand("UPDATE Alumnos SET " + valorAModificarQuery + " = N'" + valorNuevoQuery + "' WHERE (Alumnos.NumeroCuenta = " + numeroCuentaQuery.ToString() + ")", connection);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show(ModifiableValues.Value[modificar] + " modificado por " + valorNuevoQuery + ".");
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBoxResult mes = MessageBox.Show("Query: " + cmd.CommandText + " - " + ex.ToString());
                connection.Close();
                return false;
            }
            catch (SqlCeException ex)
            {
                MessageBoxResult mes = MessageBox.Show("Query: " + cmd.CommandText + " - " + ex.ToString());
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }

        public override bool AddToDB()
        {
            if (!Active()) return false;
            connection.Open();
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Alumnos(NumeroCuenta, Nombre, Plantel)VALUES(@cuenta, @nombre, @plantel)", connection);
            cmd.Parameters.AddWithValue("@cuenta", heldItem.numeroCuenta);
            cmd.Parameters.AddWithValue("@nombre", heldItem.Nombre);
            cmd.Parameters.AddWithValue("@plantel", heldItem.plantel);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                connection.Close();
                return false;
            }
            catch (SqlCeException ex)
            {
                MessageBoxResult mes = MessageBox.Show(ex.ToString());
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }

        public override void itemModified(object sender, PropertyChangedEventArgs e)
        {
            base.itemModified(sender, e);
            switch (e.PropertyName)
            {
                case "Nombre":
                    modificarDato(ModifiableValues.Name);
                    break;

                case "Cuenta":
                    modificarDato(ModifiableValues.ID);
                    break;

                case "Plantel":
                    modificarDato(ModifiableValues.Campus);
                    break;
                default:
                    throw new NotImplementedException(e.PropertyName+" hasn't been implemented.");
            }
            Clear();
        }
    }
}