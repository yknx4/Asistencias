using System;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Windows;
using mValue = System.UInt32;

namespace Asistencias_wpf
{
    internal abstract class DBManager<T>
    {
        protected DBManager(SqlCeConnection connection){
            this.connection = connection;
        }

        abstract public bool AddToDB();

        virtual public void itemModified(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            setItem((T)sender);
        }

        abstract public bool modificarDato(mValue modificar);

        virtual public bool RemoveFromDB()
        {
            throw new NotImplementedException();
        }


        protected T heldItem;

        protected SqlCeConnection connection;

        public void setItem(T Input)
        {
            heldItem = Input;
        }

        public void Clear()
        {
            heldItem = default(T);
        }

        protected bool Active()
        {
            if (heldItem == null) return false;
            return true;
        }

        
    }
}