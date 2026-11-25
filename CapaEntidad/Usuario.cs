using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public string NombreUsuario { get; set; }
        public string ContrasenaHash { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; } // Propiedad auxiliar para mostrar el nombre del rol en vistas/sesión
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
