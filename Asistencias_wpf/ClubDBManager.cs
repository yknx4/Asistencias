using System;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Windows;

namespace Asistencias_wpf
{
    internal class ClubDBManager : DBManager<Club>
    {

        public ClubDBManager(SqlCeConnection conexion) : base(conexion)
        {
        }

        public override bool modificarDato(UInt32 modificar)
        {
            //UPDATE Alumnos SET Nombre = N'Jorge Figueroa Perez' WHERE (Alumnos.NumeroCuenta = 20094894)//
            if (!Active()) return false;
            connection.Open();
            int clubIDQuery = heldItem.Id;
            string valorAModificarQuery = "";
            string valorNuevoQuery = "";
            switch (modificar)
            {
                case ModifiableValues.Name:

                    valorAModificarQuery = "nombre";
                    valorNuevoQuery = heldItem.Nombre;
                    break;

                case ModifiableValues.NeededAttendances:

                    valorAModificarQuery = "asisForAssist";
                    valorNuevoQuery = heldItem.AsistenciasParaParcial.ToString();
                    break;

                case ModifiableValues.Partials:

                    valorAModificarQuery = "parciales";
                    valorNuevoQuery = heldItem.Parciales.ToString();
                    break;

                default:
                    throw new NotImplementedException();
            }
            SqlCeCommand cmd = new SqlCeCommand("UPDATE Clubes SET " + valorAModificarQuery + " = N'" + valorNuevoQuery + "' WHERE (Clubes.id = " + clubIDQuery.ToString() + ")", connection);

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
            SqlCeCommand cmd = new SqlCeCommand("INSERT INTO Clubes(nombre, asisForAssist, parciales)VALUES(@nombre, @asisForAssist, @parciales)", connection);
            cmd.Parameters.AddWithValue("@nombre", heldItem.Nombre);
            cmd.Parameters.AddWithValue("@asisForAssist", heldItem.AsistenciasParaParcial);
            cmd.Parameters.AddWithValue("@parciales", heldItem.Parciales);
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

        public override bool RemoveFromDB()
        {
            if (!Active()) return false;
            connection.Open();
            SqlCeCommand deleteClub = new SqlCeCommand("DELETE FROM Clubes WHERE id = @id", connection);
            deleteClub.Parameters.AddWithValue("@id", heldItem.Id);
            SqlCeCommand deleteAssist = new SqlCeCommand("DELETE FROM Asistencias WHERE idClub = @id", connection);
            deleteAssist.Parameters.AddWithValue("@id", heldItem.Id);
            try
            {
                deleteClub.ExecuteNonQuery();
                int asistenciasEliminadas = deleteAssist.ExecuteNonQuery();
                MessageBox.Show(heldItem.Nombre + " eliminado.\nEliminadas " + asistenciasEliminadas.ToString() + " asistencias.");
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

                case "Asistencias Necesarias":
                    modificarDato(ModifiableValues.NeededAttendances);
                    break;

                case "Parciales":
                    modificarDato(ModifiableValues.Partials);
                    break;
                default:
                    throw new NotImplementedException(e.PropertyName + " hasn't been implemented.");
            }
            Clear();
        }
    }
}