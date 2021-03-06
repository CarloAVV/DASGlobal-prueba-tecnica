using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DASGlobal.Models
{
    public class Colaborador
    {
        public string Nombre { get; set; }
        public string CUI { get; }
        public string ID_sucursal { get; }

        public Colaborador(string nombre, string cUI, string iD_sucursal)
        {
            Nombre = nombre;
            CUI = cUI;
            ID_sucursal = iD_sucursal;
        }
        public Colaborador(string nombre, string cui)
        {
            Nombre = nombre;
            CUI = cui;
        }
    }
}