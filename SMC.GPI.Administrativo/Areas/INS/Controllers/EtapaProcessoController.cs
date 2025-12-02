using SMC.Framework;
using SMC.Framework.Exceptions;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class EtapaProcessoController : SMCControllerBase
    {
        #region Serviços

        private ProcessoControllerService ProcessoControllerService
        {
            get
            {
                return this.Create<ProcessoControllerService>();
            }
        }

        private HierarquiaOfertaControllerService HierarquiaOfertaControllerService
        {
            get
            {
                return this.Create<HierarquiaOfertaControllerService>();
            }
        }

        private EtapaProcessoControllerService EtapaProcessoControllerService
        {
            get
            {
                return this.Create<EtapaProcessoControllerService>();
            }
        }

        private ConfiguracaoEtapaControllerService ConfiguracaoEtapaControllerService
        {
            get
            {
                return this.Create<ConfiguracaoEtapaControllerService>();
            }
        }

        #endregion Serviços

        #region Pesquisa

        /// <summary>
        /// Exibir as etapa associadas a um detterminado processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [SMCAuthorize(UC_INS_001_03_01.PESQUISAR_ASSOCIACAO_ETAPA_PROCESSO)]
        public ActionResult Index(SMCEncryptedLong seqProcesso)
        {
            try
            {
                this.EtapaProcessoControllerService.VerificarPermissaoCadastrarEtapa(seqProcesso.Value);
                var modelo = new EtapaProcessoFiltroViewModel();

                modelo.SeqProcesso = seqProcesso;
                modelo.Cabecalho = this.ProcessoControllerService.BuscarCabecalhoProcesso(seqProcesso);

                return View("Index", modelo);
            }
            catch (SMCApplicationException e)
            {
                SetErrorMessage(e.Message);
            }
            return BackToAction();
        }

        /// <summary>
        /// Action para listar as etapas associadas ao processo
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_03_01.PESQUISAR_ASSOCIACAO_ETAPA_PROCESSO)]
        public ActionResult ListarEtapaProcesso(EtapaProcessoFiltroViewModel filtros)
        {
            SMCPagerModel<EtapaProcessoListaViewModel> pager = this.EtapaProcessoControllerService
                .BuscarEtapasProcesso(filtros);
            return PartialView("_ListarEtapaProcesso", pager);
        }

        #endregion Pesquisa

        #region Incluir/Alterar

        /// <summary>
        /// Exibe tela de associação de uma etapa
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_03_02.ASSOCIAR_ETAPA_PROCESSO)]
        public ActionResult Incluir(SMCEncryptedLong seqProcesso)
        {
            var modelo = new EtapaProcessoViewModel();
            modelo.SeqProcesso = seqProcesso;
            PreencherModeloInserir(modelo);
            return View(modelo);
        }

        private void PreencherModeloInserir(EtapaProcessoViewModel modelo)
        {
            modelo.SituacaoEtapa = (short)SituacaoEtapa.AguardandoLiberacao;
            modelo.Situacoes = new List<SMCDatasourceItem> { new SMCDatasourceItem
            {
                Seq = (short)SituacaoEtapa.AguardandoLiberacao,
                Descricao = SMCEnumHelper.GetDescription(SituacaoEtapa.AguardandoLiberacao)
            }};
            modelo.Etapas = this.EtapaProcessoControllerService.BuscarEtapasSelect(modelo.SeqProcesso);
        }

        /// <summary>
        /// Exibe tela de edição para uma associação de etapa
        /// </summary>
        /// <param name="seq">Sequencial da etapa a ser editada</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_03_02.ASSOCIAR_ETAPA_PROCESSO)]
        public ActionResult Editar(SMCEncryptedLong seq, SMCEncryptedLong seqProcesso)
        {
            var modelo = this.EtapaProcessoControllerService.BuscarEtapaProcesso(seq);
            modelo.Seq = seq;
            modelo.SeqProcesso = seqProcesso;
            PreencherModeloEditar(modelo);
            return View(modelo);
        }

        private void PreencherModeloEditar(EtapaProcessoViewModel modelo)
        {
            modelo.Situacoes = this.EtapaProcessoControllerService.BuscarSituacoesPermitidas(modelo.Seq);
            modelo.Etapas = this.EtapaProcessoControllerService.BuscarEtapasSelect(modelo.SeqProcesso);
        }

        /// <summary>
        /// Salva uma associacao de etapa para um processo e redireciona o usuário para a tela de alteração
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_02.ASSOCIAR_ETAPA_PROCESSO)]
        public ActionResult Salvar(EtapaProcessoViewModel modelo)
        {
            if (modelo.Seq == 0)
            {
                return this.SaveEdit(modelo, this.EtapaProcessoControllerService.SalvarAssociacaoEtapa, PreencherModeloInserir,
                    new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
            }
            else
            {
                if (modelo.SituacaoEtapa == (short)SituacaoEtapa.Liberada &&
                   this.EtapaProcessoControllerService.BuscarEtapaProcesso(modelo.Seq).SituacaoEtapa != (short)SituacaoEtapa.Liberada)
                {
                    try
                    {
                        this.EtapaProcessoControllerService.SalvarAssociacaoEtapa(modelo);
                        SetSuccessMessage(Views.EtapaProcesso.App_LocalResources.UIResource.Mensagem_Liberacao_Etapa, target: SMCMessagePlaceholders.Centro);
                        PreencherModeloEditar(modelo);
                        return View("Editar", modelo);
                    }
                    catch (Exception e)
                    {
                        PreencherModeloEditar(modelo);
                        this.SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);
                        return View("Editar", modelo);
                    }
                }
                else
                {
                    return this.SaveEdit(modelo, this.EtapaProcessoControllerService.SalvarAssociacaoEtapa, PreencherModeloEditar,
                     new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
                }
            }
        }

        /// <summary>
        /// Salva uma associação de etapa para um processo e redireciona o usuário para a tela de novo registro
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_02.ASSOCIAR_ETAPA_PROCESSO)]
        public ActionResult SalvarNovo(EtapaProcessoViewModel modelo)
        {
            if (modelo.Seq == 0)
            {
                return this.SaveNew(modelo, this.EtapaProcessoControllerService.SalvarAssociacaoEtapa,
                    PreencherModeloInserir, new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
            }
            else
            {
                if (modelo.SituacaoEtapa == (short)SituacaoEtapa.Liberada &&
                   this.EtapaProcessoControllerService.BuscarEtapaProcesso(modelo.Seq).SituacaoEtapa != (short)SituacaoEtapa.Liberada)
                {
                    try
                    {
                        this.EtapaProcessoControllerService.SalvarAssociacaoEtapa(modelo);
                        SetSuccessMessage(Views.EtapaProcesso.App_LocalResources.UIResource.Mensagem_Liberacao_Etapa, target: SMCMessagePlaceholders.Centro);
                        return Incluir(new SMCEncryptedLong(modelo.SeqProcesso));
                    }
                    catch (Exception e)
                    {
                        PreencherModeloEditar(modelo);
                        this.SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);
                        return View("Editar", modelo);
                    }
                }
                else
                {
                    return this.SaveNew(modelo, this.EtapaProcessoControllerService.SalvarAssociacaoEtapa,
                    PreencherModeloInserir, new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
                }
            }
        }

        /// <summary>
        /// Salva uma associação de etapa para um processo e redireciona o usuário para a tela de listagem
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_02.ASSOCIAR_ETAPA_PROCESSO)]
        public ActionResult SalvarSair(EtapaProcessoViewModel modelo)
        {
            if (modelo.Seq == 0)
            {
                return this.SaveQuit(modelo, this.EtapaProcessoControllerService.SalvarAssociacaoEtapa,
                    PreencherModeloInserir, new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
            }
            else
            {
                if (modelo.SituacaoEtapa == (short)SituacaoEtapa.Liberada &&
                     this.EtapaProcessoControllerService.BuscarEtapaProcesso(modelo.Seq).SituacaoEtapa != (short)SituacaoEtapa.Liberada)
                {
                    try
                    {
                        this.EtapaProcessoControllerService.SalvarAssociacaoEtapa(modelo);
                        SetSuccessMessage(Views.EtapaProcesso.App_LocalResources.UIResource.Mensagem_Liberacao_Etapa, target: SMCMessagePlaceholders.Centro);
                        return Index(new SMCEncryptedLong(modelo.SeqProcesso));
                    }
                    catch (Exception e)
                    {
                        PreencherModeloEditar(modelo);
                        this.SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);
                        return View("Editar", modelo);
                    }
                }
                else
                {
                    return this.SaveQuit(modelo, this.EtapaProcessoControllerService.SalvarAssociacaoEtapa,
                        PreencherModeloEditar, new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
                }
            }
        }

        #endregion Incluir/Alterar

        #region Excluir

        /// <summary>
        /// Exclui uma associação de etapa do processo
        /// </summary>
        /// <param name="seqEtapa">Sequencial da etapa a ser excluida</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_02.ASSOCIAR_ETAPA_PROCESSO)]
        public ActionResult ExcluirEtapaProcesso(SMCEncryptedLong seqEtapa, SMCEncryptedLong seqProcesso)
        {
            this.EtapaProcessoControllerService.ExcluirAssociacaoEtapa(seqEtapa);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                MessagesResource.Entidade_AssociacaoEtapa),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return ListarEtapaProcesso(new EtapaProcessoFiltroViewModel { SeqProcesso = seqProcesso });
        }

        #endregion Excluir

        #region Cabecalhos

        /// <summary>
        /// Exibe o cabeçalho contendo informações sobre o processo e a etapa
        /// </summary>
        /// <param name="seqEtapa">Sequencial da etapa</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <param name="action">Action que será chamada conforma a opção do menu</param>
        /// <param name="exibirBotaoAlterarAssociacaoEtapaProcesso">Condicional que exibe o botão de alteração da associação da etapa ao processo</param>
        /// <param name="exibirBotaoCadastroConfiguracaoEtapaProcesso">Condicional que exibe o botão para cadastro da configuração</param>
        /// <param name="exibirBotaoConfiguracaoEtapaProcesso">Condicional que exibe o botão com as opções de configuração da etapa</param>
        /// <returns></returns>
        [SMCAllowAnonymous]
        public ActionResult CabecalhoProcessoEtapa(SMCEncryptedLong seqEtapa,
                                                               SMCEncryptedLong seqProcesso,
                                                               SMCEncryptedLong seqConfiguracaoEtapa = null,
                                                               EtapaProcessoActionsEnum action = EtapaProcessoActionsEnum.Nenhum,
                                                               bool exibirBotaoConfiguracaoEtapaProcesso = false,
                                                               bool exibirBotaoAlterarAssociacaoEtapaProcesso = false,
                                                               bool exibirBotaoCadastroConfiguracaoEtapaProcesso = false)
        {
            CabecalhoProcessoEtapaViewModel modelo = this.EtapaProcessoControllerService.BuscarCabecalhoProcessoEtapa(seqEtapa);

            if (seqConfiguracaoEtapa != null)
            {
                modelo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            }

            modelo.SeqProcesso = seqProcesso;
            modelo.SeqEtapaProcesso = seqEtapa;
            modelo.ExibirBotaoAlterarAssociacaoEtapaProcesso = exibirBotaoAlterarAssociacaoEtapaProcesso;
            modelo.ExibirBotaoCadastroConfiguracaoEtapaProcesso = exibirBotaoCadastroConfiguracaoEtapaProcesso;
            modelo.ExibirBotaoConfiguracaoEtapaProcesso = exibirBotaoConfiguracaoEtapaProcesso;
            modelo.Action = action;

            return PartialView("_CabecalhoProcessoEtapa", modelo);
        }

        #endregion Cabecalhos

        #region Cópia de configurações de etapa

        /// <summary>
        /// Recupera o conteúdo da janela modal de cópia de configuirações da etapa
        /// </summary>
        [SMCAuthorize(UC_INS_001_03_05.COPIAR_CONFIGURACOES_ETAPA)]
        public ActionResult CopiaConfiguracoesEtapa(SMCEncryptedLong seqEtapa, SMCEncryptedLong seqProcesso)
        {
            var modelo = new CopiarConfiguracoesEtapaViewModel
            {
                SeqEtapaProcesso = seqEtapa,
                SeqProcessoOrigem = seqProcesso,
                SeqTipoTemplateProcessoOrigem = this.ProcessoControllerService.BuscarSeqTipoTemplateProcesso(seqProcesso),
                CopiarDocumentacao = true,
                CopiarPaginas = true
            };
            //FIX: Criar método de busca para as configurações da etapa de origem
            modelo.ConfiguracaoesEtapaOrigem = ConfiguracaoEtapaControllerService.BuscarConfiguracoesEtapaKeyValue(
                new ConfiguracaoEtapaFiltroViewModel { SeqEtapaProcesso = seqEtapa });
            return PartialView("_CopiarConfiguracoesEtapa", modelo);
        }

        /// <summary>
        /// Recupera o conteúdo da janela modal de cópia de configuirações da etapa
        /// </summary>
        [SMCAuthorize(UC_INS_001_03_05.COPIAR_CONFIGURACOES_ETAPA)]
        public JsonResult CopiarConfiguracoesEtapa(CopiarConfiguracoesEtapaViewModel modelo)
        {
            this.ConfiguracaoEtapaControllerService.CopiarConfiguracoesEtapa(modelo);
            SetSuccessMessage(
                SMC.GPI.Administrativo.Areas.INS.Views.EtapaProcesso.App_LocalResources.UIResource.Mensagem_Sucesso_Copia_Etapa,
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);
            return null;
        }

        #endregion Cópia de configurações de etapa

        #region Prorrogação/Alteração de período de etapa

        [SMCAuthorize(UC_INS_001_03_17.ALTERAR_PERIODO_ETAPA_PROCESSO)]
        public ActionResult ProrrogarEtapa(SMCEncryptedLong seqEtapaProcesso, SMCEncryptedLong seqProcesso)
        {
            try
            {
                this.EtapaProcessoControllerService.VerificarPossibilidadeProrrogacao(seqEtapaProcesso);
                var filtro = new ProrrogacaoEtapaFiltroViewModel { SeqEtapaProcesso = seqEtapaProcesso, SeqProcesso = seqProcesso };
                return View(filtro);
            }
            catch (SMCApplicationException e)
            {
                SetErrorMessage(e.Message);
            }
            return BackToAction();
        }

        [SMCAuthorize(UC_INS_001_03_17.ALTERAR_PERIODO_ETAPA_PROCESSO)]
        public ActionResult SelecionarOfertasProrrogacao(ProrrogacaoEtapaViewModel filtro)
        {
            filtro.Passo = 1;
            return PartialView("_SelecionarOfertasProrrogacao", filtro);
        }

        [SMCAuthorize(UC_INS_001_03_17.ALTERAR_PERIODO_ETAPA_PROCESSO)]
        public ActionResult EditarDadosProrrogacao(ProrrogacaoEtapaViewModel model)
        {
            if (model.Passo != 0)
            {
                model.Configuracoes = this.EtapaProcessoControllerService.BuscarConfiguracoesProrrogacao(
                    model.SeqEtapaProcesso, model.Ofertas.Select(x => x.Seq).ToArray());
            }
            model.EventosTaxa = this.HierarquiaOfertaControllerService.BuscarEventosTaxaSelect(model.SeqProcesso);
            return PartialView("_EditarDadosProrrogacao", model);
        }

        [SMCAuthorize(UC_INS_001_03_17.ALTERAR_PERIODO_ETAPA_PROCESSO)]
        [HttpPost]
        public ActionResult ProrrogarEtapa(ProrrogacaoEtapaViewModel modelo)
        {
            this.EtapaProcessoControllerService.ProrrogarEtapa(modelo);
            this.SetSuccessMessage(SMC.GPI.Administrativo.Areas.INS.Views.EtapaProcesso.App_LocalResources.UIResource
                .Mensagem_Sucesso_Prorrogacao);
            return WizardRedirect("Index", "EtapaProcesso", new { seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
        }

        [SMCAuthorize(UC_INS_001_03_17.ALTERAR_PERIODO_ETAPA_PROCESSO)]
        [HttpPost]
        public ActionResult SumarioProrrogacao(ProrrogacaoEtapaViewModel modelo)
        {
            modelo.Passo = 3;
            var ofertas = modelo.Ofertas;
            modelo = this.EtapaProcessoControllerService.SumarioProrrogacao(modelo);
            modelo.Ofertas = ofertas;
            return PartialView("_SumarioProrrogacao", modelo);
        }

        #endregion Prorrogação/Alteração de período de etapa
    }
}