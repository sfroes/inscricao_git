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
    public class CodigoAutorizacaoFilterSpecification : SMCSpecification<CodigoAutorizacao>
    {

        public long SeqUnidadeResponsavel { get; set; }


        public override Expression<Func<CodigoAutorizacao, bool>> SatisfiedBy()
        {
            return c => c.SeqUnidadeResponsavel == SeqUnidadeResponsavel;
        }
    }
}
