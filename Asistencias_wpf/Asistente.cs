using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Asistencias_wpf
{
    public class Asistente : INotifyPropertyChanged
    {
        //public int vecesModificada=0;
        public int _cuentaEstatica;
       /* public int cuentaSt
        {
            get
            {
                return _cuentaEstatica;
            }
            set
            {
                if (!fueModificado)
                {
                    _cuentaEstatica = value;
                }
            }
        }*/
        private string _nombre;
        private int _numeroCuenta;
        private string _plantel;
        private int _asistencias;
        public string nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                NotifyPropertyChanged("Nombre");
                modificaciones++;
            }
        }
        public int numeroCuenta
        {
            get { return _numeroCuenta; }
            set
            {
                _numeroCuenta = value;
                if(!(modificaciones>1)) _cuentaEstatica = value;
                NotifyPropertyChanged("Cuenta");
                modificaciones++;
            }
        }
        public string plantel
        {
            get { return _plantel; }
            set
            {
                _plantel = value;
                NotifyPropertyChanged("Plantel");
                modificaciones++;
            }
        }
        public int asistencias
        {
            get { return _asistencias; }
            set
            {
                _asistencias = value;
                NotifyPropertyChanged("Asistencias");
                modificaciones++;
            }
        }
       /* public Asistente(string nombre, int numeroCuenta, string plantel, int asistencias)
        {
            this.nombre = nombre;
            this.numeroCuenta = numeroCuenta;
            this.plantel = plantel;
            this.asistencias = asistencias;
        }*/



        public int modificaciones=0;
        static public bool exists(string cuenta)
        {
            return File.Exists("Accounts/" + cuenta + ".config");
        }
        public void anadirAsistencia(string cuenta)
        {
            asistencias++;
        }
        /*void anadirAsistencia()
         * 
        {
            asistencias++;
        }*/
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

       /* protected virtual void onPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

        }*/

        #endregion

    }
}
