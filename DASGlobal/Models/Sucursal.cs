using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DASGlobal.Models
{
    public class Sucursal
    {
       public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int ID { get; }
        public int EmpresaID { get; }
        public string EmpresaNombre { get; }
        public string Pais { get; }
        public Sucursal(string nombre, string direccion, string telefono, int iD, int iD_empresa, string nombreEmpresa, string pais)
        {
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            ID = iD;
            EmpresaID = iD_empresa;
            EmpresaNombre = nombreEmpresa;
            Pais = pais;
        }

        public Sucursal(string nombre, string direccion, string telefono, int iD)
        {
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            ID = iD;
        }
    }
}