using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.GPI.Administrativo.Areas.RES.Models;
using SMC.GPI.Administrativo.Areas.RES.Services;
using SMC.Inscricoes.Common.Areas.RES.Constants;
using SMC.Inscricoes.Service.Areas.RES.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.RES.Controllers
{
    public class UnidadeResponsavelController : SMCControllerBase
    {
        #region Serviços

        private UnidadeResponsavelControllerService UnidadeResponsavelControllerService
        {
            get
            {
                return this.Create<UnidadeResponsavelControllerService>();
            }
        }
        
        private UnidadeResponsavelService UnidadeResponsavelService
        {
            get
            {
                return this.Create<UnidadeResponsavelService>();
            }
        }

        private ICodigoAutorizacaoService CodigoAutorizacaoService
        {
            get
            {
                return this.Create<ICodigoAutorizacaoService>();
            }
        }

        private TipoHierarquiaOfertaControllerService TipoHierarquiaOfertaControllerService
        {
            get
            {
                return this.Create<TipoHierarquiaOfertaControllerService>();
            }
        }

        private TipoProcessoControllerService TipoProcessoControllerService
        {
            get
            {
                return this.Create<TipoProcessoControllerService>();
            }
        }

        #endregion Serviços

        #region Listagem

        /// <summary>
        /// Exibe tela de listagem de unidades responsáveis
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_RES_001_01_01.PESQUISAR_UNIDADE_RESPONSAVEL)]
        public ActionResult Index(UnidadeResponsavelFiltroViewModel filtros = null)
        {
            return View(filtros);
        }

        /// <summary>
        /// Renderiza grid de listagem de unidades responsáveis
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_RES_001_01_01.PESQUISAR_UNIDADE_RESPONSAVEL)]
        public ActionResult ListarUnidadeResponsavel(UnidadeResponsavelFiltroViewModel filtros)
        {
            SMCPagerModel<UnidadeResponsavelListaViewModel> pager = UnidadeResponsavelControllerService.BuscarUnidadesResponsaveis(filtros);
            return PartialView("_ListarUnidadeResponsavel", pager);
        }

        #endregion Listagem

        #region Edição/Inclusão

        /// <summary>
        /// Exibe tela de edição para uma unidade responsável
        /// </summary>
        /// <param name="seqEntidade">Sequencial da unidade responsável a ser editada</param>
        [HttpGet]
        [SMCAuthorize(UC_RES_001_01_02.MANTER_UNIDADE_RESPONSAVEL)]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            UnidadeResponsavelViewModel modelo = UnidadeResponsavelControllerService.BuscarUnidadeResponsavel(seq);
            PreencherModelo(modelo);
            return View(modelo);
        }

        /// <summary>
        /// Exibe a tela de inclusão de uma unidade responsável
        /// </summary>
        [HttpGet]
        [SMCAuthorize(UC_RES_001_01_02.MANTER_UNIDADE_RESPONSAVEL)]
        public ActionResult Incluir()
        {
            UnidadeResponsavelViewModel modelo = new UnidadeResponsavelViewModel();
            PreencherModelo(modelo);
            return View(modelo);
        }

        /// <summary>
        /// Salva uma unidade responsável e redireciona o usuário para a tela de alteração
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_RES_001_01_02.MANTER_UNIDADE_RESPONSAVEL)]
        public ActionResult Salvar(UnidadeResponsavelViewModel modelo)
        {
            return this.SaveEdit(modelo, this.UnidadeResponsavelControllerService.SalvarUnidadeResponsavel, PreencherModelo);
        }

        /// <summary>
        /// Salva uma unidade responsável e redireciona o usuário para a tela de novo registro
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_RES_001_01_02.MANTER_UNIDADE_RESPONSAVEL)]
        public ActionResult SalvarNovo(UnidadeResponsavelViewModel modelo)
        {
            return this.SaveNew(modelo, this.UnidadeResponsavelControllerService.SalvarUnidadeResponsavel, PreencherModelo);
        }

        /// <summary>
        /// Salva uma unidade responsável e redireciona o usuário para a tela de listagem
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_RES_001_01_02.MANTER_UNIDADE_RESPONSAVEL)]
        public ActionResult SalvarSair(UnidadeResponsavelViewModel modelo)
        {
            return this.SaveQuit(modelo, this.UnidadeResponsavelControllerService.SalvarUnidadeResponsavel, PreencherModelo);
        }

        #endregion Edição/Inclusão

        #region Excluir

        /// <summary>
        /// Action para excluir uma unidade responsável
        /// </summary>
        /// <param name="seq">Sequencial da unidade a ser excluído</param>
        [HttpPost]
        [SMCAuthorize(UC_RES_001_01_02.MANTER_UNIDADE_RESPONSAVEL)]
        public ActionResult Excluir(SMCEncryptedLong seq)
        {
            UnidadeResponsavelControllerService.ExcluirUnidadeResponsavel(seq);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                                            MessagesResource.Entidade_UnidadeResponsavel),
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

            return RedirectToAction("ListarUnidadeResponsavel");
        }

        #endregion Excluir

        #region Configuração

        /// <summary>
        /// Configura uma unidade responsável
        /// </summary>
        [HttpGet]
        [SMCAuthorize(UC_RES_001_01_03.PESQUISAR_CONFIGURACAO_UNIDADE_RESPONSAVEL)]
        public ActionResult Configurar(SMCEncryptedLong seq)
        {
            UnidadeResponsavelCabecalhoViewModel modelo = this.UnidadeResponsavelControllerService.BuscarUnidadeResponsavelCabecalho(seq);

            return View(modelo);
        }

        /// <summary>
        /// Exibe o painel de configuração de tipos de formulário
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_03.PESQUISAR_CONFIGURACAO_UNIDADE_RESPONSAVEL)]
        public ActionResult PainelTipoFormulario(SMCEncryptedLong seqParametro, int codigoUnidade)
        {
            UnidadeResponsavelCabecalhoViewModel modelo = new UnidadeResponsavelCabecalhoViewModel() { Seq = seqParametro, CodigoUnidade = codigoUnidade };
            return PartialView("PartialsParametros/_PainelTipoFormulario", modelo);
        }

        /// <summary>
        /// Lista os típos de formulário existentes
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_03.PESQUISAR_CONFIGURACAO_UNIDADE_RESPONSAVEL)]
        public ActionResult ListarConfiguracoesTipoFormulario(SMCEncryptedLong seqParametro, int codigoUnidade)
        {
            SMCPagerModel<UnidadeResponsavelTipoFormularioListaViewModel> pager = this.UnidadeResponsavelControllerService.BuscarUnidadeResponsavelTiposFormularios(seqParametro.Value);

            var model = new ListarConfiguracoesTipoFormularioViewModel() { Pager = pager, CodigoUnidade = codigoUnidade };

            return PartialView("PartialsParametros/_ListarConfiguracaoTipoFormulario", model);
        }

        /// <summary>
        /// Exibe o painel de configuração de tipos de processo e tipos de hierarquia de ofertas
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_03.PESQUISAR_CONFIGURACAO_UNIDADE_RESPONSAVEL)]
        public ActionResult PainelTipoProcesso(SMCEncryptedLong seqParametro)
        {
            return PartialView("PartialsParametros/_PainelTipoProcesso", seqParametro);
        }

        /// <summary>
        /// Lista os vinculos entre os típos de processos e os tipos de hierarquia de oferta existentes
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_03.PESQUISAR_CONFIGURACAO_UNIDADE_RESPONSAVEL)]
        public ActionResult ListarConfiguracoesTipoProcesso(SMCEncryptedLong seqParametro)
        {
            SMCPagerModel<UnidadeResponsavelTipoProcessoListaViewModel> model = this.UnidadeResponsavelControllerService.BuscarUnidadeResponsavelTiposProcessos(seqParametro.Value);
            return PartialView("PartialsParametros/_ListarConfiguracaoTipoProcesso", model);
        }

        #endregion Configuração

        #region Tipo Formulario

        /// <summary>
        /// Salva o vínculo entre um tipo de formulário com uma unidade responsável
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_04.MANTER_TIPO_FORMULARIO)]
        public ActionResult SalvarConfiguracaoTipoFormulario(UnidadeResponsavelTipoFormularioViewModel modelo, int codigoUnidade)
        {
            var isNew = modelo.Seq == 0;
            this.UnidadeResponsavelControllerService.SalvarUnidadeResponsavelTipoFormulario(modelo);
            if (isNew)
            {
                SetSuccessMessage(SMC.GPI.Administrativo.Areas.RES.Views.UnidadeResponsavel.App_LocalResources.UIResource.Mensagem_Sucesso_Associacao_Tipo_Formulario,
                    MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);
            }
            else
            {
                SetSuccessMessage(SMC.GPI.Administrativo.Areas.RES.Views.UnidadeResponsavel.App_LocalResources.UIResource.Mensagem_Sucesso_Alteracao_Associacao_Tipo_Processo_Tipo_Hierarquia_Oferta,
                    MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);
            }
            return ListarConfiguracoesTipoFormulario(new SMCEncryptedLong(modelo.SeqUnidadeResponsavel), codigoUnidade);
        }

        /// <summary>
        /// Inclui um vínculo entre um tipo de formulário com uma unidade responsável
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_04.MANTER_TIPO_FORMULARIO)]
        public ActionResult IncluirConfiguracaoTipoFormulario(SMCEncryptedLong seqUnidadeResponsavel, int codigoUnidade)
        {
            return PartialView("PartialsParametros/_ModalConfiguracaoTipoFormulario",
                new UnidadeResponsavelTipoFormularioViewModel
                {
                    SeqUnidadeResponsavel = seqUnidadeResponsavel.Value,
                    TiposFormulario = this.UnidadeResponsavelControllerService.BuscarTiposFormulariosSelect(seqUnidadeResponsavel),
                    CodigoUnidade = codigoUnidade
                });
        }

        /// <summary>
        /// Exclui um vínculo entre um tipo de formulário com uma unidade responsável
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_04.MANTER_TIPO_FORMULARIO)]
        public ActionResult ExcluirConfiguracaoTipoFormulario(SMCEncryptedLong seqFormulario, SMCEncryptedLong seqUnidadeResponsavel, int codigoUnidade)
        {
            this.UnidadeResponsavelControllerService.ExcluirUnidadeResponsavelTipoFormulario(seqFormulario.Value);
            SetSuccessMessage(SMC.GPI.Administrativo.Areas.RES.Views.UnidadeResponsavel.App_LocalResources.UIResource.Mensagem_Sucesso_Exclusao_Associacao_Tipo_Formulario,
                    MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);
            return ListarConfiguracoesTipoFormulario(seqUnidadeResponsavel, codigoUnidade);
        }

        [SMCAuthorize(UC_RES_001_01_04.MANTER_TIPO_FORMULARIO)]
        public ActionResult AlterarConfiguracaoTipoFormulario(SMCEncryptedLong seqFormulario, int codigoUnidade)
        {
            var modelo = this.UnidadeResponsavelControllerService.BuscarUnidadeResponsavelTipoFormulario(seqFormulario);
            modelo.CodigoUnidade = codigoUnidade;
            modelo.TiposFormulario = this.UnidadeResponsavelControllerService.BuscarTiposFormulariosSelect(modelo.SeqUnidadeResponsavel);

            return PartialView("PartialsParametros/_ModalConfiguracaoTipoFormulario", modelo);
        }

        #endregion Tipo Formulario

        #region Tipo Processo e Tipo de Hierarquia de Oferta

        /// <summary>
        /// Salva o vínculo entre um tipo de processo e um tipo de hierarquia de oferta com uma unidade responsável
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_05.MANTER_TIPO_PROCESSO)]
        public ActionResult SalvarConfiguracaoTipoProcesso(UnidadeResponsavelTipoProcessoViewModel modelo)
        {
            var isNew = modelo.Seq == 0;
            this.UnidadeResponsavelControllerService.SalvarUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(modelo);
            if (isNew)
            {
                SetSuccessMessage(SMC.GPI.Administrativo.Areas.RES.Views.UnidadeResponsavel.App_LocalResources.UIResource.Mensagem_Sucesso_Associacao_Tipo_Processo_Tipo_Hierarquia_Oferta,
                    MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);
            }
            else
            {
                SetSuccessMessage(SMC.GPI.Administrativo.Areas.RES.Views.UnidadeResponsavel.App_LocalResources.UIResource.Mensagem_Sucesso_Alteracao_Associacao_Tipo_Processo_Tipo_Hierarquia_Oferta,
                    MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);
            }
            return ListarConfiguracoesTipoProcesso(new SMCEncryptedLong(modelo.SeqUnidadeResponsavel));
        }

        /// <summary>
        /// Inclui um vínculo entre um tipo de processo e os tipos de hierarquia de oferta com uma unidade responsável
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_05.MANTER_TIPO_PROCESSO)]
        public ActionResult IncluirConfiguracaoTipoProcesso(SMCEncryptedLong seqUnidadeResponsavel)
        {
            var modelo = new UnidadeResponsavelTipoProcessoViewModel();

            modelo.SeqUnidadeResponsavel = seqUnidadeResponsavel;
            modelo.TiposProcesso = this.TipoProcessoControllerService.BuscarTiposProcessosSelect();
            modelo.TiposHierarquiaOferta = this.TipoHierarquiaOfertaControllerService.BuscarTiposHierarquiaOfertaSelect();

            return PartialView("PartialsParametros/_ModalConfiguracaoTipoProcesso", modelo);
        }

        /// <summary>
        /// Exclui um vínculo entre um tipo de processo e um tipo de hierarquia de oferta com uma unidade responsável
        /// </summary>
        [SMCAuthorize(UC_RES_001_01_05.MANTER_TIPO_PROCESSO)]
        public ActionResult ExcluirConfiguracaoTipoProcesso(SMCEncryptedLong seqParametro, SMCEncryptedLong seqUnidadeResponsavel)
        {
            this.UnidadeResponsavelControllerService.ExcluirUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(seqParametro.Value);
            SetSuccessMessage(SMC.GPI.Administrativo.Areas.RES.Views.UnidadeResponsavel.App_LocalResources.UIResource.Mensagem_Sucesso_Exclusao_Associacao_Tipo_Processo,
                    MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);
            return ListarConfiguracoesTipoProcesso(seqUnidadeResponsavel);
        }

        [SMCAuthorize(UC_RES_001_01_05.MANTER_TIPO_PROCESSO)]
        public ActionResult AlterarConfiguracaoTipoProcesso(SMCEncryptedLong seqParametro)
        {
            var modelo = this.UnidadeResponsavelControllerService.BuscarUnidadeResponsavelTipoProcesso(seqParametro);

            modelo.TiposProcesso = this.TipoProcessoControllerService.BuscarTiposProcessosSelect();
            modelo.TiposHierarquiaOferta = this.TipoHierarquiaOfertaControllerService.BuscarTiposHierarquiaOfertaSelect();
            modelo.LayoutMensagemEmail = this.UnidadeResponsavelService.BuscarLayoutNotificacaoEmailPorSiglaGrupoAplicacao();

            return PartialView("PartialsParametros/_ModalConfiguracaoTipoProcesso", modelo);
        }

        #endregion Tipo Processo e Tipo de Hierarquia de Oferta

        #region Preencher Modelo

        [NonAction]
        private void PreencherModelo(UnidadeResponsavelViewModel modelo)
        {
            // Monta a lista de tipo de telefone deixando apenas tipo comercial e fax
            if (modelo.TiposTelefone.SMCIsNullOrEmpty())
            {
                foreach (TipoTelefone item in Enum.GetValues(typeof(TipoTelefone)))
                {
                    if (item == TipoTelefone.Comercial || item == TipoTelefone.Fax)
                    {
                        modelo.TiposTelefone.Add(new SMCDatasourceItem()
                        {
                            Descricao = SMCEnumHelper.GetDescription(item),
                            Seq = Convert.ToInt64(item)
                        });
                    }
                }
            }
            modelo.UnidadesResponsaveisNotificacao = this.UnidadeResponsavelControllerService.BuscarUnidadesReponsaveisNotificacaoSelect();
            modelo.UnidadesResponsaveisSGF = this.UnidadeResponsavelControllerService.BuscarUnidadesResponsaveisSelectSGF();
            modelo.SistemaOrigemGADSelect = this.UnidadeResponsavelControllerService.BuscarSistemaOrigemGADSelect("GPI");
        }

        #endregion Preencher Modelo
    }
}