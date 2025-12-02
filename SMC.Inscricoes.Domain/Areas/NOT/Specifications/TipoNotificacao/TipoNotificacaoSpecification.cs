using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.NOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.NOT.Specifications
{
    public class TipoNotificacaoSpecification : SMCSpecification<TipoNotificacao>
    {
        public string Token { get; set; }

        public override System.Linq.Expressions.Expression<Func<TipoNotificacao, bool>> SatisfiedBy()
        {
            return x => (string.IsNullOrEmpty(Token) || x.Token == Token);
        }
    }
}
