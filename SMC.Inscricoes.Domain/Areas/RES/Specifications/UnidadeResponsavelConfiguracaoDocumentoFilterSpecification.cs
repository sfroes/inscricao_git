using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.RES.Specifications
{
    public class UnidadeResponsavelConfiguracaoDocumentoFilterSpecification : SMCSpecification<UnidadeResponsavel>
    {
        public string TokenSistemaOrigemGad { get; set; }
        public long? Seq { get; set; }

        public override Expression<Func<UnidadeResponsavel, bool>> SatisfiedBy()
        {
            AddExpression(TokenSistemaOrigemGad, u => u.TokenSistemaOrigemGad == TokenSistemaOrigemGad);
            AddExpression(Seq, u => u.Seq == Seq);

            return GetExpression();
        }
    }
}
