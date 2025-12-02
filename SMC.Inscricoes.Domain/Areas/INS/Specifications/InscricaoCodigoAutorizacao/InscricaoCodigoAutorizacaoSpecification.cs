using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoCodigoAutorizacaoSpecification : SMCSpecification<InscricaoCodigoAutorizacao>
    {
        public long? SeqOferta { get; set; }

        public override System.Linq.Expressions.Expression<Func<InscricaoCodigoAutorizacao, bool>> SatisfiedBy()
        {
            return x => (!SeqOferta.HasValue || x.Inscricao.Ofertas.Any(o => o.SeqOferta == SeqOferta.Value));
        }
    }
}
