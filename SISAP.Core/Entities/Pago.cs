﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.Core.Entities
{
    public class Pago
    {
        public int PagoId { get; set; }
        public string FechaPago { get; set; }
        public int EstadoPagoId { get; set; }
        public int ClienteId { get; set; }
        public int ConsumoServicioId { get; set; }
        public int LecturaId { get; set; }
    }
}
