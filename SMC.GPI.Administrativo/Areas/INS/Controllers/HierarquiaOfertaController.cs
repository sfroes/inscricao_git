using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Security;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class HierarquiaOfertaController : SMCControllerBase
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

        private GrupoOfertaControllerService GrupoOfertaControllerService
        {
            get
            {
                return this.Create<GrupoOfertaControllerService>();
            }
        }

        private TipoProcessoControllerService TipoProcessoControllerService
        {
            get
            {
                return this.Create<TipoProcessoControllerService>();
            }
        }

        private ICodigoAutorizacaoService CodigoAutorizacaoService
        {
            get { return Create<ICodigoAutorizacaoService>(); }
        }

        private IProcessoService ProcessoService => Create<IProcessoService>();

        private IOfertaService OfertaService => Create<IOfertaService>();

        private ITipoProcessoService TipoProcessoService => Create<ITipoProcessoService>();

        #endregion Serviços

        #region Pesquisa

        [SMCAuthorize(UC_INS_001_01_03.MANTER_HIERARQUIA_OFERTA)]
        public ActionResult HierarquiaOferta(SMCEncryptedLong seqProcesso)
        {
            try
            {
                this.HierarquiaOfertaControllerService.VerificarPermissaoCadastrarHierarquia(seqProcesso.Value);
                var modelo = this.HierarquiaOfertaControllerService.BuscarInformaceosHierarquiaOferta(seqProcesso);
                return View(modelo);
            }
            catch (SMCApplicationException e)
            {
                SetErrorMessage(e.Message);
            }
            return BackToAction();
        }

        [SMCAuthorize(UC_INS_001_01_03.MANTER_HIERARQUIA_OFERTA)]
        public ActionResult ArvoreHierarquiaOferta(SMCEncryptedLong seqProcesso, long? idNode, string expandedNodes)
        {
            bool exibeArvoreFechada = ProcessoService.ExibeArvoreFechada(seqProcesso);
            ViewBag.ExibeArvoreFechada = exibeArvoreFechada;

            // Não executa os serviços de buscar caso seja uma childaction e a árvore seja fechada.
            if (ControllerContext.IsChildAction && exibeArvoreFechada)
            {
                return PartialView("_ArvoreItemHierarquiaOferta", new List<SMCTreeViewNode<ArvoreItemHierarquiaOfertaViewModel>>());
            }
            else
            {

                long[] nodes = null;
                if (!string.IsNullOrWhiteSpace(expandedNodes))
                {
                    nodes = expandedNodes.Split(',').Select(f => Convert.ToInt64(f)).ToArray();
                }

                var gestaoEventosHabilitada = TipoProcessoService.BuscarTipoProcessoPorProcesso(seqProcesso).GestaoEventos;

                List<ArvoreItemHierarquiaOfertaViewModel> itens = OfertaService.BuscarArvoreOfertaCompleta(seqProcesso, idNode, nodes)
                                                                        .TransformList<ArvoreItemHierarquiaOfertaViewModel>();
  

                var itensArvore = SMCTreeView.For(itens);
                if (itens.Count > 0 && exibeArvoreFechada)
                {
                    itensArvore.HasChildren(f => !f.EOferta);
                }
                return PartialView("_ArvoreItemHierarquiaOferta", (List<SMCTreeViewNode<ArvoreItemHierarquiaOfertaViewModel>>)itensArvore);

            }

        }

        #endregion Pesquisa

        #region Cadastrar Item Hierarquia

        /// <summary>
        /// Action para listar os itens de hierarquia de oferta para associação a árvore
        /// </summary>
        [SMCAuthorize(UC_INS_001_01_04.ASSOCIAR_ITEM_HIERARQUIA_OFERTA)]
        public ActionResult AssociarItemHierarquiaOferta(SMCEncryptedLong seqProcesso, SMCEncryptedLong seqPai)
        {
            if (ProcessoService.VerificaProcessoPossuiIntegracao(seqProcesso))
            {
                return ThrowOpenModalException(Views.HierarquiaOferta.App_LocalResources.UIResource.Mensagem_Bloqueio_PossuiIntegracao);
            }

            var modelo = new AssociarItemHierarquiaOfertaViewModel
            {
                SeqProcesso = seqProcesso,
                ItemsHierarquiaOferta = this.ProcessoControllerService.BuscarTiposItemHierarquiaOfertaSelect(seqProcesso,
                                                    seqPai == null ? new Nullable<long>() : seqPai.Value, false)
            };

            return PartialView("_AssociarItemHierarquiaOferta", modelo);
        }

        /// <summary>
        /// Salva a associação de um item de hierarquia a árvore
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_04.ASSOCIAR_ITEM_HIERARQUIA_OFERTA)]
        public ActionResult SalvarAssociacaoItemHierarquiaOferta(AssociarItemHierarquiaOfertaViewModel itemHierarquiaOferta)
        {
            this.HierarquiaOfertaControllerService.SalvarHierarquiaOferta(itemHierarquiaOferta);

            return SMCRedirectToAction(nameof(HierarquiaOferta), "HierarquiaOferta", new { seqProcesso = (SMCEncryptedLong)itemHierarquiaOferta.SeqProcesso });
        }

        /// <summary>
        /// Inclui um item na arvore de hierarquia de oferta
        /// </summary>
        /// <param name="seqItemSuperior"></param>
        /// <param name="descricaoItemSuperior"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_01_04.ASSOCIAR_ITEM_HIERARQUIA_OFERTA)]
        public ActionResult IncluirItemHierarquiaOferta(SMCEncryptedLong seqItemSuperior, string descricaoItemSuperior, SMCEncryptedLong seqProcesso)
        {
            if (ProcessoService.VerificaProcessoPossuiIntegracao(seqProcesso))
            {
                return ThrowOpenModalException(Views.HierarquiaOferta.App_LocalResources.UIResource.Mensagem_Bloqueio_PossuiIntegracao);
            }

            //cast de SMCEncryptedLong para long? não esta funcionando corretamente. Se SMCEncryptedLong é null, o valor do long? fica 0 ao invez de NULL.
            long? seqitemPai = null;
            if (seqItemSuperior != null)
            {
                seqitemPai = seqItemSuperior;
            }

            var itensHierarquiaOferta = this.ProcessoControllerService.BuscarTiposItemHierarquiaOfertaSelect(seqProcesso, seqitemPai, false);
            var modelo = new AssociarItemHierarquiaOfertaViewModel()
            {
                SeqProcesso = seqProcesso,
                SeqPai = seqItemSuperior,
                DescricaoPai = descricaoItemSuperior,
                ItemsHierarquiaOferta = itensHierarquiaOferta
            };

            return PartialView("_AssociarItemHierarquiaOferta", modelo);
        }

        /// <summary>
        /// Altera um item na arvore de hierarquia de oferta
        /// </summary>
        /// <param name="seqItemHierarquiaOferta"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_01_04.ASSOCIAR_ITEM_HIERARQUIA_OFERTA)]
        public ActionResult AlterarItemHierarquiaOferta(SMCEncryptedLong seqItemHierarquiaOferta)
        {
            var modelo = this.HierarquiaOfertaControllerService.BuscarItemHierarquiaOferta(seqItemHierarquiaOferta);
            modelo.ItemsHierarquiaOferta = this.ProcessoControllerService.BuscarTiposItemHierarquiaOfertaSelect(modelo.SeqProcesso,
                                                        modelo.SeqPai, false);
            modelo.PossuiIntegracao = ProcessoService.VerificaProcessoPossuiIntegracao(modelo.SeqProcesso);

            return PartialView("_AssociarItemHierarquiaOferta", modelo);
        }

        /// <summary>
        /// Excluir um item da árvore de hierarquia de oferta
        /// </summary>
        /// <param name="seqTipoHierarquiaOferta"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_04.ASSOCIAR_ITEM_HIERARQUIA_OFERTA)]
        public ActionResult ExcluirItemHierarquiaOferta(SMCEncryptedLong seqItemHierarquiaOferta, SMCEncryptedLong seqProcesso)
        {
            this.HierarquiaOfertaControllerService.ExcluirHierarquiaOferta(seqItemHierarquiaOferta);

            return SMCRedirectToAction(nameof(HierarquiaOferta), "HierarquiaOferta", new { seqProcesso });
        }

        #endregion Cadastrar Item Hierarquia

        #region Cadastrar Oferta

        /// <summary>
        /// Inclui uma oferta para um item na arvore de hierarquia de oferta
        /// </summary>
        /// <param name="seqItemSuperior"></param>
        /// <param name="descricaoItemSuperior"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_01_05.MANTER_OFERTA)]
        public ActionResult CadastrarOfertaHierarquiaOferta(SMCEncryptedLong seqItemSuperior, string descricaoItemSuperior, SMCEncryptedLong seqProcesso)
        {
            if (ProcessoService.VerificaProcessoPossuiIntegracao(seqProcesso))
            {
                return ThrowOpenModalException(Views.HierarquiaOferta.App_LocalResources.UIResource.Mensagem_Bloqueio_PossuiIntegracao);
            }

            var modelo = new OfertaViewModel()
            {
                SeqProcesso = seqProcesso,
                DescricaoPai = descricaoItemSuperior,
            };

            if (seqItemSuperior != null && seqItemSuperior.Value != default(long))
                modelo.SeqPai = seqItemSuperior;

            PreencherModeloOferta(modelo);

            var ultimaOfertaCadastrada = HierarquiaOfertaControllerService.BuscarUltimaOfertaCadastrada(seqProcesso);
            if (ultimaOfertaCadastrada != null)
            {
                modelo.DataInicio = ultimaOfertaCadastrada.DataInicio;
                modelo.DataFim = ultimaOfertaCadastrada.DataFim;
            }

            if (modelo.ItemsHierarquiaOferta.Count == 1)
            {
                modelo.SeqItemHierarquiaOferta = modelo.ItemsHierarquiaOferta.First().Seq;
            }

            return PartialView("_CadastrarOfertaHierarquiaOferta", modelo);
        }

        /// <summary>
        /// ALterar uma oferta para um item na arvore de hierarquia de oferta
        /// </summary>
        /// <param name="seqItemSuperior"></param>
        /// <param name="descricaoItemSuperior"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_01_05.MANTER_OFERTA)]
        public ActionResult AlterarOfertaHierarquiaOferta(SMCEncryptedLong seqOfertaHierarquiaOferta)
        {
            var modelo = this.HierarquiaOfertaControllerService.BuscarOfertaHierarquiaOferta(seqOfertaHierarquiaOferta);

            PreencherModeloOferta(modelo);

            if (modelo.Taxas.Count > 0)
            {
                modelo.TaxasOferta = new SMCMasterDetailList<TaxaOfertaViewModel>();
                modelo.TaxasOferta.AddRange(modelo.Taxas.Where(f => !f.SeqPermissaoInscricaoForaPrazo.HasValue));

                modelo.TaxasPermissoes = modelo.Taxas.Where(f => f.SeqPermissaoInscricaoForaPrazo.HasValue);
            }

            modelo.PossuiIntegracao = ProcessoService.VerificaProcessoPossuiIntegracao(modelo.SeqProcesso);

            return PartialView("_CadastrarOfertaHierarquiaOferta", modelo);
        }

        /// <summary>
        /// Salva a oferta de um item da hierarquia
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_05.MANTER_OFERTA)]
        public ActionResult SalvarOfertaHierarquiaOferta(OfertaViewModel modelo)
        {
            if (modelo.TaxasOferta != null)
                modelo.Taxas.AddRange(modelo.TaxasOferta);
            if (modelo.TaxasPermissoes != null)
                modelo.Taxas.AddRange(modelo.TaxasPermissoes);

          

            this.HierarquiaOfertaControllerService.SalvarOferta(modelo);

            return SMCRedirectToAction(nameof(HierarquiaOferta), "HierarquiaOferta", new { seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
        }

        /// <summary>
        /// Excluir um item da árvore de hierarquia de oferta
        /// </summary>
        /// <param name="seqTipoHierarquiaOferta"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_05.MANTER_OFERTA)]
        public ActionResult ExcluirOferta(SMCEncryptedLong seqItemHierarquiaOferta, SMCEncryptedLong seqProcesso)
        {
            this.HierarquiaOfertaControllerService.ExcluirOferta(seqItemHierarquiaOferta);

            return SMCRedirectToAction(nameof(HierarquiaOferta), "HierarquiaOferta", new { seqProcesso });
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_05.MANTER_OFERTA)]
        public ActionResult BuscarTaxaCobraPorQtdOferta(long? seqTaxa)
        {
            if (!seqTaxa.HasValue)
                return Json(false);

            return Json(HierarquiaOfertaControllerService.VerificaTipoTaxaCobraPorQtdOferta(seqTaxa.Value));
        }

        #endregion Cadastrar Oferta

        /// <summary>
        /// Preenche o modelo de oferta para cadastro
        /// </summary>
        /// <param name="modelo"></param>
        [NonAction]
        private void PreencherModeloOferta(OfertaViewModel modelo)
        {
            modelo.ItemsHierarquiaOferta = this.ProcessoControllerService
                .BuscarTiposItemHierarquiaOfertaSelect(modelo.SeqProcesso, modelo.SeqPai, true);
            modelo.GruposOferta = this.GrupoOfertaControllerService.BuscarGruposOfertasSelect(modelo.SeqProcesso);
            modelo.CodigosAutorizacaoSelect = this.CodigoAutorizacaoService
                .BuscarCodigosAutorizacaoPorProcessoSelect(modelo.SeqProcesso);
            modelo.TaxasSelect = this.ProcessoControllerService.BuscarTaxasOfertaSelect(modelo.SeqProcesso);
            modelo.EventosTaxasSelect = this.HierarquiaOfertaControllerService.BuscarEventosTaxaSelect(modelo.SeqProcesso);
            modelo.ParametrosCreiSelect = this.HierarquiaOfertaControllerService.BuscarDataVencimentoSelect(modelo.SeqProcesso);
            modelo.PossuiCalculoBolsaSocial = this.TipoProcessoControllerService.VerificaPossuiConsistencia(modelo.SeqProcesso, TipoConsistencia.CalculoBolsaSocial);
            modelo.BolsaExAluno = TipoProcessoControllerService.BuscarTipoProcessoPorProcesso(modelo.SeqProcesso).BolsaExAluno;
            modelo.HabilitaPercentualDesconto = TipoProcessoControllerService.BuscarTipoProcessoPorProcesso(modelo.SeqProcesso).HabilitaPercentualDesconto;
            modelo.HabilitaGestaoEventos = TipoProcessoService.BuscarTipoProcessoPorProcesso(modelo.SeqProcesso).GestaoEventos;
            // Monta a lista de tipo de telefone deixando todos os tipos
            if (modelo.TiposTelefone.SMCIsNullOrEmpty())
            {
                foreach (TipoTelefone item in Enum.GetValues(typeof(TipoTelefone)))
                {
                    if (item != TipoTelefone.Nenhum)
                    {
                        modelo.TiposTelefone.Add(new SMCDatasourceItem()
                        {
                            Descricao = SMCEnumHelper.GetDescription(item),
                            Seq = Convert.ToInt64(item)
                        });
                    }
                }
            }
        }

        [SMCAllowAnonymous]
        public JsonResult BuscarValorEvento(int seqEventoTaxa)
        {
            var valor = this.HierarquiaOfertaControllerService.BuscarValorEvento(seqEventoTaxa);
            return Json(valor);
        }

        [SMCAuthorize(UC_INS_001_01_08.SELECIONAR_OFERTAS_MANUTENCAO_TAXA_LOTE)]
        public ActionResult ListarOfertaPeriodoTaxa(OfertaTaxaFiltroViewModel filtro)
        {
            var modelo = this.HierarquiaOfertaControllerService.BuscarTaxasOferta(filtro);
            return PartialView("_ListarOfertaPeriodo", modelo);
        }

        [SMCAuthorize(UC_INS_001_01_08.SELECIONAR_OFERTAS_MANUTENCAO_TAXA_LOTE)]
        public ActionResult TaxaOfertaLote(OfertaTaxaFiltroViewModel filtro = null)
        {
            this.ProcessoControllerService.VerificarConsistenciaCadastroPeriodoTaxaEmLote(filtro.SeqProcesso);
            return SMCRedirectToAction("TaxasOferta", "HierarquiaOferta",
                new { seqProcesso = SMCDESCrypto.EncryptNumberForURL(filtro.SeqProcesso) });
        }

        [SMCAuthorize(UC_INS_001_01_08.SELECIONAR_OFERTAS_MANUTENCAO_TAXA_LOTE)]
        public ActionResult TaxasOferta(OfertaTaxaFiltroViewModel filtro = null)
        {
            if (filtro != null && filtro.Oferta != null && filtro.Oferta.Descricao != null)
                filtro.Oferta.Descricao = HttpUtility.HtmlDecode(filtro.Oferta.Descricao);

            filtro.GruposOferta = this.GrupoOfertaControllerService.BuscarGruposOfertasSelect(filtro.SeqProcesso);
            filtro.TiposTaxa = this.ProcessoControllerService.BuscarTaxasOfertaSelect(filtro.SeqProcesso);
            return View(filtro);
        }

        [SMCAuthorize(UC_INS_001_01_09.MANTER_PERIODO_TAXA)]
        public ActionResult CadastrarTaxaOferta(CadastroOfertaTaxaFiltroViewModel filtro = null)
        {
            filtro.Periodos = this.HierarquiaOfertaControllerService.BuscarPeriodosOfertas(filtro.SeqProcesso);
            filtro.TiposTaxa = this.ProcessoControllerService.BuscarTaxasOfertaSelect(filtro.SeqProcesso);
            filtro.GruposOferta = this.GrupoOfertaControllerService.BuscarGruposOfertasSelect(filtro.SeqProcesso);
            return View(filtro);
        }

        [SMCAuthorize(UC_INS_001_01_09.MANTER_PERIODO_TAXA)]
        public ActionResult ListarOfertasTaxa(CadastroOfertaTaxaFiltroViewModel filtro)
        {
            if (filtro.PageSettings == null)
                filtro.PageSettings = new SMCPageSetting();
            filtro.PageSettings.PageSize = int.MaxValue;
            //Buscar modelo de taxas
            var modelo = new CadastroTaxaOfertaLoteViewModel()
            {
                ItensAdicionar = HierarquiaOfertaControllerService.BuscarOfertasPeriodoTaxaParaInclusao(filtro),
                ItensExcluir = HierarquiaOfertaControllerService.BuscarOfertasPeriodoTaxaParaExclusao(filtro)
            };
            return PartialView("_AbasCadastroTaxaLote", modelo);
        }

        [SMCAuthorize(UC_INS_001_01_09.MANTER_PERIODO_TAXA)]
        public ActionResult ExcluirTaxaOfertas(OfertaTaxaSelecionadaViewModel modelo)
        {
            this.HierarquiaOfertaControllerService.ExcluirTaxaOfertaEmLote(modelo.SeqTipoTaxa, modelo.GridExcluir);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Lote,
                MessagesResource.Entidade_Taxas),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);
            return ListarOfertasTaxa(new CadastroOfertaTaxaFiltroViewModel
            {
                SeqProcesso = modelo.SeqProcesso,
                SeqTipoTaxa = modelo.SeqTipoTaxa,
                Periodo = modelo.Periodo
            });
        }

        [SMCAuthorize(UC_INS_001_01_09.MANTER_PERIODO_TAXA)]
        public ActionResult IncluirTaxasOferta(OfertaTaxaSelecionadaViewModel modelo)
        {
            var modeloInclusao = new IncluirTaxaOfertaViewModel()
            {
                SeqProcesso = modelo.SeqProcesso,
                SeqTipoTaxa = modelo.SeqTipoTaxa,
                SeqGrupoOferta = modelo.SeqGrupoOferta,
                Periodo = modelo.Periodo,
                Ofertas = this.HierarquiaOfertaControllerService.BuscarOfertasKeyValue(modelo.GridIncluir)
                                                                .OrderBy(o => o.Descricao).TransformList<OfertaKeyValue>(),
                TaxasSelect = this.ProcessoControllerService.BuscarTaxasOfertaSelect(modelo.SeqProcesso),
                EventosTaxasSelect = this.HierarquiaOfertaControllerService.BuscarEventosTaxaSelect(modelo.SeqProcesso),
                ParametrosCreiSelect = this.HierarquiaOfertaControllerService.BuscarDataVencimentoSelect(modelo.SeqProcesso)
            };

            //Define o valor padrão para o tipo de taxa
            modeloInclusao.Taxas.DefaultModel = new TaxaOfertaViewModel() { SeqTaxa = modelo.SeqTipoTaxa };
            ModelState.Clear();
            return PartialView("_IncluirTaxaEmLote", modeloInclusao);
        }

        [SMCAuthorize(UC_INS_001_01_09.MANTER_PERIODO_TAXA)]
        public ActionResult SalvarTaxasEmLote(IncluirTaxaOfertaViewModel modelo)
        {
            this.HierarquiaOfertaControllerService.IncluirTaxasLote(modelo);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Inclusao_Lote,
                MessagesResource.Entidade_Taxas),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);
            return ListarOfertasTaxa(new CadastroOfertaTaxaFiltroViewModel
            {
                SeqProcesso = modelo.SeqProcesso,
                SeqTipoTaxa = modelo.SeqTipoTaxa,
                SeqGrupoOferta = modelo.SeqGrupoOferta,
                Periodo = modelo.Periodo
            });
        }
    }
}