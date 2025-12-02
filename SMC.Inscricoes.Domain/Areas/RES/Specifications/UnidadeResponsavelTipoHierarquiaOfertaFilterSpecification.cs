using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.RES.Specifications
{
    public class UnidadeResponsavelTipoHierarquiaOfertaFilterSpecification : SMCSpecification<UnidadeResponsavelTipoHierarquiaOferta>
    {
        public long? SeqUnidadeResponsavelTipoProcesso { get; set; }

        public long? SeqTipoHierarquiaOferta { get; set; }

        public override Expression<Func<UnidadeResponsavelTipoHierarquiaOferta, bool>> SatisfiedBy()
        {
            AddExpression(SeqUnidadeResponsavelTipoProcesso, u => u.SeqUnidadeResponsavelTipoProcesso == SeqUnidadeResponsavelTipoProcesso);
            AddExpression(SeqTipoHierarquiaOferta, u => u.SeqTipoHierarquiaOferta == SeqTipoHierarquiaOferta);

            return GetExpression();
        }
    }
}