using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.GPI.Administrativo.Areas.NOT.Models;
using SMC.GPI.Administrativo.Areas.NOT.Services;
using SMC.GPI.Administrativo.Areas.RES.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Notificacoes.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.NOT
{
    public class ConfigurarNotificacaoController : SMCControllerBase
    {
        #region Services

        private ConfigurarNotificacaoControllerService ConfigurarNotificacaoControllerService
        {
            get
            {
                return this.Create<ConfigurarNotificacaoControllerService>();
            }
        }

        private ConsultaNotificacaoControllerService ConsultaNotificacaoControllerService
        {
            get
            {
                return this.Create<ConsultaNotificacaoControllerService>();
            }
        }

        private ProcessoControllerService ProcessoControllerService
        {
            get
            {
                return this.Create<ProcessoControllerService>();
            }
        }

        private UnidadeResponsavelControllerService UnidadeResponsavelControllerService
        {
            get { return this.Create<UnidadeResponsavelControllerService>(); }
        }

        private TipoNotificacaoControllerService TipoNotificacaoControllerService
        {
            get { return this.Create<TipoNotificacaoControllerService>(); }
        }

        #endregion Services

        #region Listar

        //
        // GET: /NOT/ConfigurarNotificacao/
        [SMCAuthorize(UC_INS_001_01_11.PESQUISAR_CONFIGURACAO_NOTIFICACAO)]
        public ActionResult Index(ConfigurarNotificacaoFiltroViewModel filtro)
        {
            return View(filtro);
        }

        [SMCAuthorize(UC_INS_001_01_11.PESQUISAR_CONFIGURACAO_NOTIFICACAO)]
        public ActionResult ListarConfiguracaoNotificacao(ConfigurarNotificacaoFiltroViewModel filtro)
        {
            SMCPagerModel<ConfigurarNotificacaoListaViewModel> model = ConfigurarNotificacaoControllerService.BuscarConfiguracoesNotificacao(filtro);
            return PartialView("_ListarConfiguracoesNotificacao", model);
        }

        #endregion Listar

        #region Incluir / Editar

        [SMCAuthorize(UC_INS_001_01_12.MANTER_CONFIGURACAO_NOTIFICACAO)]
        public ActionResult Incluir(SMCEncryptedLong seqProcesso)
        {
            try
            {
                ConfigurarNotificacaoControllerService.VerificaPermissaoAlteracao(seqProcesso);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return RedirectToAction("Index", new { seqProcesso = seqProcesso });
            }

            ConfigurarNotificacaoViewModel model = new ConfigurarNotificacaoViewModel();
            model.SeqProcesso = seqProcesso;

            return View(model);
        }

        [SMCAuthorize(UC_INS_001_01_12.MANTER_CONFIGURACAO_NOTIFICACAO)]
        public ActionResult Editar(SMCEncryptedLong seqConfiguracaoNotificacao)
        {
            ConfigurarNotificacaoViewModel model = ConfigurarNotificacaoControllerService.BuscarConfiguracaoNotificacao(seqConfiguracaoNotificacao);
            return View(model);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_12.MANTER_CONFIGURACAO_NOTIFICACAO)]
        public ActionResult Salvar(ConfigurarNotificacaoViewModel model)
        {
            foreach (var item in model.ConfiguracoesEmail)
            {
                if (item.ConfiguracaoNotificacao != null)
                {
                    item.ConfiguracaoNotificacao.Mensagem = SMCHtmlUtils.DecodeHtml(item.ConfiguracaoNotificacao.Mensagem);
                }
            }
            if (Save(model, ConfigurarNotificacaoControllerService.SalvarConfiguracaoNotificacao))
            {
                return WizardRedirect("Index", routeValues: new { seqProcesso = (SMCEncryptedLong)model.SeqProcesso });
            }
            return WizardConfiguracaoEmail(model);
        }

        [SMCAuthorize(UC_INS_001_01_12.MANTER_CONFIGURACAO_NOTIFICACAO)]
        [HttpPost]
        public ActionResult WizardSelecaoTipoNotificacao(ConfigurarNotificacaoViewModel model)
        {
            if (model.TiposNotificacao == null)
                model.TiposNotificacao = ConsultaNotificacaoControllerService.BuscarTiposNotificacao(model.SeqProcesso);

            model.Step = 0;

            return PartialView("_WizardSelecaoTipoNotificacao", model);
        }

        [SMCAuthorize(UC_INS_001_01_12.MANTER_CONFIGURACAO_NOTIFICACAO)]
        [HttpPost]
        public ActionResult WizardConfiguracaoEmail(ConfigurarNotificacaoViewModel model)
        {
            //Verifica se o tipo de notificação foi modificado e se a mudança é valida
            if (model.Seq != 0)
            {
                ConfigurarNotificacaoControllerService.ValidarTipoNotificacao(model);
            }

            // Se ConfiguracoesEmail for null, está criando uma nova configuração e está passando pelo segundo passo pela primeira vez.
            if (model.ConfiguracoesEmail == null)
            {
                model.ConfiguracoesEmail = new List<ConfiguracaoNotificacaoIdiomaViewModel>();
                var processo = ProcessoControllerService.BuscarProcesso(model.SeqProcesso);
                var unidadeResponsavel = UnidadeResponsavelControllerService.BuscarUnidadeResponsavel(processo.SeqUnidadeResponsavel);

                var email = processo.EnderecosEletronicos.Where(f => f.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).FirstOrDefault();
                if (email == null)
                    unidadeResponsavel.EnderecosEletronicos.Where(f => f.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).FirstOrDefault();

                foreach (var idioma in processo.Idiomas)
                {
                    model.Idiomas.Add(idioma.Idioma.ToString());
                    model.ConfiguracoesEmail.Add(new ConfiguracaoNotificacaoIdiomaViewModel()
                    {
                        Idioma = idioma.Idioma,
                        ConfiguracaoNotificacao = new ConfiguracaoNotificacaoEmailViewModel()
                        {
                            SeqTipoNotificacao = model.SeqTipoNotificacao,
                            DataInicioValidade = DateTime.Now,
                            NomeOrigem = unidadeResponsavel.Nome,
                            EmailOrigem = (email != null) ? email.Descricao : string.Empty
                        }
                    });
                }
            }
            else
            {
                // Verifica se o tipo de notificação foi modificado, ao entrar no segundo passo pela segunda vez.
                if (model.OldSeqTipoNotificacao > 0
                        && model.SeqTipoNotificacao != model.OldSeqTipoNotificacao)
                {
                    foreach (var conf in model.ConfiguracoesEmail)
                    {
                        conf.ConfiguracaoNotificacao.SeqTipoNotificacao = model.SeqTipoNotificacao;
                    }
                }
            }

            model.OldSeqTipoNotificacao = model.SeqTipoNotificacao;
            model.Step = 1;

            return PartialView("_WizardConfiguracaoEmail", model);
        }

        [SMCAuthorize(UC_INS_001_01_12.MANTER_CONFIGURACAO_NOTIFICACAO)]
        [HttpPost]
        public ActionResult CarregaObservacoes(long seqTipoNoficicacao)
        {
            var obs = ConfigurarNotificacaoControllerService.BuscarObservacaoTipoNotificacao(seqTipoNoficicacao);
            return Json(obs ?? string.Empty);
        }

        #endregion Incluir / Editar

        #region Excluir

        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_12.MANTER_CONFIGURACAO_NOTIFICACAO)]
        public ActionResult Excluir(SMCEncryptedLong seqConfiguracaoNotificacao, SMCEncryptedLong seqProcesso)
        {
            ConfigurarNotificacaoControllerService.Excluir(seqConfiguracaoNotificacao);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                                            MessagesResource.Entidade_ConfiguracaoNotificacao),
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);
            return RenderAction("ListarConfiguracaoNotificacao", new ConfigurarNotificacaoFiltroViewModel() { SeqProcesso = seqProcesso });
        }

        #endregion Excluir

        #region Configurar Parametros

        [SMCAuthorize(UC_INS_001_01_13.CONFIGURAR_ENVIO_NOTIFICACAO)]
        public ActionResult ParametrosNotificacao(SMCEncryptedLong seqConfiguracaoNotificacao, SMCEncryptedLong seqTipoNotificacao, string tipoNotificacao)
        {
            try
            {
                var modelo = ConfigurarNotificacaoControllerService.BuscarParametrosNotificacao(seqConfiguracaoNotificacao);
                modelo.AtributosDisponiveis = this.TipoNotificacaoControllerService.BuscarAtributosAgendamentoTipo(seqTipoNotificacao);
                modelo.TipoNotificacao = tipoNotificacao;
                return PartialView("_ParametrosNotificacao", modelo);
            }
            catch (Exception ex)
            {
                return ThrowOpenModalException(ex.Message);
            }
        }

        [SMCAuthorize(UC_INS_001_01_13.CONFIGURAR_ENVIO_NOTIFICACAO)]
        public ActionResult SalvarParametros(ParametroNotificacaoViewModel modelo)
        {
            ConfigurarNotificacaoControllerService.SalvarParametrosNotificacao(modelo);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Alteracao_Registro,
                                            MessagesResource.Entidade_ParametroEnvioNotificacao),
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

            return null;
        }

        #endregion Configurar Parametros
    }
}