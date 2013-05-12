using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asistencias_wpf
    {
    public class Club
        {
        string _nombre;
        int _id;
        int _asistencias;
        int _parciales;
        public string Nombre
            {
            get { return _nombre; }
            set {_nombre = value;}
            }
        public int Id
            {
            get { return _id; }
            set { _id = value; }
            }
        public int AsistenciasParaParcial
            {
            get { return _asistencias; }
            set { _asistencias = value; }
            }
        public int Parciales
            {
            get { return _parciales; }
            set { _parciales = value; }
            }
        public override string ToString()
            {
            return Nombre;
            }
        }
    }
