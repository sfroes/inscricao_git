using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Models
{
    public partial class InscricaoBoletoTitulo
    {
        [NotMapped]
        public bool Cancelado 
        {
            get
            {
                return new InscricaoBoletoTituloCanceladoSpecification().IsSatisfiedBy(this);
            }
            set
            {

            }
        }

        [NotMapped]
        public bool Vencido
        {
            get
            {
                return new InscricaoBoletoTituloVencidoSpecification().IsSatisfiedBy(this);
            }
            set
            {

            }
        }

        [NotMapped]
        public bool Pago
        {
            get
            {
                return new InscricaoBoletoTituloPagoSpecification().IsSatisfiedBy(this);
            }
            set
            {

            }
        }

    }
}
