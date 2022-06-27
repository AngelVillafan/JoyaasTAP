using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyaasTAP.Models
{
    public enum Tipo { Collar, Anillo, Aretes, Brazaletes, Relojes}
    public class Joya
    {
        public string Nombre { get; set; } = "";
        public string Origen { get; set; } = "";
        public Tipo Tipo { get; set; }
        public string Peso { get; set; } = "";
        public string Foto { get; set; } = "";
        public string Descripcion { get; set; } = "";

    }
}
