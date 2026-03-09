namespace MvcNetCorePaginacionRegistros.Models
{
    public class DepartamentoEmpleados
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; }
        public string Localidad { get; set; }
        public int NumRegistros { get; set; }
        public List<Empleado> Empleados { get; set; }
    }
}
