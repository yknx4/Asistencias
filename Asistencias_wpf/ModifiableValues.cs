using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mValue = System.UInt32;

namespace Asistencias_wpf
{
    static class ModifiableValues
    {
        public const mValue Name = 0;
        public const mValue ID = 1;
        public const mValue Campus = 2;
        public const mValue NeededAttendances = 3;
        public const mValue Partials = 4;
        public static string[] Value = { "Name", "ID", "Campus", "Needed Attendances", "Partials" };
    }
}
