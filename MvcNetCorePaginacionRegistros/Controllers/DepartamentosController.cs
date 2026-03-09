using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MvcNetCorePaginacionRegistros.Models;
using MvcNetCorePaginacionRegistros.Repositories;

namespace MvcNetCorePaginacionRegistros.Controllers
{
    public class DepartamentosController : Controller
    {
        private RepositoryHospital repo;

        public DepartamentosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int idDepartamento, int? posicion)
        {
            DepartamentoEmpleados departamentoEmpleados;
            if (posicion == null)
            {
                posicion = 1; 
                departamentoEmpleados =
                    await this.repo.GetEmpleadosDepartamentoOutAsync(idDepartamento, posicion.Value);
            }
            else
            {
                
                departamentoEmpleados =
                    await this.repo.GetEmpleadosDepartamentoOutAsync(idDepartamento, posicion.Value);
            }

                Departamento departamento = await this.repo.FindDepartamentoAsync(idDepartamento);
                departamentoEmpleados.IdDepartamento = departamento.IdDepartamento;
                departamentoEmpleados.Nombre = departamento.Nombre;
                departamentoEmpleados.Localidad = departamento.Localidad;
                ViewData["REGISTROS"] = departamentoEmpleados.NumRegistros;
                ViewData["IDDEPARTAMENTO"] = departamento.IdDepartamento;
                ViewData["POSICION"] = posicion;
                ViewData["SIGUIENTE"] = posicion+1;
                ViewData["ANTERIOR"] = posicion - 1;
                return View(departamentoEmpleados);
        }

    }
}
