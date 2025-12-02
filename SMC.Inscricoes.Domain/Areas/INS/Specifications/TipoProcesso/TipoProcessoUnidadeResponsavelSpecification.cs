using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class TipoProcessoUnidadeResponsavelSpecification : SMCSpecification<TipoProcesso>
    {
        public long? SeqUnidadeResponsavel { get; set; }

        public override Expression<Func<TipoProcesso, bool>> SatisfiedBy()
        {
            return x => x.UnidadeResponsavelTipoProcesso.Any(f => f.SeqUnidadeResponsavel == SeqUnidadeResponsavel);
        }
    }
}
