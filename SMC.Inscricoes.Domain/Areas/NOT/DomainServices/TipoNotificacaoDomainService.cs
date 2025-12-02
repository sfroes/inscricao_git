using SMC.Inscricoes.Common.Areas.NOT;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Specification;
using System.Linq;
using SMC.Inscricoes.Common.Areas.NOT.Exceptions;
using SMC.Inscricoes.Domain.Areas.NOT.Specifications;
using SMC.Inscricoes.Domain.Areas.NOT.Models;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;

namespace SMC.Inscricoes.Domain.Areas.NOT.DomainServices
{
    public class TipoNotificacaoDomainService : InscricaoContextDomain<TipoNotificacao>
    {
        #region Serviços
        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }
        #endregion

        public long Salvar(TipoNotificacao tipoNotificacao, long oldSeq)
        {
            tipoNotificacao.Token = NotificacaoService.BuscarTipoNotificacao(tipoNotificacao.Seq).Token;
            if (oldSeq != 0)
            {
                var spec = new SMCSeqSpecification<TipoNotificacao>(oldSeq);
                var tipoNotificacaoBanco = this.SearchByKey(spec,
                                                IncludesTipoNotificacao.ProcessosConfiguracaoNotificacao |
                                                IncludesTipoNotificacao.AtributosAgendamento);

                //O Tipo de Notificação não pode ser alterado se já existir registro de configuração de notificação
                if (tipoNotificacaoBanco.ProcessosConfiguracaoNotificacao.Any())
                {
                    if (oldSeq != tipoNotificacao.Seq)
                    {
                        throw new TipoNotificacaoAlterarTipoComConfiguracaoException();
                    }

                    if (tipoNotificacao.PermiteAgendamento != tipoNotificacaoBanco.PermiteAgendamento)
                    {
                        throw new TipoNotificacaoAlterarAgendamentoComConfiguracaoException();
                    }

                    if (!tipoNotificacao.AtributosAgendamento.SMCContainsList(tipoNotificacaoBanco.AtributosAgendamento, p => p.AtributoAgendamento)
                            || tipoNotificacao.AtributosAgendamento.Count != tipoNotificacaoBanco.AtributosAgendamento.Count)
                    {
                        throw new TipoNotificacaoAlterarAtributoComConfiguracaoException();
                    }
                }
                // Atualiza a entidade.
                this.UpdateEntity(tipoNotificacao);
            }
            else
            {             
                if (this.Count(new TipoNotificacaoSpecification() { Token = tipoNotificacao.Token }) > 0)
                {
                    throw new TipoNotificacaoJaExistenteException();
                }
                // Insere a entidade. Como o id da tabela não é identity, chamar SaveEntity gera um bug pois o framework acha que está dando update.
                this.InsertEntity(tipoNotificacao);
            }
            
            return tipoNotificacao.Seq;
        }
    }
}
