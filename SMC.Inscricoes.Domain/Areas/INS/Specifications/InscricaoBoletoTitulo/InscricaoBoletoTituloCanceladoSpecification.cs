using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoBoletoTituloCanceladoSpecification : SMCSpecification<InscricaoBoletoTitulo>
    {
        public override Expression<Func<InscricaoBoletoTitulo, bool>> SatisfiedBy()
        {
            return p => p.DataCancelamento.HasValue && p.DataCancelamento.Value <= DateTime.Now;
        }
    }
}
