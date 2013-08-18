using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;

namespace Asistencias_wpf
{
    internal class AsistentesPopulator
    {
        private List<Asistente> _acreditados;
        private List<Asistente> _asistentes;
        private DataTable Alumnos = new DataTable();
        private Club clubSeleccionado;
        private SqlCeConnection conn;
        private AsistenteDBManager asistenteDB;
        private int parcial;

        public void cuentaModificada(object sender, PropertyChangedEventArgs e)
        {
            asistenteDB.cuentaModificada(sender, e);
        }

        public AsistentesPopulator(SqlCeConnection conn, int parcial, Club seleccionado)
        {
            this.conn = conn;
            this.parcial = parcial;
            this.clubSeleccionado = seleccionado;
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
                if (Alumno.asistencias >= clubSeleccionado.AsistenciasParaParcial) _acreditados.Add(Alumno);
            }
        }

        public void generarLista()
        {
            Alumnos.Clear();
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
                SqlCeDataAdapter asist = new SqlCeDataAdapter("SELECT Asistencias.* FROM Asistencias WHERE (idClub = " + clubSeleccionado.Id + ") AND (idAlumno = " + accountValue + ") AND (parcial = " + parcial + ")", conn);
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
                    Nombre = nmb,
                    numeroCuenta = accountValue,
                    plantel = ptl,

                    // asistencias = Asistencia.Count,
                    Asistencias = Asistencia,
                };
                actual.PropertyChanged += cuentaModificada;
                _asistentes.Add(actual);
            }
        }
    }
}