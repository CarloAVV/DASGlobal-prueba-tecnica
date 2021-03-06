using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DASGlobal.Models
{
    public class Empresa
    {
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public int ID { get; }

        public Empresa(string nombre, string pais, int iD)
        {
            Nombre = nombre;
            Pais = pais;
            ID = iD;
        }
    }
}