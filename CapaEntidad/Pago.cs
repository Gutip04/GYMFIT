using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdMembresia { get; set; }
        public int IdUsuarioCobro { get; set; }
        public string NombreUsuarioCobro { get; set; } // Auxiliar para reportes
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
