using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCorePaginacionRegistros.Data;
using MvcNetCorePaginacionRegistros.Models;
using System.Collections.Generic;

#region VISTAS Y PROCEDIMIENTOS
/*
 CREATE VIEW V_GRUPO_EMPLEADOS
AS
	SELECT CAST(ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION, EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
	FROM EMP
GO

CREATE PROCEDURE SP_GRUPO_EMPLEADOS
(@posicion INT) 
AS
	SELECT EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
	FROM V_GRUPO_EMPLEADOS
	WHERE POSICION >= @posicion AND POSICION < (@posicion + 3)
GO
 

CREATE OR ALTER PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO(@posicion INT, @oficio NVARCHAR(50), @registros INT OUT)
AS
	SELECT @registros= COUNT(EMP_NO) FROM EMP WHERE OFICIO=@oficio
	SELECT EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO  FROM 
	(SELECT CAST(ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION, EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
	FROM EMP
	WHERE OFICIO = @oficio) QUERY
	WHERE (QUERY.POSICION >= @posicion AND QUERY.POSICION < (@posicion +3))
GO

CREATE OR ALTER PROCEDURE SP_GRUPO_EMPLEADOS_DEPARTAMENTO(@posicion INT, @departamento int, @registros INT OUT)
AS
	SELECT @registros= COUNT(EMP_NO) FROM EMP WHERE DEPT_NO=@departamento
	SELECT EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO  FROM 
	(SELECT CAST(ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION, EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
	FROM EMP
	WHERE DEPT_NO = @departamento) QUERY
	WHERE (QUERY.POSICION = @posicion)
GO
 */
#endregion

namespace MvcNetCorePaginacionRegistros.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;
        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }
        public async Task<int> GetNumeroRegistrosVistaDepartamentosAsync()
        {
            return await this.context.VistaDepartamentos.CountAsync();
        }
        public async Task<VistaDepartamento> GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento departamento =
                await this.context.VistaDepartamentos.Where(z => z.Posicion == posicion)
                .FirstOrDefaultAsync();
            return departamento;
        }
        public async Task<List<VistaDepartamento>> GetGrupoVistaDepartamentoAsync(int posicion)
        {
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion && datos.Posicion < (posicion + 2)
                           select datos;
            List<VistaDepartamento> departamentos = await consulta.ToListAsync();
            return departamentos;
        }

        public async Task<List<Departamento>> GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @posicion";
            SqlParameter paramPosicion = new SqlParameter("@posicion", posicion);

            var consulta = this.context.Departamentos.FromSqlRaw(sql, paramPosicion);
            return await consulta.ToListAsync();
        }

        public async Task<int> GetEmpleadosCountAsync()
        {
            return await this.context.Empleados.CountAsync();
        }

        public async Task<List<Empleado>> GetGrupoEmpleadosAsync(int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS @posicion";
            SqlParameter paramPosicion = new SqlParameter("@posicion", posicion);
            var consulta = this.context.Empleados.FromSqlRaw(sql, paramPosicion);
            return await consulta.ToListAsync();
        }
        public async Task<int> GetEmpleadosOficioCountAsync(string oficio)
        {
            return await this.context.Empleados.Where(e => e.Oficio == oficio).CountAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosOficioAsync(string oficio, int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO @posicion, @oficio";
            SqlParameter paramPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter paramOficio = new SqlParameter("@oficio", oficio);

            var consulta = this.context.Empleados.FromSqlRaw(sql, paramPosicion, paramOficio);
            return await consulta.ToListAsync();
        }

        public async Task<EmpleadosOficio> GetEmpleadosOficioOutAsync(string oficio, int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO @posicion, @oficio, @registros out";
            SqlParameter paramPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter paramOficio = new SqlParameter("@oficio", oficio);
            SqlParameter paramRegistros = new SqlParameter("@registros", 0);
            paramRegistros.DbType = System.Data.DbType.Int32;
            paramRegistros.Direction = System.Data.ParameterDirection.Output;

            var consulta = this.context.Empleados.FromSqlRaw(sql, paramPosicion, paramOficio, paramRegistros);
            List<Empleado> empleados = await consulta.ToListAsync();
            int registros = (int)paramRegistros.Value;
            return new EmpleadosOficio
            {
                Empleados = empleados,
                NumeroRegistros = registros
            };
            
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            return await this.context.Departamentos.ToListAsync();
        }

        public async Task<Departamento> FindDepartamentoAsync(int id)
        {
            return await this.context.Departamentos.Where(d => d.IdDepartamento == id).FirstOrDefaultAsync();
        }

        public async Task<DepartamentoEmpleados> GetEmpleadosDepartamentoOutAsync(int departamento, int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS_DEPARTAMENTO @posicion, @departamento, @registros out";
            SqlParameter paramPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter paramDepartamento = new SqlParameter("@departamento", departamento);
            SqlParameter paramRegistros = new SqlParameter("@registros", 0);
            paramRegistros.DbType = System.Data.DbType.Int32;
            paramRegistros.Direction = System.Data.ParameterDirection.Output;

            var consulta = this.context.Empleados.FromSqlRaw(sql, paramPosicion, paramDepartamento, paramRegistros);
            List<Empleado> empleados = await consulta.ToListAsync();
            int registros = (int)paramRegistros.Value;
            return new DepartamentoEmpleados
            {
                Empleados = empleados,
                NumRegistros = registros
            };

        }
    }
}
