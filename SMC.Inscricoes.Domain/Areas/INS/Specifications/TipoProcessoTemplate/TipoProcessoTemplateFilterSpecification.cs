using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class TipoProcessoTemplateFilterSpecification : SMCSpecification<TipoProcessoTemplate>
    {
        public long? SeqTipoProcesso { get; set; }

        public long? SeqTemplateProcessoSGF { get; set; }

        public override Expression<Func<TipoProcessoTemplate, bool>> SatisfiedBy()
        {
            AddExpression(SeqTipoProcesso, x => x.SeqTipoProcesso == SeqTipoProcesso);
            AddExpression(SeqTemplateProcessoSGF, x => x.SeqTemplateProcessoSGF == SeqTemplateProcessoSGF);

            return GetExpression();
        }
    }
}