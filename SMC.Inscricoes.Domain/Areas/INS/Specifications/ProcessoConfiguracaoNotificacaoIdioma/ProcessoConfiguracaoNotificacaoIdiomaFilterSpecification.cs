using SMC.Framework;
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
    public class ProcessoConfiguracaoNotificacaoIdiomaFilterSpecification : SMCSpecification<ProcessoConfiguracaoNotificacaoIdioma>
    {
        public long SeqProcesso { get; set; }

        public SMCLanguage Idioma { get; set; }

        public string TokenTipoNotificacao { get; set; }

        public override Expression<Func<ProcessoConfiguracaoNotificacaoIdioma, bool>> SatisfiedBy()
        {
            return p => p.ProcessoConfiguracaoNotificacao.SeqProcesso == this.SeqProcesso &&
                        p.ProcessoIdioma.Idioma == this.Idioma &&
                        p.ProcessoConfiguracaoNotificacao.TipoNotificacao.Token.Equals(this.TokenTipoNotificacao);
        }        
    }
}
