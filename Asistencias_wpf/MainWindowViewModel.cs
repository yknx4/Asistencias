using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace Asistencias_wpf
{
    public class MainWindowViewModel
    {
        public ICollectionView Asistentes { get; private set; }

        // public ICollectionView Acreditados { get; private set; }

        public MainWindowViewModel(List<Asistente> inputData)
        {
            List<Asistente> _asistentes = inputData;

            Asistentes = CollectionViewSource.GetDefaultView(_asistentes);

            /*Acreditados = new ListCollectionView(_customers);
            Acreditados.GroupDescriptions.Add(new PropertyGroupDescription("Gender"));*/
        }
    }
}