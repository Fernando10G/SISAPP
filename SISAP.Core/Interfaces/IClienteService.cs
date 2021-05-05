﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.Core.Entities;

namespace SISAP.Core.Interfaces
{
    public interface IClienteService
    {
        IEnumerable<Cliente> GetAll();

        Cliente ObtenerCliente(string Codigo, string Nombre, string Apellido);
    }
}


