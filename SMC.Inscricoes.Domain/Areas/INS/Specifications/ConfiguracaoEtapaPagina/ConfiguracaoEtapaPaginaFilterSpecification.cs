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
    public class ConfiguracaoEtapaPaginaFilterSpecification : SMCSpecification<ConfiguracaoEtapaPagina>
    {
        public long SeqConfiguracaoEtapa { get; set; }

        public string Token { get; set; }

        public override Expression<Func<ConfiguracaoEtapaPagina, bool>> SatisfiedBy()
        {
            return c => c.SeqConfiguracaoEtapa == this.SeqConfiguracaoEtapa
                && (string.IsNullOrEmpty(this.Token) || c.Token.Equals(this.Token));
        }
    }
}
