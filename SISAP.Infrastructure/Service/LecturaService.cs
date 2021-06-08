﻿using SISAP.Core.Interfaces;
using SISAP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SISAP.Infrastructure.Service
{
	public class LecturaService : _BaseContext, ILecturaService
	{
		public void UpdateProcessLectura(UpdateLectura updateLectura)
		{
			using (var dbContext = GetSISAPDBContext())
			{
				var data = dbContext.Lecturas.First(l =>l.Annio == updateLectura.Annio && l.Mes == updateLectura.Mes && l.ClienteId == updateLectura.ClienteId);

				data.CantidadLecturaAntigua = updateLectura.CantidadLecturaActualizar;
				dbContext.SaveChanges();
			}
		}

		public IEnumerable<Lectura> CheckIfExistLecturaNYM(int? Annio, int? Mes, int? UrbanizacionId)
		{
			using (var dbContext = GetSISAPDBContext())
			{
				int? value = 0;

				var year = (from l in dbContext.Lecturas
							join c in dbContext.Clientes on l.ClienteId equals c.ClienteId
							join u in dbContext.Urbanizacions on c.UrbanizacionId equals u.UrbanizacionId
							where (l.Annio > Annio)
							select l).ToList();

				var month = (from l in dbContext.Lecturas
							join c in dbContext.Clientes on l.ClienteId equals c.ClienteId
							join u in dbContext.Urbanizacions on c.UrbanizacionId equals u.UrbanizacionId
							where (l.Mes == 12)
							select l).ToList();



				var sql = (from l in dbContext.Lecturas
						   join c in dbContext.Clientes on l.ClienteId equals c.ClienteId
						   join u in dbContext.Urbanizacions on c.UrbanizacionId equals u.UrbanizacionId
						   where (l.Annio == Annio
								&& (l.Mes > Mes) 
								&& u.UrbanizacionId == UrbanizacionId
								)
						   select l).ToList();
				return sql;

			}
		}
		public IEnumerable<Lectura> CheckIfExistLectura(int? Annio, int? Mes, int? UrbanizacionId)
		{
			using (var dbContext = GetSISAPDBContext())
			{
				var sql = (from l in dbContext.Lecturas
						   join c in dbContext.Clientes on l.ClienteId equals c.ClienteId
						   join u in dbContext.Urbanizacions on c.UrbanizacionId equals u.UrbanizacionId
						   where (l.Annio == Annio && (l.Mes > Mes) && u.UrbanizacionId == UrbanizacionId)
						   select l).ToList();
				return sql;

			}
		}
		public IEnumerable<Lectura> ValidateNextYearUpdateLectura(int? Annio, int? Mes, int? UrbanizacionId)
		{
			using (var dbContext = GetSISAPDBContext())
			{
				var sql = (from l in dbContext.Lecturas
						   join c in dbContext.Clientes on l.ClienteId equals c.ClienteId
						   join u in dbContext.Urbanizacions on c.UrbanizacionId equals u.UrbanizacionId
						   where ( l.Annio > Annio && l.Mes == Mes && u.UrbanizacionId == UrbanizacionId)
						   select l).ToList();
				return sql;
			}
		}
		public IEnumerable<Lectura> ValidateValueNoNullable(int? Annio, int? Mes, int? UrbanizacionId)
		{
			using (var dbContext = GetSISAPDBContext())
			{
				var sql = (from l in dbContext.Lecturas
						   join c in dbContext.Clientes on l.ClienteId equals c.ClienteId
						   join u in dbContext.Urbanizacions on c.UrbanizacionId equals u.UrbanizacionId
						   where ( l.Annio == Annio && l.Mes == Mes && u.UrbanizacionId == UrbanizacionId)
						   select l).ToList();
				return sql;
			}
		}

		public void UpdateDataExistLectura(Lectura objLectura)
		{
			using (var dbContext = GetSISAPDBContext())
			{
				dbContext.Lecturas.Attach(objLectura);
				dbContext.Entry(objLectura).State = EntityState.Modified;
				dbContext.SaveChanges();
			}
		}

		public Lectura Create(Lectura objLectura)
		{
			using (var dbContext = GetSISAPDBContext())
			{
				dbContext.Lecturas.Add(objLectura);
				dbContext.SaveChanges();
			}
			return objLectura;
		}

		public IEnumerable<Lectura> GetFirst6Data()
		{
			using (var dbContext = GetSISAPDBContext())
			{
				var top6 = dbContext.Lecturas.OrderByDescending(u => u.Mes).Take(6);
				var listadoFinal = top6.ToList();
				return listadoFinal;

			}
		}

		public IEnumerable<Cliente> ListarClienteLectura(int? Annio, int?Mes, int? UrbanizacionId, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros)
		{
			using (var dbContext = GetSISAPDBContext())
			{
				var sql = (from c in dbContext.Clientes
						   join l in dbContext.Lecturas on c.ClienteId equals l.ClienteId into dcl
						   from lec in dcl.DefaultIfEmpty()
						   join u in dbContext.Urbanizacions on c.UrbanizacionId equals u.UrbanizacionId
						   join m in dbContext.Manzanas on c.ManzanaId equals m.ManzanaId
						   where (c.UrbanizacionId == UrbanizacionId || null == UrbanizacionId)
								&& (lec.Annio == Annio || lec.Annio == null)
								&& (lec.Mes == Mes || lec.Mes == null)
								&& (String.IsNullOrEmpty(FilterNombre) || c.Nombre.Contains(FilterNombre))
						   orderby c.Nombre
						   select new
						   {
							   c.ClienteId,
							   c.Nombre,
							   c.Apellido,
							   c.Telefono,
							   c.Direccion,
							   c.UrbanizacionId,
							   c.ManzanaId,
							   c.NumeroMedidor,
							   c.Complemento,
							   u.NombreUrbanizacion,
							   m.NombreManzana,
							   LecturaId = lec ==null? 0 : lec.LecturaId,
							   Annio = lec ==null? 00 : lec.Annio,
							   Mes = lec ==null? 00 : lec.Mes,
							   CantidadLectura = lec ==null? 0 : lec.CantidadLectura,
							   Consumo = lec ==null? 00 : lec.Consumo,
							   Promedio = lec ==null? 00 : lec.Promedio,
							   Alerta = lec ==null? String.Empty : lec.Alerta,
							   CantidadLecturaAntigua = lec == null? 00 : lec.CantidadLecturaAntigua

						   });
				nroTotalRegistros = sql.Count();
				sql = sql.Skip(skip).Take(pageSize);

				var ListadoFinal = (from c in sql.ToList()
									select new Cliente()
									{
										ClienteId = c.ClienteId,
										Nombre = c.Nombre,
										Apellido = c.Apellido,
										Telefono = c.Telefono,
										Direccion = c.Direccion,
										UrbanizacionId = c.UrbanizacionId,
										ManzanaId = c.ManzanaId,
										NumeroMedidor = c.NumeroMedidor,
										UrbanizacionNombre = c.NombreUrbanizacion,
										Complemento = c.Complemento,
										ManzanaNombre = c.NombreManzana,
										LecturaId = c.LecturaId,
										Annio = c.Annio,
										Mes = c.Mes,
										CantidadLectura = c.CantidadLectura,
										Consumo = c.Consumo,
										Promedio = c.Promedio,
										Alerta = c.Alerta,
										CantidadLecturaAntigua = c.CantidadLecturaAntigua
									}).ToList();
				return ListadoFinal;
			}
		}
	}
}
