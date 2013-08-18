using System;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Windows;

namespace Asistencias_wpf
{
    internal class ClubDBManager
    {
        public enum ValoresModificables
        {
            Nombre, AsistenciasParaAcreditar, Parciales
        }

        public ClubDBManager(SqlCeConnection conexion)
        {
            this.conexion = conexion;
        }

        private SqlCeConnection conexion;
        private Club Holder;

        public void setClub(Club Input)
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

        public bool modificarDato(ValoresModificables modificar)
        {
            //UPDATE Alumnos SET Nombre = N'Jorge Figueroa Perez' WHERE (Alumnos.NumeroCuenta = 20094894)//
            if (!Active()) return false;
            int clubIDQuery = Holder.Id;
            string valorAModificarQuery = "";
            string valorNuevoQuery = "";
            switch (modificar)
            {
                case ValoresModificables.Nombre:

                    valorAModificarQuery = "nombre";
                    valorNuevoQuery = Holder.Nombre;
                    break;

                case ValoresModificables.AsistenciasParaAcreditar:

                    valorAModificarQuery = "asisForAssist";
                    valorNuevoQuery = Holder.AsistenciasParaParcial.ToString();
                    break;

                case ValoresModificables.Parciales:

                    valorAModificarQuery = "parciales";
                    valorNuevoQuery = Holder.Parciales.ToString();
                    break;

                default:
                    throw new NotImplementedException();
            }
            conexion.Open();

            SqlCeCommand cmd = new SqlCeCommand("UPDATE Clubes SET " + valorAModificarQuery + " = N'" + valorNuevoQuery + "' WHERE (Clubes.id = " + clubIDQuery.ToString() + ")", conexion);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show(modificar.ToString() + " modificado por " + valorNuevoQuery + ".");
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBoxResult mes = MessageBox.Show("Query: " + cmd.CommandText + " - " + ex.ToString());
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
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Clubes(nombre, asisForAssist, parciales)VALUES(@nombre, @asisForAssist, @parciales)", conexion);
            cmd.Parameters.AddWithValue("@nombre", Holder.Nombre);
            cmd.Parameters.AddWithValue("@asisForAssist", Holder.AsistenciasParaParcial);
            cmd.Parameters.AddWithValue("@parciales", Holder.Parciales);
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

        public bool RemoveFromDB()
        {
            if (!Active()) return false;
            conexion.Open();
            SqlCeCommand deleteClub = new SqlCeCommand("DELETE FROM Clubes WHERE id = @id", conexion);
            deleteClub.Parameters.AddWithValue("@id", Holder.Id);
            SqlCeCommand deleteAssist = new SqlCeCommand("DELETE FROM Asistencias WHERE idClub = @id", conexion);
            deleteAssist.Parameters.AddWithValue("@id", Holder.Id);
            try
            {
                deleteClub.ExecuteNonQuery();
                int asistenciasEliminadas = deleteAssist.ExecuteNonQuery();
                MessageBox.Show(Holder.Nombre + " eliminado.\nEliminadas " + asistenciasEliminadas.ToString() + " asistencias.");
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

        public void clubModificado(object sender, PropertyChangedEventArgs e)
        {
            Holder = (Club)sender;
            switch (e.PropertyName)
            {
                case "Nombre":
                    modificarDato(ValoresModificables.Nombre);
                    break;

                case "Asistencias Necesarias":
                    modificarDato(ValoresModificables.AsistenciasParaAcreditar);
                    break;

                case "Parciales":
                    modificarDato(ValoresModificables.Parciales);
                    break;
            }
            Clear();
        }
    }
}