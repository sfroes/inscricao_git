using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class TipoProcessoSituacaoFilterSpecification : SMCSpecification<TipoProcessoSituacao>
    {
        public long? SeqTipoProcesso { get; set; }

        public long? SeqProcesso { get; set; }

        public string Token { get; set; }

        public string[] Tokens { get; set; }

        public long? SeqSituacaoSGF { get; set; }

        public long[] SeqsSituacoSGF { get; set; }

        public override Expression<Func<TipoProcessoSituacao, bool>> SatisfiedBy()
        {
            AddExpression(SeqTipoProcesso, t => t.SeqTipoProcesso == SeqTipoProcesso);
            AddExpression(SeqProcesso, t => t.TipoProcesso.Processos.Any(f => f.Seq == SeqProcesso));
            AddExpression(Token, t => t.Token.Equals(Token));
            AddExpression(Tokens, t => Tokens.Contains(t.Token));
            AddExpression(SeqSituacaoSGF, t => t.SeqSituacao == SeqSituacaoSGF);
            AddExpression(SeqsSituacoSGF, t => SeqsSituacoSGF.Contains(t.SeqSituacao));

            return GetExpression();
        }
    }
}