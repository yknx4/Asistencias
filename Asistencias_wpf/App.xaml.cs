using Asistencias_wpf.Properties;
using System.Windows;

namespace Asistencias_wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void alSalir(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}