using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class RegistroAcceso
    {
        public int IdAcceso { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } // Auxiliar para historial de ingresos
        public string DocumentoCliente { get; set; } // Auxiliar para historial de ingresos
        public DateTime FechaHoraIngreso { get; set; }
        public bool AccesoPermitido { get; set; }
        public string Observacion { get; set; }
    }
}
