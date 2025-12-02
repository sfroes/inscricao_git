using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Models
{
    public partial class Processo
    {
        [NotMapped]
        public bool Cancelado 
        {
            get
            {
                return new ProcessoCanceladoSpecification().IsSatisfiedBy(this);
            }
        }
    }
}
