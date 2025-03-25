using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cine2doseg.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cine2doseg.Controllers
{
    public class SucursalController : ApiController
    {
        [HttpPost]
        [Route("check/Sucursal/spinsucursal")]
        public clsApiStatus spInSucursal([FromBody] clsSucursal modelo)
        {
            // Validar si el objeto modelo es nulo
            if (modelo == null)
            {
                return new clsApiStatus
                {
                    statusExec = false,
                    msg = "Error: Datos de sucursal no recibidos.",
                    ban = -1
                };
            }

            clsApiStatus objrespuesta = new clsApiStatus();
            JObject jsonresp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                // Validar que los campos del modelo no sean nulos
                if (string.IsNullOrEmpty(modelo.nombre) || string.IsNullOrEmpty(modelo.direccion) ||
                    string.IsNullOrEmpty(modelo.url) || string.IsNullOrEmpty(modelo.logo))
                {
                    return new clsApiStatus
                    {
                        statusExec = false,
                        msg = "Error: Todos los campos de la sucursal son obligatorios.",
                        ban = -1
                    };
                }

                // Crear instancia de clsSucursal y llamar al procedimiento almacenado
                clsSucursal objSucursal = new clsSucursal(modelo.nombre, modelo.direccion, modelo.url, modelo.logo);
                ds = objSucursal.spInSucursal();

                // Verificar si el DataSet contiene datos antes de acceder a ellos
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objrespuesta.statusExec = true;
                    objrespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    objrespuesta.msg = "Sucursal registrada exitosamente";
                    jsonresp.Add("msData", "Sucursal registrada exitosamente");
                }
                else
                {
                    objrespuesta.statusExec = false;
                    objrespuesta.ban = -1;
                    objrespuesta.msg = "No se recibieron datos desde la base de datos.";
                    jsonresp.Add("msData", "No se recibió información de la base de datos.");
                }

                objrespuesta.datos = jsonresp;
            }
            catch (Exception ex)
            {
                objrespuesta.statusExec = false;
                objrespuesta.ban = -1;
                objrespuesta.msg = "Fallo la inserción de la sucursal, verificar...";
                jsonresp.Add("msData", ex.Message);
                objrespuesta.datos = jsonresp;
            }

            return objrespuesta;
        }
        [HttpGet]
        [Route("check/Sucursal/vwrptsucursales")]
        public clsApiStatus vwrptsucursales()
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                clsSucursal objUsuario = new clsSucursal();
                ds = objUsuario.vwrptsucursales();
                //Configuracion del objSalida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte consultado exitosamente";
                //Formatear los datos recibidos (Data set) para 
                //enviarlos de salida(JSON)
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                //Configuracion del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            return objRespuesta;
        }

    }
}
