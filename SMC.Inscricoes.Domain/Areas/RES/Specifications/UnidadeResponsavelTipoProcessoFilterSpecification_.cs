using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.RES.Specifications
{
    public class UnidadeResponsavelTipoProcessoFilterSpecification : SMCSpecification<UnidadeResponsavelTipoProcesso>
    {
        public UnidadeResponsavelTipoProcessoFilterSpecification()
        {
            SetOrderBy(x => x.TipoProcesso.Descricao);
        }

        public long? SeqUnidadeResponsavel {get;set;}

        public long? SeqTipoProcesso { get; set; }

        public long? SeqUnidadeResponsavelTipoProcessoIdVisual { get; set; }

      
        public override Expression<Func<UnidadeResponsavelTipoProcesso, bool>> SatisfiedBy()
        {
            AddExpression(SeqUnidadeResponsavel, u => u.SeqUnidadeResponsavel == SeqUnidadeResponsavel);
            AddExpression(SeqTipoProcesso, u => u.SeqTipoProcesso == SeqTipoProcesso);
            AddExpression(SeqUnidadeResponsavelTipoProcessoIdVisual, u => u.IdentidadesVisuais.Any(a => a.Seq == SeqUnidadeResponsavelTipoProcessoIdVisual.Value));

            return GetExpression();
        }
    }
}
