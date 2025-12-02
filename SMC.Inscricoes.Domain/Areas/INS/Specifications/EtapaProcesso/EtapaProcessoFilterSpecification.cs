using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class EtapaProcessoFilterSpecification : SMCSpecification<EtapaProcesso>
    {
        public EtapaProcessoFilterSpecification(long SeqProcesso)
        {
            this.SeqProcesso = SeqProcesso;
        }

        public long SeqProcesso { get; set; }

        public long? SeqEtapaSGF { get; set; }

        public string Token { get; set; }

        public SituacaoEtapa? SituacaoEtapa { get; set; }

        public override Expression<Func<EtapaProcesso, bool>> SatisfiedBy()
        {
            AddExpression(x => x.SeqProcesso == SeqProcesso);
            AddExpression(SeqEtapaSGF, x => x.SeqEtapaSGF == SeqEtapaSGF);
            AddExpression(Token, x => x.Token == Token);
            AddExpression(SituacaoEtapa, x => x.SituacaoEtapa == SituacaoEtapa);

            return GetExpression();
        }
    }
}
