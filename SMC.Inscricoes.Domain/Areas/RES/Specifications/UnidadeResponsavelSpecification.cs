using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.RES;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.RES.Specifications
{
    public class UnidadeResponsavelSpecification : SMCSpecification<UnidadeResponsavel>
    {
        public long SeqUnidadeResponsavel { get; set; }

        public override Expression<Func<UnidadeResponsavel, bool>> SatisfiedBy()
        {
            return u => u.Seq == SeqUnidadeResponsavel;
        }
    }
}
