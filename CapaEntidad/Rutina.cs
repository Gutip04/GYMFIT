using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Rutina
    {
        public int IdRutina { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } // Auxiliar para el entrenador
        public int IdEntrenador { get; set; }
        public string NombreEntrenador { get; set; } // Auxiliar para mostrar quién la asignó
        public string Objetivo { get; set; }
        public string DescripcionRutina { get; set; }
        public string ObservacionesFisicas { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
