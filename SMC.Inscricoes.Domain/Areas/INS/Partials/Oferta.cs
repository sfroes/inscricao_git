using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Models
{
    public partial class Oferta
    {
        [NotMapped]
        public bool Vigente
        {
            get
            {
                return new OfertaVigenteSpecification().IsSatisfiedBy(this);
            }
        }

        [NotMapped]
        public bool Cancelada
        {
            get
            {
                return new OfertaCanceladaSpecification().IsSatisfiedBy(this);
            }
        }

        [NotMapped]
        public override bool EOferta
        {
            get
            {
                return true;
            }
        }
    }
}
