using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SuperDigital.ContaCorrente.API.ViewModels
{
    public class ContaCorrenteViewModel
    {
        public Guid Id { get; set; }
        public int? CodigoAgencia { get; set; }
        public int NumeroConta { get; set; }
        public decimal? Saldo { get; set; }

    }
}
