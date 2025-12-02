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
    public class InscricaoEnvioNotificacaoFilterSpecification : SMCSpecification<InscricaoEnvioNotificacao>
    {
        public long? SeqInscricao { get; set; }

        public List<long> SeqsNotificacoesEmailDestinatario { get; set; }

        public List<long> SeqsParametroEnvioNotificacao { get; set; }

        public long? SeqProcessoConfiguracaoNotificacao { get; set; }

        public override Expression<Func<InscricaoEnvioNotificacao, bool>> SatisfiedBy()
        {
            if (SeqsNotificacoesEmailDestinatario == null)
                this.SeqsNotificacoesEmailDestinatario = new List<long>();

            return i => (!this.SeqInscricao.HasValue || i.SeqInscricao == this.SeqInscricao)
                && (!this.SeqsNotificacoesEmailDestinatario.Any() || this.SeqsNotificacoesEmailDestinatario.Contains(i.SeqNotificacaoEmailDestinatario))
                && (!this.SeqsParametroEnvioNotificacao.Any() || this.SeqsParametroEnvioNotificacao.Contains(i.SeqParametroEnvioNotificacao.Value))
                && (!this.SeqProcessoConfiguracaoNotificacao.HasValue || i.ProcessoConfiguracaoNotificacaoIdioma.SeqProcessoConfiguracaoNotificacao == this.SeqProcessoConfiguracaoNotificacao);
        }
    }
}
