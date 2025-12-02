using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.NOT.Models;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Data;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;

namespace SMC.GPI.Administrativo.Areas.NOT.Services
{
    public class ConfigurarNotificacaoControllerService : SMCControllerServiceBase
    {
        #region Services
        private IConfigurarNotificacaoService ConfigurarNotificacaoService
        {
            get { return this.Create<IConfigurarNotificacaoService>(); }
        }

        private Inscricoes.ServiceContract.Areas.NOT.Interfaces.ITipoNotificacaoService TipoNotificacaoService
        {
            get { return this.Create<Inscricoes.ServiceContract.Areas.NOT.Interfaces.ITipoNotificacaoService>(); }
        }

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }
        #endregion

        public SMCPagerModel<ConfigurarNotificacaoListaViewModel> BuscarConfiguracoesNotificacao(ConfigurarNotificacaoFiltroViewModel filtro)
        {
            var pagerData = this.ConfigurarNotificacaoService.BuscarConfiguracoesNotificacao(filtro.Transform<ConfigurarNotificacaoFiltroData>())
                .Transform<SMCPagerData<ConfigurarNotificacaoListaViewModel>>();
            var pagerModel = new SMCPagerModel<ConfigurarNotificacaoListaViewModel>(pagerData, filtro.PageSettings, filtro);
            return pagerModel;
        }

        public ConfigurarNotificacaoViewModel BuscarConfiguracaoNotificacao(long seqConfiguracaoNotificacao)
        {
            return ConfigurarNotificacaoService.BuscarConfiguracaoNotificacao(seqConfiguracaoNotificacao)
                                                    .Transform<ConfigurarNotificacaoViewModel>();
        }

        public long SalvarConfiguracaoNotificacao(ConfigurarNotificacaoViewModel model)
        {
            return ConfigurarNotificacaoService.SalvarConfiguracaoNotificacao(model.Transform<ConfigurarNotificacaoData>());
        }        

        public void VerificaPermissaoAlteracao(long seqProcesso)
        {
            ConfigurarNotificacaoService.VerificaPermissaoAlteracao(seqProcesso);
        }

        public void ValidarTipoNotificacao(ConfigurarNotificacaoViewModel model)
        {
            ConfigurarNotificacaoService.ValidarTipoNotificacao(model.Transform<ConfigurarNotificacaoData>());
        }

        public void Excluir(long seqConfiguracaoNotificacao)
        {
            ConfigurarNotificacaoService.Excluir(seqConfiguracaoNotificacao);
        }

        public ParametroNotificacaoViewModel BuscarParametrosNotificacao(long seqConfiguracaoNotificacao)
        {
            var notificacao = BuscarConfiguracaoNotificacao(seqConfiguracaoNotificacao);
            var tipoNotificacao = TipoNotificacaoService.BuscarTipoNotificacao(notificacao.SeqTipoNotificacao);

            if (!tipoNotificacao.PermiteAgendamento)
                throw new SMCApplicationException(Views.ConfigurarNotificacao.App_LocalResources.UIResource._ParametrosNotificacao_Pagina_Erro);

            return ConfigurarNotificacaoService.BuscarParametrosNotificacao(seqConfiguracaoNotificacao).Transform<ParametroNotificacaoViewModel>();
        }

        public void SalvarParametrosNotificacao(ParametroNotificacaoViewModel modelo)
        {
            ConfigurarNotificacaoService.SalvarParametrosNotificacao(modelo.Transform<ProcessoConfiguracaoNotificacaoData>());
        }        

        public string BuscarObservacaoTipoNotificacao(long seqTipoNoficicacao)
        {
            var tipoNotificacao = NotificacaoService.BuscarTipoNotificacao(seqTipoNoficicacao);
            return tipoNotificacao.Observacao;
        }
    }
}