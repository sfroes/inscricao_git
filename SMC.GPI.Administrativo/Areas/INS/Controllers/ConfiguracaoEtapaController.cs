using SMC.Framework;
using SMC.Framework.Exceptions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.UI.Mvc.Util;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.GPI.Administrativo.Services;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Exceptions;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class ConfiguracaoEtapaController : SMCControllerBase
    {
        #region Serviços

        private ConfiguracaoEtapaControllerService ConfiguracaoEtapaControllerService
        {
            get
            {
                return this.Create<ConfiguracaoEtapaControllerService>();
            }
        }

        private AcompanhamentoProcessoControllerService AcompanhamentoProcessoControllerService
        {
            get
            {
                return this.Create<AcompanhamentoProcessoControllerService>();
            }
        }

        private GrupoOfertaControllerService GrupoOfertaControllerService
        {
            get
            {
                return this.Create<GrupoOfertaControllerService>();
            }
        }

        private ProcessoControllerService ProcessoControllerService
        {
            get
            {
                return this.Create<ProcessoControllerService>();
            }
        }

        private EtapaProcessoControllerService EtapaProcessoControllerService
        {
            get
            {
                return this.Create<EtapaProcessoControllerService>();
            }
        }

        private ArquivoControllerService ArquivoControllerService
        {
            get
            {
                return this.Create<ArquivoControllerService>();
            }
        }

        #endregion Serviços

        #region Pesquisar

        /// <summary>
        /// Exibir as configurações de etapa para um determinado processo
        /// </summary>
        /// <param name="seqEtapa">Sequencial da etapa</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [SMCAuthorize(UC_INS_001_03_03.PESQUISAR_CONFIGURACAO_ETAPA)]
        public ActionResult Index(SMCEncryptedLong seqEtapaProcesso, SMCEncryptedLong seqProcesso)
        {
            var modelo = new ConfiguracaoEtapaFiltroViewModel();

            modelo.SeqProcesso = seqProcesso;
            modelo.SeqEtapaProcesso = seqEtapaProcesso;

            return View(modelo);
        }

        /// <summary>
        /// Action para listar as configurações de etapas associadas ao processo
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_03_03.PESQUISAR_CONFIGURACAO_ETAPA)]
        public ActionResult ListarConfiguracaoEtapa(ConfiguracaoEtapaFiltroViewModel filtros)
        {
            SMCPagerModel<ConfiguracaoEtapaListaViewModel> pager = this.ConfiguracaoEtapaControllerService.BuscarConfiguracoesEtapa(filtros);
            return PartialView("_ListarConfiguracaoEtapa", pager);
        }

        #endregion Pesquisar

        #region Incluir/Alterar

        /// <summary>
        /// Incluir uma configuração de etapa associada a um processo
        /// </summary>
        /// <param name="seqEtapa">Sequencial da etapa</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [SMCAuthorize(UC_INS_001_03_04.MANTER_CONFIGURACAO_ETAPA)]
        public ActionResult Incluir(SMCEncryptedLong seqEtapa, SMCEncryptedLong seqProcesso)
        {
            var modelo = new ConfiguracaoEtapaViewModel();

            modelo.SeqEtapaProcesso = seqEtapa;
            modelo.SeqProcesso = seqProcesso;
            modelo.GruposOfertaSelect = this.GrupoOfertaControllerService.BuscarGruposOfertasSelect(seqProcesso);
            return View(modelo);
        }

        /// <summary>
        /// Editar uma configuração de etapa associada a um processo
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_04.MANTER_CONFIGURACAO_ETAPA)]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            var modelo = this.ConfiguracaoEtapaControllerService.BuscarConfiguracaoEtapa(seq);
            modelo.GruposOfertaSelect = this.GrupoOfertaControllerService.BuscarGruposOfertasSelect(modelo.SeqProcesso);
            return View(modelo);
        }

        /// <summary>
        /// Salvar uma configuração de etapa vinculada a um processo
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_04.MANTER_CONFIGURACAO_ETAPA)]
        public ActionResult Salvar(ConfiguracaoEtapaViewModel modelo)
        {
            //if (modelo.ArquivoImagem != null && modelo.ArquivoImagem.State == SMCUploadFileState.Changed)
            //{
            //    modelo.ArquivoImagem.FileData = SMCUploadHelper.GetFileData(modelo.ArquivoImagem);
            //}
            modelo.DescricaoEntregaDocumentacao = HttpUtility.HtmlDecode(modelo.DescricaoEntregaDocumentacao);
            return this.SaveEdit(modelo, ConfiguracaoEtapaControllerService.SalvarConfiguracaoEtapa);
        }

        /// <summary>
        /// Salvar uma configuração de etapa e direcionar o usuário para a tela de inclusão de um novo registro
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_04.MANTER_CONFIGURACAO_ETAPA)]
        public ActionResult SalvarNovo(ConfiguracaoEtapaViewModel modelo)
        {
            modelo.DescricaoEntregaDocumentacao = HttpUtility.HtmlDecode(modelo.DescricaoEntregaDocumentacao);
            return this.SaveNew(modelo, ConfiguracaoEtapaControllerService.SalvarConfiguracaoEtapa,
                routeValues: new
                {
                    @seqEtapa = new SMCEncryptedLong(modelo.SeqEtapaProcesso),
                    @seqProcesso = new SMCEncryptedLong(modelo.SeqProcesso)
                });
        }

        /// <summary>
        /// Salvar uma configuração de etapa e direcionar o usuário para a tela de listagem
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_04.MANTER_CONFIGURACAO_ETAPA)]
        public ActionResult SalvarSair(ConfiguracaoEtapaViewModel modelo)
        {
            modelo.DescricaoEntregaDocumentacao = HttpUtility.HtmlDecode(modelo.DescricaoEntregaDocumentacao);
            return this.SaveQuit(modelo, ConfiguracaoEtapaControllerService.SalvarConfiguracaoEtapa, routeValues: new
            {
                @seqEtapaProcesso = new SMCEncryptedLong(modelo.SeqEtapaProcesso),
                @seqProcesso = new SMCEncryptedLong(modelo.SeqProcesso)
            });
        }

        #endregion Incluir/Alterar

        #region Excluir

        /// <summary>
        /// Exclui uma configuração de etapa do processo
        /// </summary>
        /// <param name="seqEtapa">Sequencial da configuração etapa a ser excluida</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_04.MANTER_CONFIGURACAO_ETAPA)]
        public ActionResult Excluir(SMCEncryptedLong seqConfiguracaoEtapa,
            SMCEncryptedLong seqEtapaProcesso, SMCEncryptedLong seqProcesso)
        {
            this.ConfiguracaoEtapaControllerService.ExcluirConfiguracaoEtapa(seqConfiguracaoEtapa);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                MessagesResource.Entidade_ConfiguracaoEtapa),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return ListarConfiguracaoEtapa(new ConfiguracaoEtapaFiltroViewModel { SeqEtapaProcesso = seqEtapaProcesso, SeqProcesso = seqProcesso });
        }

        #endregion Excluir

        #region Cabecalhos

        [SMCAllowAnonymous]
        public ActionResult CabecalhoProcessoEtapaConfiguracao(SMCEncryptedLong seqEtapa,
                                                       SMCEncryptedLong seqProcesso,
                                                       SMCEncryptedLong seqConfiguracaoEtapa,
                                                       EtapaProcessoActionsEnum action = EtapaProcessoActionsEnum.Nenhum,
                                                       bool exibirBotaoConfiguracaoEtapaProcesso = false)
        {
            CabecalhoProcessoEtapaConfiguracaoViewModel modelo = this.ConfiguracaoEtapaControllerService.BuscarCabecalhoProcessoEtapaConfiguracao(seqConfiguracaoEtapa);

            modelo.SeqProcesso = seqProcesso;
            modelo.SeqEtapaProcesso = seqEtapa;
            modelo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            modelo.ExibirBotaoConfiguracaoEtapaProcesso = exibirBotaoConfiguracaoEtapaProcesso;
            modelo.Action = action;

            return PartialView("_CabecalhoProcessoEtapaConfiguracao", modelo);
        }

        [SMCAllowAnonymous]
        [HttpPost]
        public JsonResult BuscarDescricaoConfiguracao(long? seqConfiguracaoEtapa)
        {
            if (!seqConfiguracaoEtapa.HasValue)
            {
                return Json(string.Empty);
            }

            var descricao = this.ConfiguracaoEtapaControllerService.BuscarNomeConfiguracaoEtapa(seqConfiguracaoEtapa.Value);
            return Json(descricao);
        }

        [SMCAllowAnonymous]
        [HttpPost]
        public JsonResult BuscarTemplateProcesso(long? ProcessoDestino)
        {
            if (ProcessoDestino.HasValue)
            {
                var seq = ProcessoControllerService.BuscarSeqTipoTemplateProcesso(ProcessoDestino.Value);
                return Json(seq);
            }
            return Json(0);
        }

        [SMCAllowAnonymous]
        [HttpPost]
        public JsonResult VerificarIdiomasDestino(long? ProcessoDestino, long seqProcessoOrigem)
        {
            if (ProcessoDestino.HasValue)
            {
                var comp = ProcessoControllerService.CompararIdiomasProcesso(ProcessoDestino.Value, seqProcessoOrigem);
                return Json(comp);
            }
            return Json(0);
        }

        #endregion Cabecalhos

        [SMCAllowAnonymous]
        public ActionResult Download(string guidFile, string name, string type)
        {
            try
            {
                var enctyptedLong = new SMCEncryptedLong(guidFile);
                var arq = AcompanhamentoProcessoControllerService.BuscarArquivo(enctyptedLong);

                if (arq.FileData == null)
                    return File(CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF, "application/pdf", arq.Name + "_expurgo.pdf");

                return File(arq.FileData, arq.Type, arq.Name);
            }
            catch (SMCEncryptionException)
            {
                var data = SMCUploadHelper.GetFileData(new SMCUploadFile { GuidFile = guidFile });
                return File(data, type, name);
            }
        }
    }
}