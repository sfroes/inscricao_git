using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class TipoProcessoController : SMCControllerBase
    {
        #region Serviços


        private TipoProcessoControllerService TipoProcessoControllerService
        {
            get
            {
                return this.Create<TipoProcessoControllerService>();
            }
        }

        private ITipoTaxaService TipoTaxaService
        {
            get
            {
                return this.Create<ITipoTaxaService>();
            }
        }
        private ITipoDocumentoService TipoDocumentoService
        {
            get
            {
                return this.Create<ITipoDocumentoService>();
            }
        }

        private TipoProcessoCampoInscritoService TipoProcessoCampoInscritoService => this.Create<TipoProcessoCampoInscritoService>();

        #endregion Serviços

        #region Listagem

        /// <summary>
        /// Exibe tela de listagem de tipos de processo
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_09_01.PESQUISAR_TIPO_PROCESSO)]
        public ActionResult Index(TipoProcessoFiltroViewModel filtros = null)
        {
            return View(filtros);
        }

        /// <summary>
        /// Action para listar os tipos de processo
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_09_01.PESQUISAR_TIPO_PROCESSO)]
        public ActionResult ListarTipoProcesso(TipoProcessoFiltroViewModel filtros)
        {
            SMCPagerModel<TipoProcessoListaViewModel> pager = this.TipoProcessoControllerService.BuscarTiposProcesso(filtros);
            return PartialView("_ListarTipoProcesso", pager);
        }

        #endregion Listagem

        #region Edição/Inclusão

        /// <summary>
        /// Exibe a tela de inclusão de um tipo de processo
        /// </summary>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public ActionResult Incluir()
        {
            this.SetViewMode(SMCViewMode.Insert);

            TipoProcessoViewModel modelo = new TipoProcessoViewModel();

            modelo.TiposTemplateProcessoSelect = this.TipoProcessoControllerService.BuscarTiposTemplateProcessoSelect();
            modelo.TiposTaxaSelect = this.TipoTaxaService.BuscarTiposTaxaSelect();
            modelo.TiposDocumentoSelect = this.TipoDocumentoService.BuscarTiposDocumentoKeyValue();
            modelo.ContextoBibliotecaSelect = TipoProcessoControllerService.BuscarContextosBibliotecas();

            PreencheCamposInscrito(modelo);

            return View(modelo);
        }

        /// <summary>
        /// Exibe tela de edição para um tipo de processo
        /// </summary>
        /// <param name="seq">Sequencial do tipo de processo a ser editado</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            this.SetViewMode(SMCViewMode.Edit);

            TipoProcessoViewModel modelo = this.TipoProcessoControllerService.BuscarTipoProcesso(seq);
            modelo.ContextoBibliotecaSelect = TipoProcessoControllerService.BuscarContextosBibliotecas();
            if (modelo.SeqContextoBibliotecaGed.HasValue)
            {
                modelo.SeqContexto = this.TipoProcessoControllerService.BuscarSeqContexto(modelo.SeqContextoBibliotecaGed.Value);
            }
            PreencherModelo(modelo);

            return View(modelo);
        }

        /// <summary>
        /// Salva um tipo de processo e redireciona o usuário para a tela de alteração
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public ActionResult Salvar(TipoProcessoViewModel modelo)
        {
            try
            {
                modelo.Seq = this.TipoProcessoControllerService.SalvarTipoProcesso(modelo);
                modelo.ContextoBibliotecaSelect = TipoProcessoControllerService.BuscarContextosBibliotecas();
                if (modelo.SeqContextoBibliotecaGed.HasValue)
                {
                    modelo.SeqContexto = this.TipoProcessoControllerService.BuscarSeqContexto(modelo.SeqContextoBibliotecaGed.Value);
                }

                SetSuccessMessage("Tipo de Processo salvo com sucesso.",
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

                return SMCRedirectToAction("Editar", routeValues: new { Seq = modelo.Seq });

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message, MessagesResource.Titulo_Erro, SMCMessagePlaceholders.Centro);
                return BackToAction();
            }
        }

        /// <summary>
        /// Salva um tipo de processo e redireciona o usuário para a tela de novo registro
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public ActionResult SalvarNovo(TipoProcessoViewModel modelo)
        {
            try
            {
                modelo.Seq = this.TipoProcessoControllerService.SalvarTipoProcesso(modelo);
                modelo.ContextoBibliotecaSelect = TipoProcessoControllerService.BuscarContextosBibliotecas();
                if (modelo.SeqContextoBibliotecaGed.HasValue)
                {
                    modelo.SeqContexto = this.TipoProcessoControllerService.BuscarSeqContexto(modelo.SeqContextoBibliotecaGed.Value);
                }

                SetSuccessMessage("Tipo de Processo salvo com sucesso.",
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

                return SMCRedirectToAction("Incluir");

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message, MessagesResource.Titulo_Erro, SMCMessagePlaceholders.Centro);
                return BackToAction();
            }
        }

        /// <summary>
        /// Salva um tipo de processo e redireciona o usuário para a tela de listagem
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public ActionResult SalvarSair(TipoProcessoViewModel modelo)
        {
            try
            {
                modelo.Seq = this.TipoProcessoControllerService.SalvarTipoProcesso(modelo);
                modelo.ContextoBibliotecaSelect = TipoProcessoControllerService.BuscarContextosBibliotecas();
                if (modelo.SeqContextoBibliotecaGed.HasValue)
                {
                    modelo.SeqContexto = this.TipoProcessoControllerService.BuscarSeqContexto(modelo.SeqContextoBibliotecaGed.Value);
                }

                SetSuccessMessage("Tipo de Processo salvo com sucesso.",
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

                return SMCRedirectToAction("Index");

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message, MessagesResource.Titulo_Erro, SMCMessagePlaceholders.Centro);
                return BackToAction();
            }
        }

        private void PreencherModelo(TipoProcessoViewModel modelo)
        {
            modelo.TiposTemplateProcessoSelect = this.TipoProcessoControllerService.BuscarTiposTemplateProcessoSelect();
            modelo.TemplatesProcessoSelect = this.TipoProcessoControllerService.BuscarTemplatesTiposProcessoSelect(modelo.SeqTipoTemplateProcessoSGF.Value);
            modelo.TiposTaxaSelect = this.TipoTaxaService.BuscarTiposTaxaSelect();
            modelo.TiposDocumentoSelect = this.TipoDocumentoService.BuscarTiposDocumentoKeyValue();
            modelo.HabilitaPercentualDesconto = TipoProcessoControllerService.BuscarTipoProcesso(modelo.Seq.Value).HabilitaPercentualDesconto;
            modelo.ValidaLimiteDesconto = TipoProcessoControllerService.BuscarTipoProcesso(modelo.Seq.Value).ValidaLimiteDesconto;

            PreencheCamposInscrito(modelo);
        }
        #endregion Edição/Inclusão

        private void PreencheCamposInscrito(TipoProcessoViewModel modelo)
        {
            modelo.ListaCamposInscrito = new List<SMCSelectListItem>();
            foreach (CampoInscrito item in Enum.GetValues(typeof(CampoInscrito)))
            {
                if (item != CampoInscrito.Nenhum)
                {
                    modelo.ListaCamposInscrito.Add(new SMCSelectListItem()
                    {
                        Text = SMCEnumHelper.GetDescription(item),
                        Value = Convert.ToInt64(item).ToString(),
                    });
                }
            }
        }

        #region Excluir

        /// <summary>
        /// Action para excluir um tipo de processo
        /// </summary>
        /// <param name="seq">Sequencial do tipo de processo a ser excluído</param>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public ActionResult Excluir(SMCEncryptedLong seq)
        {
            TipoProcessoControllerService.ExcluirTipoProcesso(seq);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                                            MessagesResource.Entidade_TipoProcesso),
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

            return RenderAction("ListarTipoProcesso");
        }

        #endregion Excluir

        #region Configuração de Template de Processo

        [HttpPost]
        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public ActionResult BuscarConfiguracaoTemplateProcesso(string descricaoTipoProcesso, long? seqTipoTemplateProcesso)
        {
            if (!seqTipoTemplateProcesso.HasValue)
                return null;

            var modelo = new TipoProcessoViewModel();

            modelo.Descricao = descricaoTipoProcesso;
            modelo.SeqTipoTemplateProcessoSGF = seqTipoTemplateProcesso;

            modelo.TemplatesProcessoSelect = this.TipoProcessoControllerService.BuscarTemplatesTiposProcessoSelect(seqTipoTemplateProcesso.Value);
            modelo.Situacoes = this.TipoProcessoControllerService.BuscarSituacoesTiposProcesso(seqTipoTemplateProcesso.Value);


            return PartialView("_ConfiguracaoTipoProcesso", modelo);
        }

        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public bool? VerficaHabilitaPercentualDesconto(bool habilitaPercentualDesconto)
        {

            if (!habilitaPercentualDesconto)
                return false;
            else
                return null;

        }


        #endregion Configuração de Template de Processo

        [SMCAuthorize(UC_INS_001_09_02.MANTER_TIPO_PROCESSO)]
        public JsonResult BuscarSeqContexto(long seqContextoBibliotecaGed)
        {
            var seqContexto = this.TipoProcessoControllerService.BuscarSeqContexto(seqContextoBibliotecaGed);
            return Json(seqContexto);
        }
    }
}
