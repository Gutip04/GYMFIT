using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Membresia
    {
        public int IdMembresia { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } // Auxiliar para listados y GridViews
        public string DocumentoCliente { get; set; } // Auxiliar para búsquedas rápidas
        public int IdPlan { get; set; }
        public string NombrePlan { get; set; } // Auxiliar para mostrar el plan adquirido
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
    }
}
