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
    public class InscricaoOfertaHistoricoSituacaoPorProcessoSpecification : SMCSpecification<InscricaoOfertaHistoricoSituacao>
    {
        public long SeqProcesso { get; set; }

        public override Expression<Func<InscricaoOfertaHistoricoSituacao, bool>> SatisfiedBy()
        {
            AddExpression(x => x.InscricaoOferta.Inscricao.SeqProcesso == SeqProcesso);
            return GetExpression();
        }
    }
}
