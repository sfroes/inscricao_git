using Microsoft.ReportingServices.Interfaces;
using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.UI.Mvc.Util;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.GPI.Administrativo.Areas.RES.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class ConfiguracaoEtapaPaginaController : SMCControllerBase
    {
        #region Serviços

        private ConfiguracaoEtapaPaginaControllerService ConfiguracaoEtapaPaginaControllerService
        {
            get
            {
                return this.Create<ConfiguracaoEtapaPaginaControllerService>();
            }
        }

        private GrupoOfertaControllerService GrupoOfertaControllerService
        {
            get
            {
                return this.Create<GrupoOfertaControllerService>();
            }
        }

        private EtapaProcessoControllerService EtapaProcessoControllerService
        {
            get
            {
                return this.Create<EtapaProcessoControllerService>();
            }
        }

        private UnidadeResponsavelControllerService UnidadeResponsavelControllerService
        {
            get
            {
                return this.Create<UnidadeResponsavelControllerService>();
            }
        }

        #endregion Serviços

        #region Arvore

        /// <summary>
        /// Exibir as configurações de página da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        [SMCAuthorize(UC_INS_001_03_06.CONFIGURAR_PAGINAS_ETAPA)]
        public ActionResult Index(SMCEncryptedLong seqConfiguracaoEtapa, SMCEncryptedLong seqEtapa, SMCEncryptedLong seqProcesso)
        {
            var modelo = new ConfiguracaoPaginaEtapaViewModel();

            modelo.SeqEtapa = seqEtapa;
            modelo.SeqProcesso = seqProcesso;
            modelo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            modelo.PossuiPaginas = this.ConfiguracaoEtapaPaginaControllerService
                .VerificarConfiguracaoPossuiPaginas(seqConfiguracaoEtapa);
            modelo.Cabecalho = this.EtapaProcessoControllerService.BuscarCabecalhoProcessoEtapa(seqEtapa);

            return View(modelo);
        }

        /// <summary>
        /// Montar a árvode de configuração das páginas da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        [SMCAuthorize(UC_INS_001_03_06.CONFIGURAR_PAGINAS_ETAPA)]
        public ActionResult ArvoreConfiguracaoPaginaEtapa(SMCEncryptedLong seqConfiguracaoEtapa)
        {
            List<ArvoreItemConfiguracaoPaginaEtapaViewModel> itens = this.ConfiguracaoEtapaPaginaControllerService.BuscarItensArvoreConfiguracaoPaginaEtapa(seqConfiguracaoEtapa);
            List<SMCTreeViewNode<ArvoreItemConfiguracaoPaginaEtapaViewModel>> itensArvore = SMCTreeView.For<ArvoreItemConfiguracaoPaginaEtapaViewModel>(itens);

            return PartialView("_ArvoreItemConfiguracaoPaginaEtapa", itensArvore);
        }

        [SMCAuthorize(UC_INS_001_03_06.CONFIGURAR_PAGINAS_ETAPA)]
        public ActionResult CriarEstruturaPaginas(SMCEncryptedLong seqConfiguracaoEtapa, SMCEncryptedLong seqEtapa, SMCEncryptedLong seqProcesso)
        {
            this.ConfiguracaoEtapaPaginaControllerService.CriarPaginasPadraoConfiguracao(seqConfiguracaoEtapa);
            return SMCRedirectToAction("Index", "ConfiguracaoEtapaPagina", new
            {
                @seqConfiguracaoEtapa = seqConfiguracaoEtapa,
                @seqEtapa = seqEtapa,
                @seqProcesso = seqProcesso
            });
        }

        #endregion Arvore

        #region Incluir/Excluir Idioma

        /// <summary>
        /// Alterar o idioma da configuração da págian da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="etapa"></param>
        /// <param name="configuracaoEtapa"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_07.ASSOCIAR_DESASSOCIAR_IDIOMA)]
        public ActionResult AlterarIdiomaConfiguracaoPaginaEtapa(SMCEncryptedLong seqConfiguracaoEtapa)
        {
            var modelo = this.ConfiguracaoEtapaPaginaControllerService.BuscarIdiomasEtapa(seqConfiguracaoEtapa);

            return PartialView("_AlterarIdiomaEtapa", modelo);
        }

        /// <summary>
        /// Salvar as alterações de idioma de uma configuração de página da etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_07.ASSOCIAR_DESASSOCIAR_IDIOMA)]
        public ActionResult SalvarAlteraracaoIdiomaConfiguracaoPaginaEtapa(AlterarIdiomaEtapaViewModel modelo)
        {
            this.ConfiguracaoEtapaPaginaControllerService.SalvarAlteraracaoIdiomaConfiguracaoPaginaEtapa(modelo);

            return RenderAction("ArvoreConfiguracaoPaginaEtapa", new SMCEncryptedLong(modelo.SeqConfiguracaoEtapa));
        }

        #endregion Incluir/Excluir Idioma

        #region Recuperar Página Excluida

        /// <summary>
        /// Buscar uma recuperação de uma página da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="etapa"></param>
        /// <param name="configuracaoEtapa"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_08.RECUPERAR_PAGINA)]
        public ActionResult RecuperarPaginasConfiguracaoPaginaEtapa(SMCEncryptedLong seqConfiguracaoEtapa)
        {
            var modelo = new RecuperarPaginaEtapaViewModel
            {
                PaginasDisponiveis = this.ConfiguracaoEtapaPaginaControllerService.BuscarPaginasNaoCriadas(seqConfiguracaoEtapa),
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa
            };

            return PartialView("_RecuperarPaginasEtapa", modelo);
        }

        /// <summary>
        /// Salvar uma recuperação de página da configuração de págian da etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_08.RECUPERAR_PAGINA)]
        public ActionResult SalvarRecuperacaoPaginasConfiguracaoPaginaEtapa(RecuperarPaginaEtapaViewModel modelo)
        {
            if (modelo.SeqPaginas.Count > 0)
            {
                this.ConfiguracaoEtapaPaginaControllerService.SalvarRecuperacaoPaginasConfiguracaoPaginaEtapa(modelo);
            }

            return RenderAction("ArvoreConfiguracaoPaginaEtapa", new SMCEncryptedLong(modelo.SeqConfiguracaoEtapa));
        }

        #endregion Recuperar Página Excluida

        #region Configurar Página

        /// <summary>
        /// Configurar uma págian da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="seqPagina"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_09.CONFIGURAR_PAGINA)]
        public ActionResult ConfigurarPaginaConfiguracaoPaginaEtapa(SMCEncryptedLong seqConfiguracaoEtapaPagina)
        {
            var modelo = this.ConfiguracaoEtapaPaginaControllerService
                .BuscarConfiguracaoPagina(seqConfiguracaoEtapaPagina);

            return PartialView("_ConfigurarPaginaEtapa", modelo);
        }

        /// <summary>
        /// Salvar uma configuração de págian da etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_09.CONFIGURAR_PAGINA)]
        public void SalvarConfiguracaoPaginaEtapa(ConfigurarPaginaEtapaViewModel modelo)
        {
            this.ConfiguracaoEtapaPaginaControllerService.SalvarConfiguracaoPaginaEtapa(modelo);
        }

        #endregion Configurar Página

        #region Duplicar/Excluir página

        /// <summary>
        /// Excluir uma configuração de págian da etapa
        /// </summary>
        [SMCAuthorize(UC_INS_001_03_08.RECUPERAR_PAGINA)]
        public ActionResult ExcluirConfiguracaoEtapaPagina(SMCEncryptedLong seqConfiguracaoEtapa, SMCEncryptedLong seqConfiguracaoEtapaPagina)
        {
            this.ConfiguracaoEtapaPaginaControllerService.ExcluirConfiguracaoPaginaEtapa(seqConfiguracaoEtapaPagina);

            return RenderAction("ArvoreConfiguracaoPaginaEtapa", seqConfiguracaoEtapa);
        }

        /// <summary>
        /// Duplicar uma configuração de página da etapa
        /// </summary>
        [SMCAuthorize(UC_INS_001_03_08.RECUPERAR_PAGINA)]
        public ActionResult DuplicarPaginaConfiguracaoEtapaPagina(SMCEncryptedLong seqConfiguracaoEtapaPagina,
            SMCEncryptedLong seqConfiguracaoEtapa)
        {
            ConfiguracaoEtapaPaginaControllerService.DuplicarConfiguracaoEtapaPagina(seqConfiguracaoEtapaPagina);
            return RenderAction("ArvoreConfiguracaoPaginaEtapa", seqConfiguracaoEtapa);
        }

        #endregion Duplicar/Excluir página

        #region Configuração Página Idioma

        /// <summary>
        /// Confiogurar o idioma de uma configuração de página da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="seqIdioma"></param>
        /// <param name="pagina"></param>
        /// <param name="idioma"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_10.CONFIGURAR_PAGINA_IDIOMA)]
        public ActionResult ConfigurarPaginaIdiomaConfiguracaoPaginaEtapa(string paginaToken, SMCEncryptedLong seqConfiguracaoEtapaPaginaIdioma, string pagina, string idioma,
            SMCEncryptedLong seqConfiguracaoEtapa, SMCEncryptedLong seqEtapa, SMCEncryptedLong seqProcesso)
        {
            var modelo = this.ConfiguracaoEtapaPaginaControllerService
                .BuscarConfiguracaoPaginaIdioma(seqConfiguracaoEtapaPaginaIdioma);
            modelo.Pagina = pagina;
            modelo.Idioma = idioma;
            modelo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            modelo.SeqEtapa = seqEtapa;
            modelo.SeqProcesso = seqProcesso;
            modelo.PaginaToken = paginaToken;

            if (modelo.ExibeFormulario)
            {
                modelo.TiposFormulario = this.UnidadeResponsavelControllerService
                    .BuscarTiposFormularioAssociadosSelect(modelo.SeqUnidadeResponsavel);
                if (modelo.SeqFormulario.HasValue)
                {
                    modelo.Formularios = this.UnidadeResponsavelControllerService
                    .BuscarFormulariosSelect(modelo.SeqTipoFormulario.Value);
                    modelo.Visoes = this.UnidadeResponsavelControllerService
                    .BuscarVisoesSelect(modelo.SeqTipoFormulario.Value);
                }
            }

            return PartialView("_ConfigurarPaginaIdiomaEtapa", modelo);
        }

        /// <summary>
        /// Salvar uma configuração de idioma de uma configuração de página da etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_10.CONFIGURAR_PAGINA_IDIOMA)]
        public ActionResult SalvarConfiguracaoPaginaIdiomaEtapa(ConfigurarPaginaIdiomaEtapaViewModel modelo)
        {

            this.Assert(modelo, Views.ConfiguracaoEtapaPagina.App_LocalResources.UIResource.Assert_TrocaFormulario, () =>
            {
                if (ConfiguracaoEtapaPaginaControllerService.VerificaFormularioEmUso(modelo.SeqConfiguracaoEtapaPaginaIdioma))
                {
                    return ConfiguracaoEtapaPaginaControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapaPaginaIdioma);
                }
                return false;
            });

            this.ConfiguracaoEtapaPaginaControllerService.SalvarConfiguracaoPaginaIdiomaEtapa(modelo);

            return SMCRedirectToAction("Index", "ConfiguracaoEtapaPagina", new
            {
                SeqConfiguracaoEtapa = (SMCEncryptedLong)modelo.SeqConfiguracaoEtapa,
                SeqEtapa = (SMCEncryptedLong)modelo.SeqEtapa,
                SeqProcesso = (SMCEncryptedLong)modelo.SeqProcesso
            });
        }

        #endregion Configuração Página Idioma

        #region Seção Texto

        /// <summary>
        /// Configurar um texto de uma seção de configuração de uma página da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="seqSecao"></param>
        /// <param name="pagina"></param>
        /// <param name="idioma"></param>
        /// <param name="secao"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_11.CONFIGURAR_TEXTO)]
        public ActionResult ConfigurarTextoSecaoConfiguracaoPaginaEtapa(SMCEncryptedLong seq, string pagina, string idioma, string secao)
        {
            var modelo = this.ConfiguracaoEtapaPaginaControllerService.BuscarTextoSecao(seq);

            modelo.Pagina = pagina;
            modelo.Idioma = idioma;
            modelo.Secao = secao;

            return PartialView("_ConfigurarTextoSecao", modelo);
        }

        /// <summary>
        /// Salvar a configuração do texto de uma seção
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_11.CONFIGURAR_TEXTO)]
        public void SalvarConfiguracaoTextoSecao(ConfigurarTextoSecaoViewModel modelo)
        {
            modelo.Texto = modelo.Texto == "&lt;br&gt;" ? string.Empty : modelo.Texto;

            this.ConfiguracaoEtapaPaginaControllerService.SalvarConfiguracaoTextoSecao(modelo);
        }

        #endregion Seção Texto

        #region Seção Arquivo

        /// <summary>
        /// Configurar arquivos de uma seção em uma configuração de página da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="seqSecao"></param>
        /// <param name="pagina"></param>
        /// <param name="idioma"></param>
        /// <param name="secao"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_12.CONFIGURAR_ARQUIVOS)]
        public ActionResult ConfigurarArquivoSecaoConfiguracaoPaginaEtapa(SMCEncryptedLong seqSecaoPaginaSGF,
            SMCEncryptedLong seqConfiguracaoEtapaPaginaIdioma, string pagina, string idioma, string secao)
        {
            var modelo = new ConfigurarArquivoSecaoViewModel();

            modelo.SeqConfiguracaoEtapaPaginaIdioma = seqConfiguracaoEtapaPaginaIdioma;
            modelo.SeqSecaoPaginaSGF = seqSecaoPaginaSGF;
            modelo.Pagina = pagina;
            modelo.Idioma = idioma;
            modelo.Secao = secao;
            modelo.Arquivos.AddRange(this
                .ConfiguracaoEtapaPaginaControllerService.BuscarArquivosSecao(seqConfiguracaoEtapaPaginaIdioma,
                seqSecaoPaginaSGF));

            return PartialView("_ConfigurarArquivoSecao", modelo);
        }

        /// <summary>
        /// Salvar uma configuração de arquivo de uma seção
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_12.CONFIGURAR_ARQUIVOS)]
        public void SalvarConfiguracaoArquivoSecao(ConfigurarArquivoSecaoViewModel modelo)
        {
            foreach (var item in modelo.Arquivos)
            {
                item.Arquivo.FileData = SMCUploadHelper.GetFileData(item.Arquivo);
                item.SeqSecaoPaginaSGF = modelo.SeqSecaoPaginaSGF;
                item.SeqConfiguracaoEtapaPaginaIdioma = modelo.SeqConfiguracaoEtapaPaginaIdioma;
            }

            this.ConfiguracaoEtapaPaginaControllerService.SalvarConfiguracaoArquivoSecao(modelo);
        }

        #endregion Seção Arquivo

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarFormularios(long SeqTipoFormulario)
        {
            var listaFormularios = this.UnidadeResponsavelControllerService.BuscarFormulariosSelect(SeqTipoFormulario);

            return Json(listaFormularios);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarVisoes(long SeqTipoFormulario)
        {
            var listaVisoes = this.UnidadeResponsavelControllerService.BuscarVisoesSelect(SeqTipoFormulario);

            return Json(new { SeqVisao = listaVisoes, SeqVisaoGestao = listaVisoes });
        }
    }
}