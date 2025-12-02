using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.SEL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.SEL.Specifications
{
    public class InscricaoOfertaHistoricoSituacaoSpecification : SMCSpecification<InscricaoOfertaHistoricoSituacao>
    {
        public long? Seq { get; set; }

        public long? SeqInscricaoOferta { get; set; }

        public bool? Atual { get; set; }

        public override Expression<Func<InscricaoOfertaHistoricoSituacao, bool>> SatisfiedBy()
        {
            AddExpression(this.Seq, a => a.Seq == this.Seq);
            AddExpression(this.SeqInscricaoOferta, a => a.SeqInscricaoOferta == this.SeqInscricaoOferta);
            AddExpression(this.Atual, a => a.Atual == this.Atual);

            return GetExpression();
        }
    }
}
