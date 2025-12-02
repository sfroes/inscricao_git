using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Security;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.Areas.SEL.Models;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.SEL.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Data;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.SEL.Controllers
{
    public class AnaliseSelecaoController : SMCControllerBase
    {
        #region Serviços

        private ISelecaoService SelecaoService
        {
            get { return Create<ISelecaoService>(); }
        }

        private IInscricaoOfertaHistoricoSituacaoService InscricaoOfertaHistoricoSituacaoService
        {
            get { return Create<IInscricaoOfertaHistoricoSituacaoService>(); }
        }

        private ITipoProcessoService TipoProcessoService
        {
            get { return Create<ITipoProcessoService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return Create<ISituacaoService>(); }
        }

        private IInscricaoService InscricaoService
        {
            get { return Create<IInscricaoService>(); }
        }

        private IGrupoOfertaService GrupoOfertaService
        {
            get { return Create<IGrupoOfertaService>(); }
        }

        private IProcessoService ProcessoService
        {
            get { return Create<IProcessoService>(); }
        }

        #endregion Serviços

        // GET: SEL/AnaliseSelecao
        public ActionResult Index()
        {
            return RedirectToAction("Index", "AcompanhamentoSelecao");
        }

        #region Consulta Seleção em Lote

        [SMCAuthorize(UC_SEL_001_01_08.ANALISE_SELECAO_LOTE)]
        public ActionResult AnaliseLote(AnaliseSelecaoLoteFiltroViewModel filtro = null)
        {
            var cabecalho = SelecaoService.BuscarCabecalhoSelecaoProcesso(filtro.SeqProcesso);
            filtro.TipoProcesso = cabecalho.TipoProcesso;
            filtro.Descricao = cabecalho.Descricao;

            filtro.SituacoesProcesso = ProcessoService.BuscarSituacoesProcessoPorEtapaKeyValue(filtro.SeqProcesso, TOKENS.ETAPA_SELECAO, TOKENS.ETAPA_CONVOCACAO)
                                                        .TransformList<SMCDatasourceItem>();
            filtro.GrupoOfertas = GrupoOfertaService.BuscaGruposOfertaKeyValue(filtro.SeqProcesso).TransformList<SMCDatasourceItem>();

            // Corrige a exibição da descriçao da oferta. Por utilizar '>', o MVC converte para &gt;
            if (filtro.Oferta != null)
                filtro.Oferta.Descricao = Server.HtmlDecode(filtro.Oferta.Descricao);

            return View(filtro);
        }

        [SMCAuthorize(UC_SEL_001_01_08.ANALISE_SELECAO_LOTE)]
        public ActionResult ListarAnaliseLote(AnaliseSelecaoLoteFiltroViewModel filtro = null)
        {
            SMCPagerData<ConsultaCandidatosProcessoListaViewModel> lista;

            if (filtro.Oferta == null || !filtro.Oferta.Seq.HasValue)
            {
                lista = new SMCPagerData<ConsultaCandidatosProcessoListaViewModel>();
            }
            else
            {
                lista = SelecaoService.BuscarCandidatosProcesso(filtro.Transform<ConsultaCandidatosProcessoFiltroData>())
                                        .Transform<SMCPagerData<ConsultaCandidatosProcessoListaViewModel>>();
                ViewBag.SeqOferta = filtro.Oferta.Seq;
            }

            var model = new SMCPagerModel<ConsultaCandidatosProcessoListaViewModel>(lista, filtro.PageSettings, filtro);
            return PartialView("_ListarAnaliseLote", model);
        }

        #endregion Consulta Seleção em Lote

        #region Lançamento de resultado

        [SMCAuthorize(UC_SEL_001_01_04.LANCAMENTO_RESULTADO)]
        public ActionResult LancamentoResultado(LancamentoResultadoViewModel model)
        {
            try
            {
                var cabecalho = SelecaoService.BuscarCabecalhoSelecaoOferta(model.SeqOferta.GetValueOrDefault());
                model = SMCMapperHelper.Create(model, cabecalho);


                if (model.Lancamentos == null)
                {
                    try
                    {
                        model.Lancamentos = SelecaoService.BuscarInscricoesOfertaParaSelecao(model.SeqProcesso, model.InscricoesOfertas)
                                                       .TransformList<LancamentoResultadoItemViewModel>();
                    }
                    catch (Exception ex)
                    {

                        
                        if (model.BackUrl != null)
                        {
                            SetErrorMessage(ex.Message);
                            return SMCRedirectToUrl(model.BackUrl);
                        }
                        else
                        {
                            throw ex; 
                        }
                    }
                }

                model.Situacoes = TipoProcessoService.BuscarTipoProcessoSitucaoDestinoPorToken(model.SeqProcesso, TOKENS.SITUACAO_CANDIDATO_CONFIRMADO, TOKENS.ETAPA_SELECAO)
                                                        .TransformList<SMCDatasourceItem>();
                return View(model);
            }
            catch (SMCApplicationException e)
            {
                SetErrorMessage(e.Message);
                if (!model.EdicaoLote)
                {
                    return RedirectToAction("ConsultaSelecoesProcesso", "AcompanhamentoSelecao", new { seqProcesso = (SMCEncryptedLong)model.SeqProcesso });
                }
                else
                {
                    return RedirectToAction("AnaliseLote", "AnaliseSelecao", new { seqProcesso = (SMCEncryptedLong)model.SeqProcesso }); ;
                }
            }
        }

        [SMCAuthorize(UC_SEL_001_01_04.LANCAMENTO_RESULTADO)]
        public ActionResult ExibirOfertasInscrito(long seqInscricao, string opcao)
        {
            ViewBag.Opcao = opcao;
            var model = SelecaoService.BuscarOpcoesInscricao(seqInscricao).TransformList<OpcoesInscricaoViewModel>();
            return PartialView("_ExibirOfertasInscrito", model);
        }

        [SMCAuthorize(UC_SEL_001_01_04.LANCAMENTO_RESULTADO)]
        [HttpPost]
        public ActionResult SalvarLancamento(LancamentoResultadoViewModel model)
        {

            try
            {
                var lancamentos = model.Lancamentos.TransformList<LancamentoResultadoItemData>();
                Assert(model, Views.AnaliseSelecao.App_LocalResources.UIResource.Assert_QuantidadeVagas, () =>
                {
                    return !SelecaoService.VerificaDisponibilidadeVagas(model.SeqOferta.Value, lancamentos);
                });

                SelecaoService.SalvarLancamentos(lancamentos);
                if (string.IsNullOrEmpty(model.BackUrl))
                {
                    if (!model.EdicaoLote)
                    {
                        SetSuccessMessage(Views.AnaliseSelecao.App_LocalResources.UIResource.Mensagem_Sucesso_Salvar_Lancamento, target: SMCMessagePlaceholders.Centro);
                        return SMCRedirectToAction("ConsultaSelecoesProcesso", "AcompanhamentoSelecao", new { seqProcesso = (SMCEncryptedLong)model.SeqProcesso });
                    }
                    else
                    {
                        SetSuccessMessage(Views.AnaliseSelecao.App_LocalResources.UIResource.Mensagem_Sucesso_Salvar_Lancamento, target: SMCMessagePlaceholders.Centro);
                        return SMCRedirectToAction("AnaliseLote", "AnaliseSelecao", new { seqProcesso = (SMCEncryptedLong)model.SeqProcesso }); ;
                    }
                }
                else
                {
                    SetSuccessMessage(Views.AnaliseSelecao.App_LocalResources.UIResource.Mensagem_Sucesso_Salvar_Lancamento, target: SMCMessagePlaceholders.Centro);
                    return SMCRedirectToUrl(model.BackUrl);
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message, target: SMCMessagePlaceholders.Centro);
                return null;
            }


        }

        [SMCAuthorize(UC_SEL_001_01_04.LANCAMENTO_RESULTADO)]
        public ActionResult DesfazerLancamento(SMCEncryptedLong seqInscricaoOferta)
        {
            try
            {
                var insOfertas = new List<long> { seqInscricaoOferta };
                SelecaoService.DesfazerLancamentoResultado(insOfertas);
                SetSuccessMessage(Views.AnaliseSelecao.App_LocalResources.UIResource.Mensagem_Sucesso_Desfazer_Lancamento, target: SMCMessagePlaceholders.Centro);
                return null;
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex);
                return null;
            }
        }

        [SMCAuthorize(UC_SEL_001_01_04.LANCAMENTO_RESULTADO)]
        public ActionResult DesfazerLancamentoLote(LancamentoResultadoViewModel model)
        {
            try
            {
                var assertModel = new AssertLancamentoResultadoLoteViewModel();
                assertModel.Candidatos = InscricaoService.BuscarNomesInscritos(model.InscricoesOfertas);
                Assert(model, "_AssertDesfazerLancamentoLote", assertModel, () =>
                {
                    return assertModel.Candidatos.Any();
                });

                SelecaoService.DesfazerLancamentoResultado(model.InscricoesOfertas);
                SetSuccessMessage(Views.AnaliseSelecao.App_LocalResources.UIResource.Mensagem_Sucesso_Desfazer_Lancamento_Lote, target: SMCMessagePlaceholders.Centro);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex);
            }
            return RenderAction("ListarAnaliseLote");
        }

        [SMCAuthorize(UC_SEL_001_01_04.LANCAMENTO_RESULTADO)]
        public JsonResult BuscarMotivosSituacao(long seqResultadoSelecao)
        {
            var tipoProcesso = TipoProcessoService.BuscarTipoProcessoSituacao(seqResultadoSelecao);
            var motivos = SituacaoService.BuscarMotivosSituacao(tipoProcesso.SeqSituacao).TransformList<SMCDatasourceItem>();

            return Json(motivos);
        }

        #endregion Lançamento de resultado

        #region Historico Situação

        [SMCAuthorize(UC_SEL_001_01_06.PESQUISAR_HISTORICO_SITUACAO_SELECAO)]
        public ActionResult HistoricoSituacao(SMCEncryptedLong seqProcesso, SMCEncryptedLong seqInscricaoOferta, string backUrl = null)
        {
            var model = SelecaoService.BuscarCabecalhoInscricaoOferta(seqInscricaoOferta)
                                        .Transform<HistoricoSituacaoListaViewModel>();

            model.Historicos = SelecaoService.BuscarHistoricosSituacao(seqInscricaoOferta)
                                                .TransformList<HistoricoSituacaoItemViewModel>();
            model.BackURL = backUrl;
            return View(model);
        }

        [SMCAuthorize(UC_SEL_001_01_07.MANTER_HISTORICO_SITUACAO_SELECAO)]
        public ActionResult EditarHistoricoSituacao(SMCEncryptedLong seqHistoricoSituacao, SMCEncryptedLong seqInscricaoOferta, string backUrl)
        {
            // Se não passar de parametros os sequencias, redireciona para a página inicial do sistema para não exibir erros na tela.
            if (seqHistoricoSituacao == null && seqInscricaoOferta == null)
                return RedirectToAction("Index", "Home", new { area = string.Empty });

            var model = new HistoricoSituacaoViewModel()
            {
                SeqInscricaoOferta = seqInscricaoOferta
            };
            model = SMCMapperHelper.Create(model, SelecaoService.BuscarHistoricoSituacao(seqHistoricoSituacao));

            model.BackUrl = backUrl;

            PreencherModeloHistoricoSituacao(model);
            return View(model);
        }

        [SMCAuthorize(UC_SEL_001_01_07.MANTER_HISTORICO_SITUACAO_SELECAO)]
        public ActionResult MotivoRequerJustificativa(long seqMotivo)
        {
            return new ContentResult { Content = SituacaoService.BuscarMotivo(seqMotivo).ExigeJustificativa.ToString() };
        }

        [SMCAuthorize(UC_SEL_001_01_07.MANTER_HISTORICO_SITUACAO_SELECAO)]
        public ActionResult SalvarHistoricoSituacao(HistoricoSituacaoViewModel model)
        {
            return this.Save<HistoricoSituacaoData, HistoricoSituacaoViewModel>(model, SelecaoService.SalvarAlteracaoSituacao,
                    "HistoricoSituacao", null, "EditarHistoricoSituacao",
                    routeValues: new
                    {
                        seqProcesso = SMCDESCrypto.EncryptNumberForURL(model.SeqProcesso),
                        seqInscricaoOferta = SMCDESCrypto.EncryptNumberForURL(model.SeqInscricaoOferta),
                        seqHistoricoSituacao = SMCDESCrypto.EncryptNumberForURL(model.Seq),
                        backUrl=model.BackUrl
                    },
                    preencherModelo: PreencherModeloHistoricoSituacao);
        }

        private void PreencherModeloHistoricoSituacao(HistoricoSituacaoViewModel model)
        {
            model = SMCMapperHelper.Create(model, SelecaoService.BuscarCabecalhoInscricaoOferta(model.SeqInscricaoOferta));
            model.Motivos = SituacaoService.BuscarMotivosSituacao(model.SeqSituacao).TransformList<SMCDatasourceItem>();
        }

        #endregion Historico Situação

        #region Convocação

        [SMCAuthorize(UC_SEL_001_01_05.CONVOCACAO)]
        public ActionResult Convocacao(ConvocacaoFiltroViewModel filtro)
        {
            var model = new ConvocacaoViewModel()
            {
                SeqProcesso = filtro.SeqProcesso
            };
            try
            {
                var cabecalho = SelecaoService.BuscarCabecalhoSelecaoOferta(filtro.SeqOferta);
                model = SMCMapperHelper.Create(model, cabecalho);

                model.Convocados = SelecaoService.BuscarInscricoesOfertaParaConvocacao(filtro.SeqProcesso, filtro.InscricoesOfertas)
                    .TransformList<ConvocacaoItemViewModel>();

                // A busca pelos conmvocados traz apenas usuarios da mesma situacao.
                var convocado = model.Convocados.First();
                model.SituacaoAtual = convocado.Situacao;

                model.SituacoesDestino = TipoProcessoService
                    .BuscarTipoProcessoSitucaoDestinoPorToken(filtro.SeqProcesso, convocado.TokenSituacao, TOKENS.ETAPA_CONVOCACAO, convocado.TokenEtapa != TOKENS.ETAPA_SELECAO, false)
                    .TransformList<SMCDatasourceItem>();

                return PartialView("_Convocacao", model);
            }
            catch (SMCApplicationException ex)
            {
                return ThrowOpenModalException(ex.Message);
            }
        }

        [HttpPost]
        [SMCAuthorize(UC_SEL_001_01_05.CONVOCACAO)]
        public ActionResult SalvarConvocacao(ConvocacaoViewModel model)
        {
            var data = model.Transform<AlterarHistoricoSituacaoData>();
            data.SeqInscricoesOferta = model.Convocados.Select(f => f.SeqInscricaoOferta).ToList();
            InscricaoOfertaHistoricoSituacaoService.AlterarHistoricoSituacao(data);

            SetSuccessMessage(Views.AnaliseSelecao.App_LocalResources.UIResource.Mensagem_Sucesso_Alteracao_Situacao, target: SMCMessagePlaceholders.Centro);

            return Json(true);
        }

        #endregion Convocação
    }
}