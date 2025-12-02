using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ViewEventoSaeFilterSpecification : SMCSpecification<ViewEventoSae>
    {

        public int? CodigoUnidadePromotora { get; set; }
        public int? Ano { get; set; }

        public override Expression<Func<ViewEventoSae, bool>> SatisfiedBy()
        {
            AddExpression(Ano, a => a.Ano == Ano);
            AddExpression(CodigoUnidadePromotora, a => a.CodigoUnidadePromotora == CodigoUnidadePromotora);

            return GetExpression();
        }
    }
}
