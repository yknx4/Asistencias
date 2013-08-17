using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Windows;

namespace Asistencias_wpf
{
    internal class AsistentesPopulator
    {
        private List<Asistente> _acreditados;
        private List<Asistente> _asistentes;
        private DataTable Alumnos = new DataTable();
        private Club seleccionado;
        private SqlCeConnection conn;
        private AsistenteDBManager asistenteDB;
        private int parcial;

        public AsistentesPopulator(SqlCeConnection conn, int parcial, Club seleccionado)
        {
            this.conn = conn;
            this.parcial = parcial;
            this.seleccionado = seleccionado;
            asistenteDB = new AsistenteDBManager(conn);
            generarLista();
            generarAcreditados();
        }

        public List<Asistente> Asistentes
        {
            set { throw new NotImplementedException(); }
            get { return _asistentes; }
        }

        public List<Asistente> Acreditados
        {
            set { throw new NotImplementedException(); }
            get { return _acreditados; }
        }

        public void generarAcreditados()
        {
            _acreditados = new List<Asistente>();
            foreach (Asistente Alumno in _asistentes)
            {
                if (Alumno.asistencias >= seleccionado.AsistenciasParaParcial) _acreditados.Add(Alumno);
            }
        }

        public void generarLista()
        {
            _asistentes = new List<Asistente>();
            SqlCeDataAdapter adap = new SqlCeDataAdapter("SELECT Alumnos.* FROM Alumnos", conn);
            adap.Fill(Alumnos);
            foreach (DataRow Row in Alumnos.Rows)
            {
                // this.Title = Row["NumeroCuenta"].ToString();
                int accountValue = Convert.ToInt32(Row["NumeroCuenta"].ToString());
                string nmb = Row["Nombre"].ToString();
                string ptl = Row["Plantel"].ToString();
                DataTable Asistencias = new DataTable();
                SqlCeDataAdapter asist = new SqlCeDataAdapter("SELECT Asistencias.* FROM Asistencias WHERE (idClub = " + seleccionado.Id + ") AND (idAlumno = " + accountValue + ") AND (parcial = " + parcial + ")", conn);
                asist.Fill(Asistencias);
                List<Asistencia> Asistencia = new List<Asistencia>();
                foreach (DataRow aRow in Asistencias.Rows)
                {
                    DateTime tmpDate;
                    try
                    {
                        tmpDate = Convert.ToDateTime(aRow["date"]);
                    }
                    catch (System.InvalidCastException)
                    {
                        tmpDate = new DateTime(0);
                    }
                    Asistencia tmpAsistencia = new Asistencia()
                    {
                        //ID = Convert.ToInt32(aRow["id"].ToString()),
                        Date = tmpDate,
                        Parcial = parcial,
                    };
                    Asistencia.Add(tmpAsistencia);
                }
                Asistente actual = new Asistente()
                {
                    nombre = nmb,
                    numeroCuenta = accountValue,
                    plantel = ptl,

                    // asistencias = Asistencia.Count,
                    Asistencias = Asistencia,
                };
                actual.PropertyChanged += cuentaModificada;
                _asistentes.Add(actual);
            }
        }

        public void cuentaModificada(object sender, PropertyChangedEventArgs e)
        {
            MessageBox.Show(e.PropertyName + " Modificado");
            /*

            //UPDATE Alumnos SET Nombre = N'Jorge Figueroa Perez' WHERE (Alumnos.NumeroCuenta = 20094894)//
            Asistente source = (Asistente)sender;
            switch (e.PropertyName)
                {
                case "Nombre":
                    if (source.saveData(e.PropertyName))
                        {
                        lblEstado.Content = "Nombre de " + source.nombre + " modificado.";
                        }
                    else lblEstado.Content = "Error al modificar " + source.nombre + ".";

                    break;

                case "Cuenta":
                    if (source.numeroCuenta.ToString().Length != 8)
                        {
                        lblEstado.Content = "Cuenta Incorrecta, " + source.nombre + " no se modifico.";
                        }
                    else
                        {
                        if (source.saveData(e.PropertyName))
                            {
                            lblEstado.Content = "Cuenta de " + source.nombre + " modificada.";
                            }
                        else lblEstado.Content = "Error al modificar " + source.numeroCuenta + ".";
                        generarLista();
                        }
                    break;

                case "Plantel":
                    if (source.saveData(e.PropertyName))
                        {
                        lblEstado.Content = "Plantel de " + source.nombre + " Modificado.";
                        }else lblEstado.Content = "Plantel de " + source.nombre + " no modificado.";
                    break;
                }*/
        }
    }
}