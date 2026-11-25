using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class NovedadPersonal
    {
        public int IdNovedad { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } // Auxiliar para mostrar quién reportó
        public string NombreRol { get; set; } // Auxiliar para saber si fue limpieza, recepción, etc.
        public string TipoRegistro { get; set; }
        public string Area { get; set; }
        public string Detalle { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
