using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DASGlobal.Models
{
    public class ClaseParaArchivo
    {
        public EmpresaArchivo empresa { get; set; }
    }

    public class EmpresaArchivo
    {
        public string nombre { get; set; }
        public string pais { get; set; }
        public SucursalArchivo[] sucursales {get; set; }
    }
    public class SucursalArchivo
    {
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public ColaboradorArchivo[] colaboradores { get; set; }
    }
    public class ColaboradorArchivo
    {
        public string nombre { get; set; }
        public string CUI { get; set; }
    }
}