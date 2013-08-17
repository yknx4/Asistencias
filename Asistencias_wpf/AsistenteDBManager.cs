﻿using System;
using System.Data.SqlServerCe;
using System.Windows;

namespace Asistencias_wpf
{
    internal class AsistenteDBManager
    {
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
    }
}