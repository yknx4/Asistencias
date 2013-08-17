using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asistencias_wpf
{
    public class Asistente : INotifyPropertyChanged
    {
        private int _cuentaAnterior;
        private string _nombre;
        private int _numeroCuenta;
        private string _plantel;
        private List<Asistencia> _asistencias = new List<Asistencia>();

        public int CuentaOriginal()
        {
            return _cuentaAnterior;
        }

        public List<Asistencia> Asistencias
        {
            get
            {
                return _asistencias;
            }
            set
            {
                _asistencias = value;
            }
        }

        public string nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                NotifyPropertyChanged("Nombre");
            }
        }

        public int numeroCuenta
        {
            get { return _numeroCuenta; }
            set
            {
                _cuentaAnterior = _numeroCuenta;
                _numeroCuenta = value;
                NotifyPropertyChanged("Cuenta");
            }
        }

        public string plantel
        {
            get { return _plantel; }
            set
            {
                _plantel = value;
                NotifyPropertyChanged("Plantel");
            }
        }

        public int asistencias
        {
            get { return _asistencias.Count; }
            set
            {
                throw new NotImplementedException();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Private Helpers
    }
}