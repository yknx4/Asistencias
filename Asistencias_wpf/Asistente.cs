using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asistencias_wpf
{
    /// <summary>
    /// Class about an attendant
    /// </summary>
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

        /// <summary>
        /// Gets or sets the attendances of the attendant.
        /// </summary>
        /// <value>
        /// The asistencias.
        /// </value>
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

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                NotifyPropertyChanged("Nombre");
            }
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
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

        /// <summary>
        /// Gets or sets the campus.
        /// </summary>
        /// <value>
        /// The campus.
        /// </value>
        public string plantel
        {
            get { return _plantel; }
            set
            {
                _plantel = value;
                NotifyPropertyChanged("Plantel");
            }
        }

        /// <summary>
        /// Gets or sets the number of attendances.
        /// </summary>
        /// <value>
        /// The Attendances list count.
        /// </value>
        /// <exception cref="System.NotImplementedException">When trying to set attendance number</exception>
        public int asistencias
        {
            get { return _asistencias.Count; }
            set
            {
                throw new NotImplementedException();
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members

        #region Private Helpers

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
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