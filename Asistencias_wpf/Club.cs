using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace Asistencias_wpf
{
    public class Club
    {
        private string _nombre;
        private int _id;
        private int _asistencias;
        private int _parciales;

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                
                _nombre = value;
                NotifyPropertyChanged("Nombre");
            }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int AsistenciasParaParcial
        {
            get { return _asistencias; }
            set
            {
                _asistencias = value;
                NotifyPropertyChanged("Asistencias Necesarias");
            }
        }

        public int Parciales
        {
            get { return _parciales; }
            set
            {
                _parciales = value;
                NotifyPropertyChanged("Parciales");
            }
        }

        public override string ToString()
        {
            return Nombre;
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