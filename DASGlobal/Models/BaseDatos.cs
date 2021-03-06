using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DASGlobal.Models
{
    public class BaseDatos
    {
        List<Empresa> Empresas { get; }
        List<Sucursal> Sucursales { get; }
        List<Colaborador> Colaboradores { get; }
        List<RegistroTablaCompleta>TablaCompleta { get; }
    }
}