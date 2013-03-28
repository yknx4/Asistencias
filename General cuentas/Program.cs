using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace General_cuentas
{
    class Program
    {
        // FileStream nombres = new FileStream(@"D:\Sources\GeneradorCuentas\nombres.txt", FileMode.Open, FileAccess.Read);
        //FileStream cuentas = new FileStream(@"D:\Sources\GeneradorCuentas\numerosCuenta.txt", FileMode.Open, FileAccess.Read);
        //FileStream planteles = new FileStream(@"D:\Sources\GeneradorCuentas\planteles.txt", FileMode.Open, FileAccess.Read);
        static StreamReader streamReader1 = new StreamReader(@"D:\Sources\GeneradorCuentas\nombres.txt");
        static StreamReader streamReader2 = new StreamReader(@"D:\Sources\GeneradorCuentas\numerosCuenta.txt");
        static StreamReader streamReader3 = new StreamReader(@"D:\Sources\GeneradorCuentas\planteles.txt");
        
        static void Main(string[] args)
        {
            string nombres = streamReader1.ReadToEnd();
            string numerosCuenta = streamReader2.ReadToEnd();
            string Planteles = streamReader3.ReadToEnd();
            streamReader1.Close();
            streamReader2.Close();
            streamReader3.Close();
            string[] nombresA = nombres.Split('\n');
            string[] numerosA = numerosCuenta.Split('\n');
            string[] plantelesA = Planteles.Split('\n');
            for (int i = 0; i < nombresA.Length-1;i++ )
            {
               // Console.WriteLine(numerosA[i]);
                Console.WriteLine(Generate(nombresA[i],numerosA[i],plantelesA[i]));
            }
            Console.ReadLine();
        }
        static string Generate(string nombre, string numeroCuenta, string plantel)
        {
            nombre = nombre.Substring(0, nombre.Length - 1);
            numeroCuenta = numeroCuenta.Substring(0, numeroCuenta.Length - 1);
            plantel = plantel.Substring(0, plantel.Length - 1);
            string res = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<configuration>\n<appSettings>\n<add key=\"nombre\" value=\"" + nombre + "\"/>\n<add key=\"plantel\" value=\"" + plantel + "\"/>\n<add key=\"_1Parcial\" value=\"0\"/>\n<add key=\"_2Parcial\" value=\"0\"/>\n<add key=\"_3Parcial\" value=\"0\"/>\n</appSettings>\n</configuration>";
            System.IO.File.WriteAllText(@"D:\Sources\GeneradorCuentas\"+numeroCuenta+".config", res);

            return res;
        }
    }
}
