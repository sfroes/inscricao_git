using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ViewTipoDocumentoFilterSpecification : SMCSpecification<ViewTipoDocumento>
    {
        public long? Seq { get; set; }
        public string Descricao { get; set; }
        public string Token { get; set; }

        public override Expression<Func<ViewTipoDocumento, bool>> SatisfiedBy()
        {
            AddExpression(Seq, a => a.Seq == Seq);
            AddExpression(Descricao, a => a.Descricao == Descricao);
            AddExpression(Token, a => a.Token == Token);

            return GetExpression();
        }
    }
}