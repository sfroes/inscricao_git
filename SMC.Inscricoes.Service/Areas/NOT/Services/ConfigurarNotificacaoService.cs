using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.NOT.DomainServices;
using SMC.Inscricoes.Domain.Areas.NOT.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Data;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.NOT.Services
{
    public class ConfigurarNotificacaoService : SMCServiceBase, IConfigurarNotificacaoService
    {
        #region Services
        private ProcessoConfiguracaoNotificacaoDomainService ProcessoConfiguracaoNotificacaoDomainService
        {
            get { return this.Create<ProcessoConfiguracaoNotificacaoDomainService>(); }
        }

        private ProcessoConfiguracaoNotificacaoIdiomaDomainService ProcessoConfiguracaoNotificacaoIdiomaDomainService
        {
            get { return this.Create<ProcessoConfiguracaoNotificacaoIdiomaDomainService>(); }
        }

        private TipoNotificacaoDomainService TipoNotificacaoDomainService
        {
            get { return this.Create<TipoNotificacaoDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }
        #endregion

        public SMCPagerData<ConfigurarNotificacaoListaData> BuscarConfiguracoesNotificacao(ConfigurarNotificacaoFiltroData filtro)
        {
            var spec = filtro.Transform<ConfiguraraoNotificacaoSpecification>();

            if (!spec.OrderByClauses.Any())
                spec.SetOrderBy(x => x.TipoNotificacao.Descricao);

            int total;
            var data = this.ProcessoConfiguracaoNotificacaoDomainService.SearchProjectionBySpecification(spec,
                x => new ConfigurarNotificacaoListaData
                {
                    Seq = x.Seq,
                    TipoNotificacao = x.TipoNotificacao.Descricao,
                    SeqTipoNotificacao = x.SeqTipoNotificacao,
                    EnvioAutomatico = x.EnvioAutomatico,
                    PermiteAgendamento = x.TipoNotificacao.PermiteAgendamento,
                    SeqProcesso = filtro.SeqProcesso,
                    SeqUnidadeResponsavel = x.Processo.SeqUnidadeResponsavel
                }, out total);
            return new SMCPagerData<ConfigurarNotificacaoListaData>(data, total);
        }

        public ConfigurarNotificacaoData BuscarConfiguracaoNotificacao(long seqConfiguracaoNotificacao)
        {
            return this.ProcessoConfiguracaoNotificacaoDomainService.
                BuscarConfiguracaoNotificacao(seqConfiguracaoNotificacao).Transform<ConfigurarNotificacaoData>();
        }

        public long SalvarConfiguracaoNotificacao(ConfigurarNotificacaoData configurarNotificacaoData)
        {
            var vo = configurarNotificacaoData.Transform<ConfigurarNotificacaoVO>();
            vo.ValidaTags = true;
            return this.ProcessoConfiguracaoNotificacaoDomainService.SalvarConfiguracaoNotificacao(vo);
        }

        /// <summary>
        /// Verifica se o processo permite a inclusão/edição de configurações de notificação.
        /// </summary>
        public void VerificaPermissaoAlteracao(long seqProcesso)
        {
            this.ProcessoConfiguracaoNotificacaoDomainService.VerificaPermissaoAlteracao(seqProcesso);
        }

        public void ValidarTipoNotificacao(ConfigurarNotificacaoData configurarNotificacaoData)
        {
            this.ProcessoConfiguracaoNotificacaoDomainService.ValidarTipoNotificacao(
                configurarNotificacaoData.Transform<ConfigurarNotificacaoVO>());
        }

        public void Excluir(long seqConfiguracaoNotificacao)
        {
            ProcessoConfiguracaoNotificacaoDomainService.Excluir(seqConfiguracaoNotificacao);
        }

        public ProcessoConfiguracaoNotificacaoData BuscarParametrosNotificacao(long seqConfiguracaoNotificacao)
        {
            return ProcessoConfiguracaoNotificacaoDomainService.SearchByKey<ProcessoConfiguracaoNotificacao, ProcessoConfiguracaoNotificacaoData>(seqConfiguracaoNotificacao,
                                                IncludesProcessoConfiguracaoNotificacao.ParametrosEnvioNotificacao);
        }

        public void SalvarParametrosNotificacao(ProcessoConfiguracaoNotificacaoData processoConfiguracaoNotificacaoData)
        {
            ProcessoConfiguracaoNotificacaoDomainService.SalvarParametrosNotificacao(processoConfiguracaoNotificacaoData.Transform<ProcessoConfiguracaoNotificacao>());
        }
    }
}
