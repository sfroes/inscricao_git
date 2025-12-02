using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{

    public class GrupoTaxaItemFilterSpecification : SMCSpecification<GrupoTaxaItem>
    {
        public GrupoTaxaItemFilterSpecification()
        {
            SetOrderBy(x => x.Taxa.TipoTaxa.Descricao);
        }

        public long? Seq { get; set; }

        public long? SeqGrupoTaxa { get; set; }

        public long? SeqTaxa { get; set; }

        public override Expression<Func<GrupoTaxaItem, bool>> SatisfiedBy()
        {
            AddExpression(Seq, gti => gti.Seq == this.Seq.Value);
            AddExpression(SeqGrupoTaxa, gti => gti.SeqGrupoTaxa == this.SeqGrupoTaxa.Value);
            AddExpression(SeqTaxa, gti => gti.SeqTaxa == this.SeqTaxa.Value);

            return GetExpression();
        }
    }
}
