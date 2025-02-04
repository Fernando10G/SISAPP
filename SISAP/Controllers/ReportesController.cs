﻿using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SISAP.Core.Interfaces;
using SISAP.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SISAP.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IReportesService _reportesService;
        public ReportesController ()
		{
            _reportesService = new ReportesService();

        }
        // GET: Reportes
        public ActionResult Reportes()
        {
            return View();
        }
        public ActionResult LecturasProcesadas()
        {
            return View();
        }

        public ActionResult CriticaLectura()
        {
            return View();
        }
        public ActionResult ReportesDeudas()
        {
            return View();
        }
        public ActionResult ReportesDeudasDistrito()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeudaRuta(int? Annio, int? Mes, int? UrbanizacionId)
		{
             var cantidad = _reportesService.getDeudaRuta(Annio, Mes, UrbanizacionId);
            return Json(new { respuesta = cantidad }, JsonRequestBehavior.AllowGet);
		}
        
        [HttpPost]
        public JsonResult DeudaDistrito(int? Annio)
		{
             var cantidad = _reportesService.getDeudaDistrito(Annio);
            return Json(new { respuesta = cantidad }, JsonRequestBehavior.AllowGet);
		}
        
        [HttpPost]
        public JsonResult IngresosAnuales(int? Annio)
		{
             var cantidad = _reportesService.getIngresoAnual(Annio);
            return Json(new { respuesta = cantidad }, JsonRequestBehavior.AllowGet);
		}
                
        [HttpPost]
        public JsonResult IngresosMensuales(int? Annio, int? Mes)
		{
             var cantidad = _reportesService.getIngresoMensual(Annio, Mes);
            return Json(new { respuesta = cantidad }, JsonRequestBehavior.AllowGet);
		}
        
        [HttpPost]
        public JsonResult AllLecturas(int? Annio, int? Mes)
		{
             var cantidad = _reportesService.getProcessLectura(Annio, Mes);
            return Json(new { respuesta = cantidad }, JsonRequestBehavior.AllowGet);
		}


        [HttpPost]
        public JsonResult ListMainReporte(int? Annio, string FilterNombre)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;

            var lecturas = _reportesService.ListReporte(Annio, FilterNombre, pageSize, skip, out nroTotalRegistros);

            return Json(new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = lecturas }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListMain(int? UrbanizacionId, string FilterNombre)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;

            var dPagos = _reportesService.GetAllCF(UrbanizacionId, FilterNombre, pageSize, skip, out nroTotalRegistros);

            return Json(new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = dPagos }, JsonRequestBehavior.AllowGet);
        }


        #region "ReporteRuta"

        public ActionResult ReporteRuta(int urb)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["SISAPDBContext"].ConnectionString;
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/ReportesCR"), "rptDeudaRuta.rpt"));
            rd.SetParameterValue("@urbanizacionId", urb);
            //rd.DataSourceConnections[0].IntegratedSecurity = true;
            //rd.DataSourceConnections[0].SetConnection("DESKTOP-KTMHKON", "SISAP-DEV", true);
            Response.Buffer = false;
            Response.ClearContent();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf"/*"facturas.pdf"*/);

        }
        #endregion



        #region "ReporteDistrito"

        public ActionResult ReporteAnnio(int urb)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["SISAPDBContext"].ConnectionString;
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/ReportesCR"), "rptLectura.rpt"));
            rd.SetParameterValue("@urbanizacionId", urb);
            //rd.DataSourceConnections[0].IntegratedSecurity = true;
            //rd.DataSourceConnections[0].SetConnection("DESKTOP-KTMHKON", "SISAP-DEV", true);
            Response.Buffer = false;
            Response.ClearContent();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf"/*"facturas.pdf"*/);

        }
        #endregion
    }
}