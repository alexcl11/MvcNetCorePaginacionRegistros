using Microsoft.AspNetCore.Mvc;
using MvcNetCorePaginacionRegistros.Models;
using MvcNetCorePaginacionRegistros.Repositories;

namespace MvcNetCorePaginacionRegistros.ViewComponents
{
    public class MenuDepartamentosViewComponent: ViewComponent
    {
        private RepositoryHospital repo;
        public MenuDepartamentosViewComponent(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
            return View(departamentos);
        }
    }
}
