using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class HierarquiaOfertaFilterSpecification : SMCSpecification<HierarquiaOferta>
    {
        public HierarquiaOfertaFilterSpecification()
        {
            SetOrderBy(o => o.Nome);
        }

        public long? SeqProcesso { get; set; }

        public long? SeqHierarquiaPai { get; set; }

        public bool? ApenasNosPai { get; set; }

        public bool? ProcessoGestaoEvento { get; set; }

        public override Expression<Func<HierarquiaOferta, bool>> SatisfiedBy()
        {
            AddExpression(SeqProcesso, h => h.SeqProcesso == SeqProcesso);
            AddExpression(SeqHierarquiaPai, h => h.SeqPai == SeqHierarquiaPai);
            AddExpression(ApenasNosPai, h => !h.SeqPai.HasValue);
            AddExpression(ProcessoGestaoEvento, a => a.Processo.TipoProcesso.GestaoEventos == ProcessoGestaoEvento);

            return GetExpression();
        }
    }
}
