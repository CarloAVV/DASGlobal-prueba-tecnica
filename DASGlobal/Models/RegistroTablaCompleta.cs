using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DASGlobal.Models
{
    public class RegistroTablaCompleta
    {
        public string Empresa { get; }
        public string Pais { get; }
        public string Sucursal { get; }
        public string SucursalDireccion { get; }
        public string SucursalTelefono { get; }
        public string Colaborador { get; }
        public string ColaboradorCUI { get; }

        public int EmpresaID { get; }
        public int SucursalID { get; }

        public RegistroTablaCompleta(string empresa, string pais, string sucursal, string sucursalDireccion, string sucursalTelefono, string colaborador, string colaboradorCUI, int empresaID, int sucursalID)
        {
            Empresa = empresa;
            Pais = pais;
            Sucursal = sucursal;
            SucursalDireccion = sucursalDireccion;
            SucursalTelefono = sucursalTelefono;
            Colaborador = colaborador;
            ColaboradorCUI = colaboradorCUI;
            EmpresaID = empresaID;
            SucursalID = sucursalID;
        }
    }
}