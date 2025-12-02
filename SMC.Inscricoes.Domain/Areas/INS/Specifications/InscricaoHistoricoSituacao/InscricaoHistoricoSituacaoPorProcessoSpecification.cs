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
    public class InscricaoHistoricoSituacaoPorProcessoSpecification : SMCSpecification<InscricaoHistoricoSituacao>
    {
        public long SeqProcesso { get; set; }
        public bool? Atual { get; set; }

        public override Expression<Func<InscricaoHistoricoSituacao, bool>> SatisfiedBy()
        {
            AddExpression(i => i.Inscricao.SeqProcesso == SeqProcesso);
            AddExpression(Atual, i => i.Atual == Atual);
            return GetExpression();
        }
    }
}
