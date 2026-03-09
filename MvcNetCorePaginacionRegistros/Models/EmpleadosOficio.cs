using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCorePaginacionRegistros.Models
{
    public class EmpleadosOficio
    {
        public List<Empleado> Empleados { get; set; }
        public int NumeroRegistros { get; set; }
    }
}
