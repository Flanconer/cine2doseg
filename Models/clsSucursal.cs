using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using MySql.Data.MySqlClient;

namespace cine2doseg.Models
{
    public class clsSucursal
    {
        public string clave { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string url { get; set; }
        public string logo { get; set; }

        string cadConn = ConfigurationManager.ConnectionStrings["cine"].ConnectionString;
        public clsSucursal()
        {
            //Codigo pendiente...
        }
    

        public clsSucursal(string nombre, string direccion, string url, string logo)
        {
            this.nombre = nombre;
            this.direccion = direccion;
            this.url = url;
            this.logo = logo;
        }

        public DataSet vwrptsucursales()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "SELECT * FROM vwrptsucursales";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwrptsucursales");
            return ds;
        }
        public DataSet spInSucursal()
        {
            string cadSq = "CALL spInsSucursales('" + this.nombre + "','" + this.direccion + "', '" + this.url + "','" + this.logo + "')";
            // Coniguracion de los objetos
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSq, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "spInsSucursales");
            //retorna  los datos recibidos
            return ds;

        }
    }
}