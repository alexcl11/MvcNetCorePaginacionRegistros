using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.CodeAnalysis.Elfie.Model;
using MvcNetCorePaginacionRegistros.Models;
using MvcNetCorePaginacionRegistros.Repositories;

namespace MvcNetCorePaginacionRegistros.Controllers
{
    public class PaginacionController : Controller
    {
        private RepositoryHospital repo;

        public PaginacionController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> RegistroVistaDepartamento(int? posicion)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            int siguiente = posicion.Value + 1;
            if (siguiente > numRegistros)
            {
                siguiente = numRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = numRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            VistaDepartamento vista = await this.repo.GetVistaDepartamentoAsync(posicion.Value);
            return View(vista);
        }

        public async Task<IActionResult> GrupoVistaDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            // LOS SIGUIENTE SERA QUE DEBERMOS DIBUJAR LOS NUMEROS DE PAGINA EN LOS LINKS
            // NECESITAMOS UNA VARIABLE PARA EL NUMERO DE PAGINA
            // VOY A REALIZAR EL DIBUJO DESDE AQUI, NO DESDE RAZOR
            int numPagina = 1;
            int numRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            ViewData["NUMREGISTROS"] = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            ViewData["NUMPAGINA"] = 1;
            string html = "<div class='container'>";
            for (int i = 1; i <= numRegistros; i+=2)
            {
                html += "<a href='GrupoVistaDepartamentos?posicion="+i+"'>Página "+numPagina+"</a>";
                numPagina += 1;
            }

            html += "</div>";
            ViewData["LINKS"] = html;
            List<VistaDepartamento> departamentos = 
                await this.repo.GetGrupoVistaDepartamentoAsync(posicion.Value);
            return View(departamentos);
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GrupoDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            // LOS SIGUIENTE SERA QUE DEBERMOS DIBUJAR LOS NUMEROS DE PAGINA EN LOS LINKS
            // NECESITAMOS UNA VARIABLE PARA EL NUMERO DE PAGINA
            // VOY A REALIZAR EL DIBUJO DESDE AQUI, NO DESDE RAZOR
            int numPagina = 1;
            int numRegistros = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            ViewData["NUMREGISTROS"] = await this.repo.GetNumeroRegistrosVistaDepartamentosAsync();
            ViewData["NUMPAGINA"] = 1;
            string html = "<div class='container'>";
            for (int i = 1; i <= numRegistros; i += 2)
            {
                html += "<a href='GrupoVistaDepartamentos?posicion=" + i + "'>Página " + numPagina + "</a>";
                numPagina += 1;
            }

            html += "</div>";
            ViewData["LINKS"] = html;
            List<Departamento> departamentos =
                await this.repo.GetGrupoDepartamentosAsync(posicion.Value);
            return View(departamentos);
        }

        public async Task<IActionResult> PaginarGrupoEmpleados(int? posicion)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            int numRegistros = await this.repo.GetEmpleadosCountAsync();
            ViewData["REGISTROS"] = numRegistros;
            List<Empleado> empleados = await this.repo.GetGrupoEmpleadosAsync(posicion.Value);
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosOficio(int? posicion, string oficio)
        {
            if (posicion == null )
            {
                posicion = 1;
                return View();
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosOficioAsync(oficio, posicion.Value);
                int registros = await this.repo.GetEmpleadosOficioCountAsync(oficio);
                ViewData["REGISTROS"] = registros;
                ViewData["OFICIO"] = oficio;
                return View(empleados);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EmpleadosOficio(string oficio)
        {
            List<Empleado> empleados = await this.repo.GetEmpleadosOficioAsync(oficio, 1);
            int registros = await this.repo.GetEmpleadosOficioCountAsync(oficio);
            ViewData["REGISTROS"] = registros;
            ViewData["OFICIO"] = oficio;
            return View(empleados);
        }
        public async Task<IActionResult> EmpleadosOficioOut(int? posicion, string oficio)
        {
            if (posicion == null)
            {
                posicion = 1;
                return View();
            }
            else
            {
                EmpleadosOficio empleados = await this.repo.GetEmpleadosOficioOutAsync(oficio, posicion.Value);
                
                ViewData["REGISTROS"] = empleados.NumeroRegistros;
                ViewData["OFICIO"] = oficio;
                return View(empleados.Empleados);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EmpleadosOficioOut(string oficio)
        {
            EmpleadosOficio empleados = await this.repo.GetEmpleadosOficioOutAsync(oficio, 1);
            
            ViewData["REGISTROS"] = empleados.NumeroRegistros;
            ViewData["OFICIO"] = oficio;
            return View(empleados.Empleados);
        }
    }
}
