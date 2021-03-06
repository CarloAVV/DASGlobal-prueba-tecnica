using DASGlobal.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DASGlobal.Controllers
{
    public class HomeController : Controller
    {
        private static string UserDB;
        private static string PassDB;

        private static class Queries
        {
            public const string CheckLogin = "insert into CheckLogin values(getdate()) select count(*) from checklogin";
            public const string SelectAllEmpresa = "select * from empresa";
            public const string SelectAllSucursal = "select * from sucursal";
            public const string SelectAllColaborador = "select * from colaborador";

            public const string GetAllData =
                "select " +
                        "e.Nombre as [Empresa], " +
                        "e.Pais as [Pais], " +
                        "s.Nombre as [Sucursal], " +
                        "s.Direccion as [SucursalDireccion], " +
                        "s.Telefono as [SucursalTelefono]," +
                        "c.Nombre as [Colaborador]," +
                        "c.CUI as [CUI], " +
                        "e.id as [EID], " +
                        "s.id as [SID] " +
                "from " +
                        "Empresa E inner join " +
                        "Sucursal S on E.id = S.ID_Empresa inner join " +
                        "Colaborador C on S.id = c.ID_Sucursal";

            public const string GetEmpresas = 
                "Select " +
                    "e.Nombre as [Nombre], " +
                    "e.Pais as [Pais], " +
                    "e.id as [ID] " +
                "from " +
                    "empresa e";

            public const string GetSucursales = 
                "Select " +
                    "s.Nombre as [Nombre], " +
                    "s.Direccion as [Direccion], " +
                    "s.Telefono as [Telefono], " +
                    "s.ID as [ID]," +
                    "e.id as [EmpresaID]," +
                    "e.Nombre as [EmpresaNombre], " +
                    "e.Pais as [Pais] " +
                "from " +
                    "SUCURSAL s inner join " +
                    "EMPRESA e on s.ID_Empresa = e.ID";

            public static string GetConnectionString (string DBUser, string DBPass)
            {
                return  "Data Source=CARLOVPC\\DASGLOBAL;" +
                        "Initial Catalog=DASGlobalDB;" +
                        $"User ID={DBUser};" +//sa
                        $"Password={DBPass}";//password...
            }
            
            
            public static string GetEmpresaQuery (int id)
            {
                return
                    "select Nombre, Pais, id " +
                    "from Empresa " +
                    $"where id = {id};";
            }
            public static string EditEmpresaQuery(string nombre, string pais, int id)
            {
                return
                    " UPDATE empresa " +
                    $" SET Nombre = '{nombre}', pais = '{pais}' " +
                    $" WHERE ID = {id};";
            }
            public static string DeleteEmpresaQuery(int id)
            {
                return
                    $"DELETE FROM EMPRESA WHERE id={id};"
                    ;
            }
            
            public static string GetSucursalQuery(int id)
            {
                return
                    "select Nombre, Direccion, Telefono, id " +
                    "from Sucursal " +
                    $"where id = {id};";
            }
            public static string EditSucursalQuery(string nombre, string direccion, string telefono, int id)
            {
                return
                    " UPDATE sucursal " +
                    $" SET Nombre = '{nombre}', direccion = '{direccion}', telefono = '{telefono}' " +
                    $" WHERE ID = {id};";
            }
            public static string DeleteSucursalQuery(int id)
            {
                return
                    $"DELETE FROM SUCURSAL WHERE id={id};"
                    ;
            }

            public static string GetColaboradorQuery(string cui)
            {
                return
                    "select Nombre, CUI " +
                    "from COLABORADOR " +
                    $"where Cui = '{cui}' ;";
            }
            public static string EditColaboradorQuery(string nombre, string cui)
            {
                return
                    " UPDATE COLABORADOR " +
                    $" SET Nombre = '{nombre}' " +
                    $" WHERE cui = '{cui}' ;";
            }
            public static string DeleteColaboradorQuery(string cui)
            {
                return
                   $"DELETE FROM COLABORADOR WHERE cui = '{cui}' ;"
                   ;
            }
        
            public static string InsertEmpresaQuery(string nombre, string pais)
            {
                return $"insert into EMPRESA values ('{nombre}','{pais}')";
            }
            public static string InsertSucursalQuery(string nombre, string direccion,string telefono, int id_empresa)
            {
                return $"insert into SUCURSAL values ('{nombre}','{direccion}','{telefono}',{id_empresa})";
            }
            public static string InsertColaboradorQuery(string nombre, string cui, int id_sucursal)
            {
                return $"insert into COLABORADOR values ('{nombre}','{cui}',{id_sucursal})";
            }

            public static string GetEmpresaID(string nombre, string pais)
            {
                return $"select id from EMPRESA where Nombre = '{nombre}' and Pais = '{pais}'";
            }
            public static string GetSucursalID(string nombre, string direccion, string telefono, int id_empresa)
            {
                return $"select id from SUCURSAL where Nombre = '{nombre}' and Direccion = '{direccion}' and Telefono = '{telefono}' and ID_Empresa = {id_empresa} ";
            }
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            UserDB = form["vUserDB"];
            PassDB = form["vPassDB"];
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.CheckLogin, connection))
                    {
                        var result = command.ExecuteScalar();
                    }
                }
                return RedirectToAction("VerBaseDatos");
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View();
            }
        }
        public ActionResult CargarArchivo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CargarArchivo(HttpPostedFileBase file)
        {
            if(file !=null && file.ContentLength > 0)
            {
                string JsonFileContentString;
                ClaseParaArchivo JsonObject;
                int id_empresa;
                int id_sucursal;
                //intentar leer archivo
                try
                {
                    JsonFileContentString = (new StreamReader(file.InputStream)).ReadToEnd();
                    JsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaseParaArchivo>(JsonFileContentString);
                }
                catch (Exception e)
                {
                    ViewBag.MensajeCarga = "Error al subir archivo o formato invalido.";
                    return View();
                }
                //intentar crear empresa
                try
                {
                    using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                    {
                        connection.Open();
                        using (var command = new SqlCommand(Queries.InsertEmpresaQuery(JsonObject.empresa.nombre,JsonObject.empresa.pais), connection))
                        {
                            var result = command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.MensajeCarga = e.Message;
                    return View();
                }
                //obtener id empresa creada
                try
                {
                    using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                    {
                        connection.Open();
                        using (var command = new SqlCommand(Queries.GetEmpresaID(JsonObject.empresa.nombre, JsonObject.empresa.pais), connection))
                        {
                            id_empresa = (int)command.ExecuteScalar();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.MensajeCarga = e.Message;
                    return View();
                }
                //insertar sucursales para la empresa creada
                //insertar colaboradores par cada sucursal
                try
                {
                    using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                    {
                        connection.Open();
                        foreach(SucursalArchivo suc_ in JsonObject.empresa.sucursales)
                        {
                            using (var command = new SqlCommand(Queries.InsertSucursalQuery(suc_.nombre,suc_.direccion,suc_.telefono,id_empresa) , connection))
                            {
                                var result = command.ExecuteNonQuery();
                            }
                            using (var command = new SqlCommand(Queries.GetSucursalID(suc_.nombre, suc_.direccion, suc_.telefono, id_empresa), connection))
                            {
                                id_sucursal = (int)command.ExecuteScalar();
                            }
                            foreach (ColaboradorArchivo col_ in suc_.colaboradores)
                            {
                                using (var command = new SqlCommand(Queries.InsertColaboradorQuery(col_.nombre,col_.CUI,id_sucursal) , connection))
                                {
                                    var result = command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.MensajeCarga = e.Message;
                    return View();
                }



                return RedirectToAction("VerBaseDatos");
            }
            else
            {
                ViewBag.MensajeCarga = "Archivo invalido o vacio.";
                return View();
            }
        }
        public ActionResult VerBaseDatos()
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            var Result = new List<RegistroTablaCompleta>();
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetAllData, connection))
                    {
                        //         DataReader
                        using (var dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Result.Add(
                                    new RegistroTablaCompleta(
                                        dr["Empresa"].ToString(),
                                        dr["Pais"].ToString(),
                                        dr["Sucursal"].ToString(),
                                        dr["SucursalDireccion"].ToString(),
                                        dr["SucursalTelefono"].ToString(),
                                        dr["Colaborador"].ToString(),
                                        dr["CUI"].ToString(),
                                        (int)dr["EID"],
                                        (int)dr["SID"]
                                        ));
                            }
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        public ActionResult VerEmpresas()
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            var Result = new List<Empresa>();
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetEmpresas, connection))
                    {
                        //         DataReader
                        using (var dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Result.Add(
                                    new Empresa(
                                        dr["Nombre"].ToString(),
                                        dr["Pais"].ToString(),
                                        (int)dr["ID"]
                                        )) ;
                            }
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        public ActionResult VerSucursales()
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            var Result = new List<Sucursal>();
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetSucursales, connection))
                    {
                        //         DataReader
                        using (var dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Result.Add(
                                    new Sucursal(
                                        dr["Nombre"].ToString(),
                                        dr["Direccion"].ToString(),
                                        dr["Telefono"].ToString(),
                                        (int)dr["ID"],
                                        (int)dr["EmpresaID"],
                                        dr["EmpresaNombre"].ToString(),
                                        dr["Pais"].ToString()
                                        )) ;
                            }
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult EditEmpresa(int id)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Empresa Result;
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetEmpresaQuery(id), connection))
                    {
                        using (var dr = command.ExecuteReader())
                        {
                            dr.Read();
                            Result = new Empresa(
                                dr["Nombre"].ToString(),
                                dr["Pais"].ToString(),
                                (int)dr["ID"]
                                );
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult EditEmpresa(string nombre, string pais, int id)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.EditEmpresaQuery(nombre,pais,id), connection))
                    {
                        var result = command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("VerEmpresas");
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult EditSucursal(int id)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Sucursal Result;
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetSucursalQuery(id), connection))
                    {
                        using (var dr = command.ExecuteReader())
                        {
                            dr.Read();
                            Result = new Sucursal(
                                dr["Nombre"].ToString(),
                                dr["Direccion"].ToString(),
                                dr["Telefono"].ToString(),
                                (int)dr["ID"]
                                );
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult EditSucursal(string nombre, string direccion, string telefono, int id)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.EditSucursalQuery(nombre,direccion, telefono, id), connection))
                    {
                        var result = command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("VerSucursales");
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult EditColaborador(string cui)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Colaborador Result;
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetColaboradorQuery(cui), connection))
                    {
                        using (var dr = command.ExecuteReader())
                        {
                            dr.Read();
                            Result = new Colaborador(
                                dr["Nombre"].ToString(),
                                dr["Cui"].ToString()
                                );
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult EditColaborador(string nombre, string cui)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.EditColaboradorQuery(nombre,cui), connection))
                    {
                        var result = command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("VerBaseDatos");
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult EliminarEmpresa(int id)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Empresa Result;
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetEmpresaQuery(id), connection))
                    {
                        using (var dr = command.ExecuteReader())
                        {
                            dr.Read();
                            Result = new Empresa(
                                dr["Nombre"].ToString(),
                                dr["Pais"].ToString(),
                                (int)dr["ID"]
                                );
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult EliminarEmpresa(string nombre, string pais, int id)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.DeleteEmpresaQuery(id), connection))
                    {
                        var result = command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("VerEmpresas");
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult EliminarSucursal(int id)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Sucursal Result;
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetSucursalQuery(id), connection))
                    {
                        using (var dr = command.ExecuteReader())
                        {
                            dr.Read();
                            Result = new Sucursal(
                                dr["Nombre"].ToString(),
                                dr["Direccion"].ToString(),
                                dr["Telefono"].ToString(),
                                (int)dr["ID"]
                                );
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult EliminarSucursal(string nombre, string direccion, string telefono, int id)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.DeleteSucursalQuery(id), connection))
                    {
                        var result = command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("VerSucursales");
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult EliminarColaborador(string cui)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Colaborador Result;
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.GetColaboradorQuery(cui), connection))
                    {
                        using (var dr = command.ExecuteReader())
                        {
                            dr.Read();
                            Result = new Colaborador(
                                dr["Nombre"].ToString(),
                                dr["Cui"].ToString()
                                );
                        }
                    }
                }
                return View(Result);
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult EliminarColaborador(string nombre, string cui)
        {
            if (UserDB == null || UserDB == string.Empty || PassDB == null || PassDB == string.Empty)
            {
                return RedirectToAction("Index");
            }
            try
            {
                using (var connection = new SqlConnection(Queries.GetConnectionString(UserDB, PassDB)))
                {
                    connection.Open();
                    using (var command = new SqlCommand(Queries.DeleteColaboradorQuery(cui), connection))
                    {
                        var result = command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("VerBaseDatos");
            }
            catch (Exception e)
            {
                ViewBag.MensajeCredenciales = e.Message;
                return View("Index");
            }
        }
    }
}