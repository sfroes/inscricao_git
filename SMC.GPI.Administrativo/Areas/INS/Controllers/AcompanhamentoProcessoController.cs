using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces;
using SMC.Formularios.UI.Mvc;
using SMC.Formularios.UI.Mvc.Model;
using SMC.Formularios.UI.Mvc.Models;
using SMC.Framework;
using SMC.Framework.Excel;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Ioc;
using SMC.Framework.Model;
using SMC.Framework.OpenXml;
using SMC.Framework.Rest;
using SMC.Framework.Security;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.UI.Mvc.Util;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.GPI.Administrativo.Areas.RES.Services;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using SMC.Localidades.ServiceContract.Areas.LOC.Data;
using SMC.Localidades.ServiceContract.Areas.LOC.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS
{
    public class AcompanhamentoProcessoController : SGFController
    {
        #region Serviços

        private ITipoProcessoService TipoProcessoService => Create<ITipoProcessoService>();
        private IProcessoService ProcessoService => Create<IProcessoService>();
        private IInscricaoDocumentoService InscricaoDocumentoService => Create<IInscricaoDocumentoService>();
        private IProcessoCampoInscritoService ProcessoCampoInscritoService => Create<IProcessoCampoInscritoService>();

        private AcompanhamentoProcessoControllerService AcompanhamentoProcessoControllerService
        {
            get { return this.Create<AcompanhamentoProcessoControllerService>(); }
        }

        private ClienteControllerService ClienteControllerService
        {
            get { return this.Create<ClienteControllerService>(); }
        }

        private UnidadeResponsavelControllerService UnidadeResponsavelControllerService
        {
            get { return this.Create<UnidadeResponsavelControllerService>(); }
        }

        private TipoProcessoControllerService TipoProcessoControllerService
        {
            get { return this.Create<TipoProcessoControllerService>(); }
        }

        private InscritoControllerService InscritoControllerService
        {
            get { return this.Create<InscritoControllerService>(); }
        }

        private GrupoOfertaControllerService GrupoOfertaControllerService
        {
            get { return this.Create<GrupoOfertaControllerService>(); }
        }

        private ProcessoControllerService ProcessoControllerService
        {
            get { return this.Create<ProcessoControllerService>(); }
        }

        private ILocalidadeService LocalidadeService
        {
            get { return Create<ILocalidadeService>(); }
        }

        private IInscricaoService InscricaoService => Create<IInscricaoService>();

        #endregion Serviços

        #region Consulta Posição Consolidada por Processo

        /// <summary>
        /// Exibe tela de listagem de processos
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_003_01_01.CONSULTA_POSICAO_CONSOLIDADA_POR_PROCESSO)]
        public ActionResult Index(ConsultaConsolidadaProcessoFiltroViewModel filtros = null)
        {
            filtros.Unidades = UnidadeResponsavelControllerService.BuscarUnidadesResponsaveisSelect();
            filtros.TiposProcessos = TipoProcessoControllerService.BuscarTiposProcessosSelect();
            filtros.Clientes = ClienteControllerService.BuscarClientesSelect();
            return View(filtros);
        }

        /// <summary>
        /// Action para listar os processos
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_003_01_01.CONSULTA_POSICAO_CONSOLIDADA_POR_PROCESSO)]
        public ActionResult ListarProcessoPosicaoConsolidada(ConsultaConsolidadaProcessoFiltroViewModel filtros, int? exportAction = null)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            if (!exportAction.HasValue)
            {
                var pager = AcompanhamentoProcessoControllerService.BuscarPosicaoConsolidadaProcessos(filtros);
                return PartialView("_ListarProcessoPosicaoConsolidada", pager);
            }
            else
            {
                filtros.PageSettings.PageSize = Int32.MaxValue;
                var modelo = AcompanhamentoProcessoControllerService.BuscarPosicaoConsolidadaProcessos(filtros);

                byte[] data = SMCGridExporter.ExportGridModelToExcel(this.ControllerContext, "PosicaoProcessos", modelo);
                return File(data, "application/download", "PosicaoConsolidadaProcessos.xlsx");
            }
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public ActionResult BuscarTiposProcesso(long? seqUnidadeResponsavel)
        {
            return Json(TipoProcessoControllerService.BuscarTiposProcessosSelect(seqUnidadeResponsavel));
        }

        #endregion Consulta Posição Consolidada por Processo

        #region Consulta Posição Consolidada por Grupo de Oferta e por Oferta

        [SMCAuthorize(UC_INS_003_01_02.CONSULTA_POSICAO_CONSOLIDADA_GRUPO_OFERTA_OFERTA)]
        public ActionResult ConsultarProcessoPorGrupoOferta(ConsultaConsolidadaGrupoOfertaFiltroViewModel filtro = null)
        {
            filtro.GrupoOfertas = GrupoOfertaControllerService.BuscarGruposOfertasSelect(filtro.SeqProcesso);
            filtro.Cabecalho = AcompanhamentoProcessoControllerService.BuscarPosicaoConsolidadaProcesso(filtro.SeqProcesso);
            return View(filtro);
        }

        [SMCAuthorize(UC_INS_003_01_02.CONSULTA_POSICAO_CONSOLIDADA_GRUPO_OFERTA_OFERTA)]
        public ActionResult ListarProcessoGrupoOferta(ConsultaConsolidadaGrupoOfertaFiltroViewModel filtros, int? exportAction = null)
        {
            if (CheckPostClearSubmit())
            {
                return DisplayFilterMessage();
            }

            if (!exportAction.HasValue)
            {
                var pager = AcompanhamentoProcessoControllerService.BuscarPosicaoConsolidadaPorGrupoOferta(filtros);
                return PartialView("_ListaProcessoGrupoOferta", pager);
            }
            else
            {
                filtros.PageSettings.PageSize = Int32.MaxValue;
                var modelo = AcompanhamentoProcessoControllerService.BuscarPosicaoConsolidadaPorGrupoOferta(filtros);
                List<List<string>> matriz = new List<List<string>>();
                foreach (var grupo in modelo)
                {
                    var linha = new List<string>();
                    linha.Add("Grupo de Oferta");
                    linha.Add(grupo.Descricao);
                    matriz.Add(linha);
                    var linhaCabecalho = new List<String>();
                    linhaCabecalho.Add("Descrição");
                    linhaCabecalho.Add("Iniciadas");
                    linhaCabecalho.Add("Finalizadas");
                    linhaCabecalho.Add("Pagas");
                    linhaCabecalho.Add("Documentação Entregue");
                    linhaCabecalho.Add("Confirmadas");
                    linhaCabecalho.Add("Não confirmadas");
                    linhaCabecalho.Add("Deferidas");
                    linhaCabecalho.Add("Indeferidas");
                    linhaCabecalho.Add("Canceladas");
                    matriz.Add(linhaCabecalho);
                    foreach (var oferta in grupo.PosicoesConsolidadasOfertas)
                    {
                        var linhaOferta = new List<String>();
                        linhaOferta.Add(oferta.Descricao);
                        linhaOferta.Add(oferta.Iniciadas.ToString());
                        linhaOferta.Add(oferta.Finalizadas.ToString());
                        linhaOferta.Add(oferta.Pagas.ToString());
                        linhaOferta.Add(oferta.DocumentacoesEntregues.ToString());
                        linhaOferta.Add(oferta.Confirmadas.ToString());
                        linhaOferta.Add(oferta.NaoConfirmadas.ToString());
                        linhaOferta.Add(oferta.Deferidos.ToString());
                        linhaOferta.Add(oferta.Indeferidos.ToString());
                        linhaOferta.Add(oferta.Canceladas.ToString());
                        matriz.Add(linhaOferta);
                    }
                    var linhaTotalGrupo = new List<String>();
                    linhaTotalGrupo.Add("Total Grupo");
                    linhaTotalGrupo.Add(grupo.Iniciadas.ToString());
                    linhaTotalGrupo.Add(grupo.Finalizadas.ToString());
                    linhaTotalGrupo.Add(grupo.Pagas.ToString());
                    linhaTotalGrupo.Add(grupo.DocumentacoesEntregues.ToString());
                    linhaTotalGrupo.Add(grupo.Confirmadas.ToString());
                    linhaTotalGrupo.Add(grupo.NaoConfirmadas.ToString());
                    linhaTotalGrupo.Add(grupo.Deferidos.ToString());
                    linhaTotalGrupo.Add(grupo.Indeferidos.ToString());
                    linhaTotalGrupo.Add(grupo.Canceladas.ToString());
                    matriz.Add(linhaTotalGrupo);
                    matriz.Add(new List<string> { "" });//Linha
                }
                var planilha = new string[matriz.Count, 10];
                for (int i = 0; i < matriz.Count; i++)
                {
                    for (int j = 0; j < matriz[i].Count; j++)
                    {
                        planilha[i, j] = matriz[i][j];
                    }
                }
                var dicMatriz = new Dictionary<string, string[,]>();
                dicMatriz.Add("PosicaoConsolidadaPorGrupo", planilha);

                //FIX: NAO PRECISA FAZER A MATRIZ PARA DEPOIS FAZER ESSA NOVA ESTRUTURA.
                //O IDEAL É JÁ CONVERTER NA NOVA ESTRUTRA
                var excel = SMCMatrixExcel.Convert(dicMatriz);
                var workBook = SMCIocContainer.Resolve<ISMCWorkbookService, byte[]>(work => work.RenderXlsFromMatrix(excel));

                return File(workBook, "application/download", "InscricoesProcesso.xlsx");
            }
        }

        #endregion Consulta Posição Consolidada por Grupo de Oferta e por Oferta

        #region Consulta de Inscrições do Processo

        /// <summary>
        /// Exibe tela de listagem de processos
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_003_01_03.CONSULTA_INSCRICOES_PROCESSO)]
        public ActionResult ConsultarInscricaoProcessoFiltroSGF(SMCEncryptedLong seqProcesso, SMCEncryptedLong oferta = null, SMCEncryptedLong seqGrupoOferta = null)
        {
            if (seqGrupoOferta == 0)
                seqGrupoOferta = null;

            if (oferta == 0)
                oferta = null;

            var model = new ConsultaInscricaoProcessoFiltroViewModel();
            model.FiltroSGF = new Formularios.UI.Mvc.Model.SGFFilterModel
            {
                SeqsFormularios = this.ProcessoControllerService.BuscarSeqsFormulariosDoProcesso(oferta, seqGrupoOferta, seqProcesso)
            };
            model.SeqProcesso = seqProcesso;

            return PartialView("_ConsultarInscricaoProfessoFiltroSGF", model);
        }

        /// <summary>
        /// Exibe tela de listagem de processos
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_003_01_03.CONSULTA_INSCRICOES_PROCESSO)]
        public ActionResult ConsultarInscricaoProcesso(ConsultaInscricaoProcessoFiltroViewModel filtros = null, SMCEncryptedLong seqOferta = null)
        {

            if (seqOferta != null)
            {
                filtros.Oferta = new GPILookupViewModel
                {
                    Seq = seqOferta
                };
            }

            filtros.SituacoesProcesso = this.ProcessoControllerService.BuscarSituacoesProcessoSelect(filtros.SeqProcesso);
            filtros.GruposOferta = this.GrupoOfertaControllerService.BuscarGruposOfertasSelect(filtros.SeqProcesso);
            filtros.BolsaExAluno = TipoProcessoService.BuscarTipoProcessoPorProcesso(filtros.SeqProcesso).BolsaExAluno;

            var taxasProcesso = ProcessoService.BuscarTaxasKeyValue(filtros.SeqProcesso);
            if (taxasProcesso.Any())
            {
                filtros.PossuiTaxa = true;
                filtros.TipoTaxa = taxasProcesso.OrderBy(t => t.Descricao).ToList();
            }

            //TODO: NV15: criar a regra para o campo: CheckinRealizado - Exibir este campo somente se o Tipo de processo do Processo em questão Habilita gestão de eventos.
            filtros.HabilitaGestaoEventos = ProcessoControllerService.BuscarProcesso(filtros.SeqProcesso).HabilitaGestaoEvento;            


            return View("ConsultarInscricaoProcesso", filtros);

        }

        [ChildActionOnly]
        [SMCAllowAnonymous]
        public ActionResult CabecalhoProcesso(long seqProcesso)
        {
            var modelo = this.AcompanhamentoProcessoControllerService.BuscarCabecalhoProcesso(seqProcesso);
            return PartialView("_CabecalhoProcesso", modelo);
        }

        /// <summary>
        /// Action para listar as inscrições nos processos
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_003_01_03.CONSULTA_INSCRICOES_PROCESSO)]
        public ActionResult ListarInscricaoProcesso(ConsultaInscricaoProcessoFiltroViewModel filtros = null, int? exportAction = null)
        {
            if (CheckPostClearSubmit())
            {
                return DisplayFilterMessage();
            }

            if (!exportAction.HasValue)
            {
                var modelo = this.AcompanhamentoProcessoControllerService.BuscarInscricoesProcesso(filtros);

                if (!string.IsNullOrEmpty(filtros.BackUrl))
                {
                    modelo.SMCForEach(f => f.BackURL = filtros.BackUrl);
                }

                return PartialView("_ListarInscricaoProcesso", modelo);
            }
            else
            {
                CultureInfo ptBR = new CultureInfo("pt-BR");
                filtros.PageSettings.PageSize = Int32.MaxValue;
                var modelo = this.AcompanhamentoProcessoControllerService.BuscarInscricoesProcessoExcel(filtros);
                List<List<string>> matriz = new List<List<string>>();
                var cabecalho = new List<string>();
                cabecalho.AddRange(new List<string> { "Nº Inscrição","Nome Inscrito","Cpf","Data de Nascimento","Grupo Oferta",
                    "Oferta","Data Inscrição","Situação", "Motivo", "Situação Documentação","Documentos Indeferidos","Documentos Pendentes","Pagamento Taxa","Valor", "Email", "Telefones", "Celular"});
                matriz.Add(cabecalho);
                foreach (var item in modelo)
                {
                    var linha = new List<string>
                    {
                        item.Seq.ToString(),
                        item.NomeInscrito,
                        item.Cpf,
                        item.DataNascimento.ToShortDateString(),
                        item.DescricaoGrupoOferta
                    };
                    string ofertas = "";
                    foreach (var oferta in item.OpcoesOferta)
                    {
                        ofertas += String.Format("{0}ª - opção: {1}\r\n", oferta.NumeroOpcao, oferta.DescricaoOferta);
                    }
                    linha.Add(ofertas);
                    linha.Add(item.DataInscricao.ToString());
                    linha.Add(item.DescricaoSituacaoAtual);
                    linha.Add(item.MotivoSituacaoAtual);
                    linha.Add(item.SituacaoDocumentacao.SMCGetDescription());
                    linha.Add(item.DocumentosIndeferidos);
                    linha.Add(item.DocumentosPendentes);
                    //linha.Add(item.DocumentacaoEntregue ? "Entregue" : "Pendente");
                    linha.Add(item.TaxaInscricaoPaga ? "Paga" : "Pendente");
                    linha.Add(item.ValorTitulo.ToString("C2", ptBR));
                    linha.Add(item.EmailInscrito);
                    string telefones = "";
                    string celular = null;
                    if (item.Telefones != null)
                    {
                        foreach (var telefone in item.Telefones)
                        {
                            if (!telefone.Numero.Contains("-"))
                            {
                                telefone.Numero = telefone.Numero.TrimEnd().Length == 9 ? telefone.Numero.Substring(0, 5) + "-" + telefone.Numero.Substring(5, 4)
                                    : telefone.Numero.Substring(0, 4) + "-" + telefone.Numero.Substring(4, 4);
                            }

                            string numeroFormatado = string.Format("{0}: +{1} ({2}) {3}\r\n", SMCEnumHelper.GetDescription((TipoTelefone)telefone.TipoTelefone)
                                , telefone.CodigoPais, telefone.CodigoArea, telefone.Numero);

                            telefones += numeroFormatado;

                            if (celular == null && telefone.TipoTelefone == TipoTelefone.Celular)
                            {
                                celular = numeroFormatado;
                            }
                        }
                    }
                    linha.Add(telefones);
                    if (celular != null)
                        linha.Add(celular);
                    matriz.Add(linha);
                }
                var planilha = new string[matriz.Count, cabecalho.Count];
                for (int i = 0; i < matriz.Count; i++)
                {
                    for (int j = 0; j < matriz[i].Count; j++)
                    {
                        planilha[i, j] = matriz[i][j];
                    }
                }
                var dicMatriz = new Dictionary<string, string[,]>();
                dicMatriz.Add("InscricoesProcesso", planilha);

                //FIX: NAO PRECISA FAZER A MATRIZ PARA DEPOIS FAZER ESSA NOVA ESTRUTURA.
                //O IDEAL É JÁ CONVERTER NA NOVA ESTRUTRA
                var excel = SMCMatrixExcel.Convert(dicMatriz);
                var workBook = SMCIocContainer.Resolve<ISMCWorkbookService, byte[]>(work => work.RenderXlsFromMatrix(excel));

                return File(workBook, "application/download", "InscricoesProcesso.xlsx");
            }
        }

        [SMCAuthorize(UC_INS_003_01_03.CONSULTA_INSCRICOES_PROCESSO)]
        public JsonResult BuscarMotivos(long seqTipoProcessoSituacao)
        {
            var motivos = this.AcompanhamentoProcessoControllerService.BuscarMotivosSituacao(seqTipoProcessoSituacao);
            return Json(motivos);
        }

        [SMCAuthorize(UC_INS_003_01_06.EXCLUIR_INSCRICAO)]
        public ActionResult ExcluirInscricao(ExcluirConsultaInscricaoProcessoFiltroViewModel model)
        {
            try
            {

                if (AcompanhamentoProcessoControllerService.PossuiBoletoPago(model.SeqInscricao.Value))
                    throw new InscricaoComBoletoPagoException();

                Assert("ExcluirInscricao", model, string.Format(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Confirmacao_Exclusao_Inscricao, model.NomeInscrito), () =>
                 {
                     return true;
                 });

                this.InscricaoService.ExcluirInscricao(model.SeqInscricao.Value);

                SetSuccessMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Sucesso_Exclusao_Inscricao,
               MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);

                if (!string.IsNullOrEmpty(model.BackUrl))
                {
                    return SMCRedirectToUrl(model.BackUrl);
                }
                return SMCRedirectToAction("ConsultarInscricaoProcesso", routeValues: new ConsultaInscricaoProcessoFiltroViewModel() { SeqProcesso = model.SeqProcesso });
            }
            catch (SMCApplicationException e)
            {
                SetErrorMessage(e.Message, MessagesResource.Titulo_Erro, SMCMessagePlaceholders.Centro);
                return BackToAction();
            }
        }

        #endregion Consulta de Inscrições do Processo

        #region Análise de Inscrição em Lote

        /// <summary>
        /// Exibe tela de listagem de processos
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_003_01_03.CONSULTA_INSCRICOES_PROCESSO)]
        public ActionResult AnalisarInscricaoLoteFiltroSGF(SMCEncryptedLong seqProcesso, SMCEncryptedLong oferta = null, SMCEncryptedLong seqGrupoOferta = null)
        {
            if (seqGrupoOferta == 0)
                seqGrupoOferta = null;

            if (oferta == 0)
                oferta = null;

            var model = new AnaliseInscricaoLoteFiltroViewModel();
            model.FiltroSGF = new SGFFilterModel
            {
                SeqsFormularios = this.ProcessoControllerService.BuscarSeqsFormulariosDoProcesso(oferta, seqGrupoOferta, seqProcesso)
            };
            model.SeqProcesso = seqProcesso;

            return PartialView("_AnalisarInscricaoLoteFiltroSGF", model);
        }

        /// <summary>
        /// Exibe tela de análise de inscricao em lote
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_003_01_04.ANALISE_INSCRICAO_LOTE)]
        public ActionResult AnalisarInscricaoLote(AnaliseInscricaoLoteFiltroViewModel filtros = null)
        {
            filtros.SituacoesProcesso = this.ProcessoControllerService.BuscarSituacoesProcessoSelect(filtros.SeqProcesso);
            filtros.GruposOferta = this.GrupoOfertaControllerService.BuscarGruposOfertasSelect(filtros.SeqProcesso);

            return View("AnalisarInscricaoLote", filtros);
        }

        /// <summary>
        /// Action para listar as inscrições nos processos
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_003_01_04.ANALISE_INSCRICAO_LOTE)]
        public ActionResult ListarAnaliseInscricaoLote(AnaliseInscricaoLoteFiltroViewModel filtros)
        {
            if (CheckPostClearSubmit())
            {
                return DisplayFilterMessage();
            }

            SMCPagerModel<AnaliseInscricaoLoteListaViewModel> modelo = new SMCPagerModel<AnaliseInscricaoLoteListaViewModel>();
            // Se os filtros obrigatórios foram informados
            // if (filtros.SeqSituacao > 0 && filtros.SeqGrupoOferta > 0 && filtros.Oferta.Seq > 0)
            //   {
            modelo = this.AcompanhamentoProcessoControllerService.BuscarInscricoesProcesso(filtros);
            // }
            return PartialView("_ListarAnaliseInscricaoLote", modelo);
        }

        #endregion Análise de Inscrição em Lote

        #region Alteração de Situação

        [SMCAuthorize(UC_INS_003_01_08.ALTERACAO_SITUACAO)]
        public ActionResult ListarAlterarSituacaoLote(InscricaoSelecionadaViewModel modelo)
        {
            if (AcompanhamentoProcessoControllerService.VerificaSituacaoInscricaoDiferenteCandidatoConfirmado(modelo.GridAnaliseInscricaoLote))
                return ThrowOpenModalException(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Erro_AlterarSituacao_CandidatoConfirmado_Lote);

            var inscricoes = this.AcompanhamentoProcessoControllerService.BuscarInscricoesAlteracaoSituacao(modelo);

            return PartialView("_ListarAlteracaoSituacao", inscricoes);
        }

        [SMCAuthorize(UC_INS_003_01_08.ALTERACAO_SITUACAO)]
        public ActionResult ListarAlterarSituacao(SMCEncryptedLong seqInscricao, SMCEncryptedLong seqSituacao, SMCEncryptedLong seqProcesso, string backUrl)
        {
            var modelo = new InscricaoSelecionadaViewModel
            {
                SeqTipoProcessoSituacao = seqSituacao,
                SeqProcesso = seqProcesso,
                GridAnaliseInscricaoLote = new List<long> { seqInscricao.Value },


            };

            if (AcompanhamentoProcessoControllerService.VerificaSituacaoInscricaoDiferenteCandidatoConfirmado(modelo.GridAnaliseInscricaoLote))
                return ThrowOpenModalException(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Erro_AlterarSituacao_CandidatoConfirmado);

            var inscricoes = this.AcompanhamentoProcessoControllerService.BuscarInscricoesAlteracaoSituacao(modelo);
            inscricoes.BackUrl = backUrl;
            return PartialView("_ListarAlteracaoSituacao", inscricoes);
        }

        [SMCAuthorize(UC_INS_003_01_08.ALTERACAO_SITUACAO)]
        public ActionResult SalvarAlteracaoSituacaoLote(AlteracaoSituacaoViewModel modelo)
        {
            // UC_INS_003_01_08
            /*1. Ao clicar em salvar, alterar a regra da mensagem de confirmação para:
                Se o tipo de processo do processo em questão estiver configurado para integrar com o SGA legado e a situação estiver sendo alterada para "Inscrição Deferida",
                emitir a mensagem de confirmação:

                "Essa alteração de situação exportará o(s) candidato(s) selecionado(s) para o SGA e a operação não poderá ser desfeita. Além disso, será enviado um e-mail de notificação automático para o(s) candidato(s), caso isso tenha sido configurado no processo. Confirma a alteração da situação?

                Senão, emitir a mensagem de confirmação abaixo:

                "Essa alteração de situação enviará um e-mail de notificação automático para o(s) candidato(s) selecionado(s), caso isso tenha sido configurado no processo. Confirma a alteração da situação?"

                Caso o usuário clique em "Sim", prosseguir com a alteração. Caso o usuário clique em "Não", abortar a operação.*/

            var integrarLegado = ProcessoService.VerificarIntegracaoLegado(modelo.SeqProcesso);
            TipoProcessoSituacaoData situacaoAlterar = null;

            if (modelo.SeqTipoProcessoSituacaoDestino != -1 && integrarLegado)
                situacaoAlterar = TipoProcessoService.BuscarTipoProcessoSituacao(modelo.SeqTipoProcessoSituacaoDestino);

            Assert(modelo, "MSG_AlterarSituacaoInscricao_Integracao_SGA", () => situacaoAlterar != null && situacaoAlterar.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA && integrarLegado);
            Assert(modelo, "MSG_AlterarSituacaoInscricao_Sem_Integracao_SGA", () => !(situacaoAlterar != null && situacaoAlterar.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA && integrarLegado));

            this.AcompanhamentoProcessoControllerService.SalvarAlteracaoSituacaoLote(modelo);

            SetSuccessMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Sucesso_Alteracao_Situacao, MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);
            if (string.IsNullOrEmpty(modelo.BackUrl))
            {
                if (modelo.Lote)
                    return SMCRedirectToAction("AnalisarInscricaoLote", "AcompanhamentoProcesso", new { area = "INS", seqProcesso = new SMCEncryptedLong(modelo.SeqProcesso) });
                else
                    return SMCRedirectToAction("ConsultarInscricaoProcesso", "AcompanhamentoProcesso", new { area = "INS", seqProcesso = new SMCEncryptedLong(modelo.SeqProcesso) });
            }
            else
            {
                return SMCRedirectToUrl(modelo.BackUrl);
            }
        }

        [SMCAuthorize(UC_INS_003_01_08.ALTERACAO_SITUACAO)]
        public JsonResult BuscarMotivosSituacao(long seqTipoProcessoSituacaoDestino)
        {
            var motivos = this.AcompanhamentoProcessoControllerService.BuscarMotivosSituacao(seqTipoProcessoSituacaoDestino);

            return Json(motivos);
        }

        #endregion Alteração de Situação

        #region Historio de Situação

        [SMCAuthorize(UC_INS_003_01_10.PESQUISAR_HISTORICO_SITUACAO)]
        public ActionResult HistoricoSituacao(HistoricoSituacaoFiltroViewModel filtro)
        {
            return View(filtro);
        }

        [ChildActionOnly]
        public ActionResult CabecalhoHistoricoSituacao(HistoricoSituacaoFiltroViewModel filtro)
        {
            HistoricoSituacaoCabecalhoViewModel model = AcompanhamentoProcessoControllerService.
                        BuscarCabecalhoProcesso(filtro.SeqProcesso).Transform<HistoricoSituacaoCabecalhoViewModel>();
            var inscrito = InscritoControllerService.BuscarNomesDadosInscrito(filtro.SeqInscrito);
            model.Inscrito = (inscrito.NomeSocial != null) ? inscrito.NomeSocial + " (" + inscrito.Nome + ")" : inscrito.Nome;
            return PartialView("_CabecalhoHistoricoSituacao", model);
        }

        [SMCAuthorize(UC_INS_003_01_10.PESQUISAR_HISTORICO_SITUACAO)]
        public ActionResult ListarHistoricoSituacao(HistoricoSituacaoFiltroViewModel filtro)
        {
            List<HistoricoSituacaoListaViewModel> model = AcompanhamentoProcessoControllerService.BuscarSituacoesInscricao(filtro.SeqInscricao);
            return PartialView("_ListarHistoricoSituacao", new SMCPagerModel<HistoricoSituacaoListaViewModel>(model));
        }

        [SMCAuthorize(UC_INS_003_01_11.MANTER_HISTORICO_SITUACAO)]
        public ActionResult EditarHistoricoSituacao(SMCEncryptedLong seqHistoricoSituacao, SMCEncryptedLong seqInscrito, SMCEncryptedLong seqInscricao, SMCEncryptedLong seqProcesso, string backUrl)
        {

            HistoricoSituacaoViewModel model = AcompanhamentoProcessoControllerService.BuscarHistoricoSituacao(seqHistoricoSituacao);
            model.BackURL = backUrl;
            model.SeqInscricao = seqInscricao;
            PreencherHistoricoSituacao(model);

            model.FiltroCabecalho = new HistoricoSituacaoFiltroViewModel()
            {
                SeqInscricao = seqInscricao,
                SeqInscrito = seqInscrito,
                SeqProcesso = seqProcesso
            };

            return View(model);
        }

        [SMCAuthorize(UC_INS_003_01_11.MANTER_HISTORICO_SITUACAO)]
        public ActionResult SalvarAlteracaoHistoricoSituacao(HistoricoSituacaoViewModel model, HistoricoSituacaoFiltroViewModel filtro)
        {
            return this.Save(model, AcompanhamentoProcessoControllerService.SalvarAlteracaoSituacao,
                    "HistoricoSituacao", null, "EditarHistoricoSituacao",
                    routeValues: new { seqInscrito = (SMCEncryptedLong)filtro.SeqInscrito, seqInscricao = (SMCEncryptedLong)filtro.SeqInscricao, seqProcesso = (SMCEncryptedLong)filtro.SeqProcesso, origem = filtro.Origem, backURL = model.BackURL },
                    preencherModelo: PreencherHistoricoSituacao);
        }

        private void PreencherHistoricoSituacao(HistoricoSituacaoViewModel model)
        {
            model.Motivos = this.AcompanhamentoProcessoControllerService.BuscarMotivosSituacao(model.SeqSituacao);
        }

        #endregion Historio de Situação

        #region Consulta Inscrito

        [SMCAuthorize(UC_INS_003_01_06.CONSULTA_INSCRITO)]
        public ActionResult ConsultaInscricao(InscricaoViewModel model)
        {
            this.AcompanhamentoProcessoControllerService.PreencherInscricaoRelatorio(model);
            return View(model);
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_003_01_06.CONSULTA_INSCRITO)]
        public ActionResult CarregarDocumento(SMCEncryptedLong seqArquivo)
        {
            SMCUploadFile Arquivo = AcompanhamentoProcessoControllerService.BuscarArquivo(seqArquivo);
            if (Arquivo.FileData == null)
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + "\"" + Arquivo.Name + "_expurgo.pdf\"");
                return File(CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF, "application/pdf");
            }

            Response.AppendHeader("Content-Disposition", "inline; filename=" + "\"" + Arquivo.Name + "\"");
            return File(Arquivo.FileData, Arquivo.Type);
        }

        [SMCAuthorize(UC_INS_003_01_06.CONSULTA_INSCRITO)]
        public ActionResult BoletoTitulo(SMCEncryptedLong id, int? pdf = 1)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["ApiBoleto"]))
                {
                    var modelo = this.AcompanhamentoProcessoControllerService.BuscarTitulo((int)id.Value);
                    if (pdf == 0)
                    {
                        return View(modelo);
                    }
                    return RenderPdfView(modelo, new GridOptions(proxy: "http://proxy.pucminas.br:80") { PageMargins = new MarginInfo { Left = 10, Right = 10, Top = 10, Bottom = 10 } });
                }

                var filtro = new { SeqTitulo = (long)id, Html = pdf == 0, Token = SMCDESCrypto.Encrypt(ConfigurationManager.AppSettings[WEB_API_REST.TOKEN_BOLETO_KEY]) };
                var url = $"{ConfigurationManager.AppSettings[WEB_API_REST.BASE_URL_KEY]}{WEB_API_REST.EMITIR_BOLETO_INSCRITO}";
                var boleto = SMCRest.PostJson(url, filtro, cancellationTimer: int.Parse(ConfigurationManager.AppSettings[WEB_API_REST.CANCELLATION_TIME_KEY]));
                return File(Convert.FromBase64String(boleto), "application/pdf");
            }
            catch (Exception e)
            {
                this.SetErrorMessage(e.Message);
                return BackToAction();
            }
        }

        #endregion Consulta Inscrito

        #region Registro de Documentação Entregue

        [HttpGet]
        [SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        public ActionResult ConsultaRegistroDocumentacao(SMCEncryptedLong seqInscricao, string backURL, string origem = null)
        {
            try
            {
                RegistroDocumentacaoViewModel modelo = AcompanhamentoProcessoControllerService.BuscarRegistroDocumentacao(seqInscricao);

                modelo.Origem = origem;
                //Passo a Url de retorno levando em consideração quem chamou a tela
                modelo.DocumentosEntregues.BackURL = backURL;

                return View(modelo);
            }
            catch (SMCApplicationException e)
            {
                SetErrorMessage(e.Message);
                return BackToAction();
            }
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        public ActionResult SalvarDocumento(DocumentoEntregueViewModel documento)
        {
            Assert(documento, Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Confirmacao_Reverter_Situacao, () =>
            {
                if (documento.SituacaoEntregaDocumentoAntiga == SituacaoEntregaDocumento.Deferido && documento.SituacaoEntregaDocumento != SituacaoEntregaDocumento.Deferido)
                {
                    return AcompanhamentoProcessoControllerService.VerificaHistoricoInscricaoConfirmada(documento.Seq, documento.SeqInscricao);
                }
                return false;
            });

            Assert(documento, Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Confirmacao_Inconsistencia_Situacao, () =>
            {
                if (documento.SituacaoEntregaDocumentoAntiga == SituacaoEntregaDocumento.Deferido && documento.SituacaoEntregaDocumento != SituacaoEntregaDocumento.Deferido)
                {
                    return AcompanhamentoProcessoControllerService.VerificarHistoricoInscricaoConfirmadaEAvancada(documento.Seq, documento.SeqInscricao);
                }
                return false;
            });

            if (documento.ArquivoAnexado != null && documento.ArquivoAnexado.State == SMCUploadFileState.Changed)
            {
                documento.ArquivoAnexado.FileData = SMCUploadHelper.GetFileData(documento.ArquivoAnexado);
            }
            documento = this.AcompanhamentoProcessoControllerService.SalvarDocumentoInscricao(documento);

            //var situacaoEntraga = documento.DocumentacaoEntregue;
            //if(situacaoEntraga != documento.DocumentacaoEntregue){
            //    return SMCRedirectToAction("ConsultaRegistroDocumentacao","AcompanhamentoProcesso",
            //        new { @seqInscricao = SMCDESCrypto.EncryptNumberForURL(documento.SeqInscricao)});
            //}
            return PartialView("_DocumentoEntregue", documento);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        public ActionResult SalvarDocumentosEntregues(SumarioDocumentosEntreguesViewModel documentos)
        {
            try
            {
                VincularInscricaoDocumentoNovo(documentos);
                VincularFileData(documentos);

                Assert(documentos, Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Confirmacao_Situacao_Anterior, () =>
                {
                    return AcompanhamentoProcessoControllerService.ValidarSituacaoAtualCandidatoOfertasConfirmadas(documentos);
                });

                Assert(documentos, Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Confirmacao_Desfazer_Entrega, () =>
                {
                    return AcompanhamentoProcessoControllerService.ValidarSituacaoAtualCandidatoOfertasDeferidas(documentos);
                });

                var seqProcesso = AcompanhamentoProcessoControllerService.SalvarSumarioDocumentosEntreguesInscricao(documentos);

                SetSuccessMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Sucesso_Entrega_Documentos);

                if (!string.IsNullOrEmpty(documentos.Origem) && documentos.Origem.Equals("AcompanhamentoInscrito") && !string.IsNullOrEmpty(documentos.BackURL))
                {
                    return SMCRedirectToUrl(documentos.BackURL);
                }
                else if (documentos.Sair)
                {
                    return SMCRedirectToAction("ConsultarInscricaoProcesso", "AcompanhamentoProcesso", new ConsultaInscricaoProcessoFiltroViewModel() { SeqProcesso = new SMCEncryptedLong(seqProcesso) });

                }
                else
                {
                    return SMCRedirectToAction("ConsultaRegistroDocumentacao", "AcompanhamentoProcesso", new { seqInscricao = new SMCEncryptedLong(documentos.SeqInscricao), backURL = documentos.BackURL });
                }
            }
            catch (Exception ex)
            {
                throw new SMCApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Método utilizado para Manter os vínculos dos documentos inseridos, com a inscrição e o documento Requerido
        /// </summary>
        /// <param name="documentos"></param>
        /// <returns></returns>
        private void VincularInscricaoDocumentoNovo(SumarioDocumentosEntreguesViewModel documentos)
        {
            documentos.DocumentosObrigatorios = documentos.DocumentosObrigatorios ?? new List<DocumentoRequeridoEntregueViewModel>();
            documentos.DocumentosOpcionais = documentos.DocumentosOpcionais ?? new List<DocumentoRequeridoEntregueViewModel>();
            documentos.GruposDocumentos = documentos.GruposDocumentos ?? new List<GrupoDocumentoEntregueViewModel>();

            var todosDocumentosRequeridos = documentos?.DocumentosObrigatorios?.ToList();
            todosDocumentosRequeridos.AddRange(documentos?.DocumentosOpcionais?.ToList());
            todosDocumentosRequeridos.AddRange(documentos?.GruposDocumentos?.SelectMany(d => d.DocumentosRequeridosGrupo).ToList());

            if (todosDocumentosRequeridos.Any(d => d.InscricaoDocumentos.Any(i => i.Seq == 0)))
            {
                foreach (var documentoRequerido in todosDocumentosRequeridos.Where(d => d.InscricaoDocumentos.Any(i => i.Seq == 0)).ToList())
                {
                    foreach (var documentoInscricao in documentoRequerido.InscricaoDocumentos.Where(d => d.Seq == 0).ToList())
                    {
                        documentoInscricao.SeqDocumentoRequerido = documentoRequerido.Seq;
                        documentoInscricao.SeqInscricao = documentoRequerido.SeqInscricao;
                        if (documentoRequerido.PermiteVarios)
                            documentoInscricao.EntregaPosterior = false;
                    }
                }
            }
        }

        #region [ Vincular FileData ]

        private void VincularFileData(SumarioDocumentosEntreguesViewModel documentos)
        {
            VincularFileData(documentos.DocumentosObrigatorios);
            VincularFileData(documentos.GruposDocumentos);
            VincularFileData(documentos.DocumentosOpcionais);
        }

        private void VincularFileData(List<GrupoDocumentoEntregueViewModel> gruposDocumentos)
        {
            foreach (var grupo in gruposDocumentos)
            {
                VincularFileData(grupo.DocumentosRequeridosGrupo);
            }
        }

        private void VincularFileData(List<DocumentoRequeridoEntregueViewModel> documentos)
        {
            foreach (var documento in documentos)
            {
                VincularFileData(documento);
            }
        }

        private void VincularFileData(DocumentoRequeridoEntregueViewModel documento)
        {
            foreach (var inscricaoDocumento in documento.InscricaoDocumentos)
            {
                VincularFileData(inscricaoDocumento);
            }
        }

        private void VincularFileData(InscricaoDocumentoViewModel inscricaoDocumento)
        {
            if (inscricaoDocumento.ArquivoAnexado != null && inscricaoDocumento.ArquivoAnexado.State == SMCUploadFileState.Changed)
            {
                inscricaoDocumento.ArquivoAnexado.FileData = SMCUploadHelper.GetFileData(inscricaoDocumento.ArquivoAnexado);
            }
        }

        #endregion [ Vincular FileData ]

        [ChildActionOnly]
        public ActionResult ListarDocumentosEntregues(long seqInscricao)
        {
            var modelo = AcompanhamentoProcessoControllerService.BuscarDocumentosEntreguesInscricao(seqInscricao);
            return PartialView("_ListaDocumentosEntregues", modelo);
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        public ActionResult DuplicarDocumentoEntregue(SMCEncryptedLong seqInscricaoDocumento, SMCEncryptedLong seqInscricao)
        {
            AcompanhamentoProcessoControllerService.DuplicarDocumentoEntregue(seqInscricaoDocumento);
            return ListarDocumentosEntregues(seqInscricao);
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        public ActionResult ExcluirDocumento(SMCEncryptedLong seqInscricaoDocumento, SMCEncryptedLong seqInscricao)
        {
            AcompanhamentoProcessoControllerService.ExcluirInscricaoDocumento(seqInscricaoDocumento);
            return ListarDocumentosEntregues(seqInscricao);
        }

        [SMCAllowAnonymous]
        public ActionResult DownloadDocumento(string guidFile, string name, string type)
        {
            if (Guid.TryParse(guidFile, out Guid guid))
            {
                var data = SMCUploadHelper.GetFileData(new SMCUploadFile { GuidFile = guidFile });
                if (data != null)
                {
                    return File(data, type, name);
                }
            }
            return CarregarDocumento(new SMCEncryptedLong(guidFile));
        }

        [SMCAllowAnonymous]
        public ActionResult DownloadDocumentos(long seqInscricao)
        {
            if (seqInscricao > 0)
            {
                SMCFile zip = AcompanhamentoProcessoControllerService.DownloadDocumentos(seqInscricao);
                if (zip != null)
                {
                    return File(zip.Conteudo, System.Net.Mime.MediaTypeNames.Application.Zip, zip.Nome);
                }
                else
                {
                    SetErrorMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Documentacao_Vazia, target: SMCMessagePlaceholders.Centro);
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            return null;
        }

        #region [ Dependency]

        //[SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        //public ActionResult PreencherDataEntrega(SituacaoEntregaDocumento situacaoEntregaDocumento, DateTime? dataEntrega, SMCUploadFile arquivoAnexado)
        //{
        //    if (situacaoEntregaDocumento != SituacaoEntregaDocumento.AguardandoEntrega && situacaoEntregaDocumento != SituacaoEntregaDocumento.Pendente)
        //    {
        //        if (dataEntrega == null || !dataEntrega.HasValue)
        //            dataEntrega = DateTime.Now.Date;
        //    }
        //    else if (situacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoEntrega || situacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)
        //    {
        //        if (arquivoAnexado == null)
        //            dataEntrega = null;
        //    }

        //    return Content(dataEntrega.HasValue ? dataEntrega.Value.ToString("MM/dd/yyyy") : string.Empty);
        //}

        //[SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        //public ActionResult PreencherVersaoDocumento(SituacaoEntregaDocumento situacaoEntregaDocumento, VersaoDocumento? versaoDocumento, VersaoDocumento VersaoDocumentoExigido, SMCUploadFile arquivoAnexado)
        //{
        //    versaoDocumento = versaoDocumento ?? VersaoDocumento.Nenhum;
        //    var listaVersaoDocumento = new List<SMCSelectListItem>();
        //    // Valores: "Original", "Cópia simples" e "Cópia autenticada".
        //    var listaPermitida = new VersaoDocumento[] { VersaoDocumento.CopiaAutenticada, VersaoDocumento.CopiaSimples, VersaoDocumento.Original };

        //    foreach (var item in listaPermitida)
        //    {
        //        listaVersaoDocumento.Add(new SMCSelectListItem() { Text = SMCEnumHelper.GetDescription(item), Value = ((int)item).ToString() });
        //    }

        //    if (situacaoEntregaDocumento != SituacaoEntregaDocumento.AguardandoEntrega && situacaoEntregaDocumento != SituacaoEntregaDocumento.Pendente)
        //    {
        //        if ((versaoDocumento == null || versaoDocumento == VersaoDocumento.Nenhum) && VersaoDocumentoExigido != VersaoDocumento.Nenhum)
        //        {
        //            listaVersaoDocumento.FirstOrDefault(l => l.Value == ((int)VersaoDocumentoExigido).ToString()).Selected = true;
        //        }
        //        else if ((versaoDocumento == null || versaoDocumento == VersaoDocumento.Nenhum) && VersaoDocumentoExigido == VersaoDocumento.Nenhum)
        //        {
        //            return Json(listaVersaoDocumento);
        //        }
        //        else
        //        {
        //            listaVersaoDocumento.FirstOrDefault(l => l.Value == ((int)versaoDocumento).ToString()).Selected = true;
        //        }
        //    }
        //    else if (situacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoEntrega || situacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)
        //    {
        //        if (arquivoAnexado != null)
        //        {
        //            if ((versaoDocumento == null || versaoDocumento == VersaoDocumento.Nenhum) && VersaoDocumentoExigido != VersaoDocumento.Nenhum)
        //            {
        //                listaVersaoDocumento.FirstOrDefault(l => l.Value == ((int)VersaoDocumentoExigido).ToString()).Selected = true;
        //            }
        //            else
        //            {
        //                listaVersaoDocumento.FirstOrDefault(l => l.Value == ((int)versaoDocumento).ToString()).Selected = true;
        //            }
        //        }
        //    }

        //    return Json(listaVersaoDocumento);
        //}

        //[SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        //public ActionResult PreencherFormaEntregaDocumento(SituacaoEntregaDocumento situacaoEntregaDocumento, FormaEntregaDocumento? formaEntregaDocumento, SMCUploadFile arquivoAnexado)
        //{
        //    var listaFormaEntregaDocumento = new List<SMCSelectListItem>();
        //    // Valores: "Correios", "E-mail",  "Presencial" e "Upload".
        //    var listaPermitida = new FormaEntregaDocumento[] { FormaEntregaDocumento.Correios, FormaEntregaDocumento.Email, FormaEntregaDocumento.Presencial, FormaEntregaDocumento.Upload };
        //    foreach (var item in listaPermitida)
        //    {
        //        listaFormaEntregaDocumento.Add(new SMCSelectListItem() { Text = SMCEnumHelper.GetDescription(item), Value = ((int)item).ToString() });
        //    }

        //    if (situacaoEntregaDocumento != SituacaoEntregaDocumento.AguardandoEntrega && situacaoEntregaDocumento != SituacaoEntregaDocumento.Pendente)
        //    {
        //        if (formaEntregaDocumento != null)
        //            listaFormaEntregaDocumento.FirstOrDefault(l => l.Value == ((int)formaEntregaDocumento).ToString()).Selected = true;
        //    }
        //    else if (situacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoEntrega || situacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)
        //    {
        //        if (arquivoAnexado != null)
        //        {
        //            if (formaEntregaDocumento != null)
        //                listaFormaEntregaDocumento.FirstOrDefault(l => l.Value == ((int)formaEntregaDocumento).ToString()).Selected = true;
        //        }
        //    }

        //    return Json(listaFormaEntregaDocumento);
        //}

        //[SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        //public ActionResult PreencherObservacao(SituacaoEntregaDocumento situacaoEntregaDocumento, string observacao, SMCUploadFile arquivoAnexado)
        //{
        //    if (situacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoEntrega || situacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)
        //    {
        //        if (arquivoAnexado == null)
        //            observacao = string.Empty;
        //    }

        //    return Content(observacao);
        //}

        [SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        public JsonResult BuscarSituacoesEntregaDocumento(long seqInscricao, long seqDocumentoRequerido, SituacaoEntregaDocumento? situacaoEntregaDocumento, SituacaoEntregaDocumento? situacaoEntregaDocumentos)
        {
            // Caso não tenha sido passado nenhuma situação atual, considera aguardando entrega
            situacaoEntregaDocumento = situacaoEntregaDocumento ?? SituacaoEntregaDocumento.AguardandoEntrega;

            var itens = InscricaoDocumentoService.BuscarSituacoesEntregaDocumento(seqInscricao, seqDocumentoRequerido);
            itens.Where(i => i.Seq == (situacaoEntregaDocumento ?? situacaoEntregaDocumentos ?? SituacaoEntregaDocumento.Nenhum).ToString()).SMCForEach(i => i.Selected = true);

            // Caso não retorne nenhum item, cria um item com o valor que chega do situacaoEntregaDocumento
            if (itens.Count == 0)
                itens.Add(new SMCDatasourceItem<string> { Selected = true, Descricao = SMCEnumHelper.GetDescription(situacaoEntregaDocumento), Seq = situacaoEntregaDocumento.ToString() });

            return Json(itens, JsonRequestBehavior.AllowGet);
        }

        [SMCAuthorize(UC_INS_003_01_05.REGISTRO_DOCUMENTACAO_ENTREGUE)]
        public ActionResult PreencherDependencySituacaoEntrega(SituacaoEntregaDocumento situacaoEntregaDocumento, string observacao, FormaEntregaDocumento? formaEntregaDocumento, SMCUploadFile arquivoAnexado, VersaoDocumento? versaoDocumento, VersaoDocumento VersaoDocumentoExigido, DateTime? dataEntrega, DateTime? dataPrazoEntrega, long? seqDocumentoRequerido, bool? exibirObservacaoParaInscrito)
        {
            // Formas de entrega de documento
            var listaFormaEntregaDocumento = new List<SMCSelectListItem>();
            var listaPermitidaFormaEntrega = new FormaEntregaDocumento[] { FormaEntregaDocumento.Correios, FormaEntregaDocumento.Email, FormaEntregaDocumento.Presencial, FormaEntregaDocumento.Upload };
            foreach (var item in listaPermitidaFormaEntrega)
                listaFormaEntregaDocumento.Add(new SMCSelectListItem() { Text = SMCEnumHelper.GetDescription(item), Value = ((int)item).ToString() });

            // Versão do documento
            versaoDocumento = versaoDocumento ?? VersaoDocumento.Nenhum;
            var listaVersaoDocumento = new List<SMCSelectListItem>();
            var listaPermitidaVersao = new VersaoDocumento[] { VersaoDocumento.CopiaAutenticada, VersaoDocumento.CopiaSimples, VersaoDocumento.Original };
            foreach (var item in listaPermitidaVersao)
                listaVersaoDocumento.Add(new SMCSelectListItem() { Text = SMCEnumHelper.GetDescription(item), Value = ((int)item).ToString() });

            if (situacaoEntregaDocumento != SituacaoEntregaDocumento.AguardandoEntrega && situacaoEntregaDocumento != SituacaoEntregaDocumento.Pendente)
            {
                // Forma de entrega
                if (formaEntregaDocumento != null)
                    listaFormaEntregaDocumento.FirstOrDefault(l => l.Value == ((int)formaEntregaDocumento).ToString()).Selected = true;

                // Situação da entrega
                if ((versaoDocumento == null || versaoDocumento == VersaoDocumento.Nenhum) && VersaoDocumentoExigido != VersaoDocumento.Nenhum)
                    listaVersaoDocumento.FirstOrDefault(l => l.Value == ((int)VersaoDocumentoExigido).ToString()).Selected = true;
                else if ((versaoDocumento == null || versaoDocumento == VersaoDocumento.Nenhum) && VersaoDocumentoExigido == VersaoDocumento.Nenhum)
                {
                    // Não retorna mais... pois tem que retornar o json apenas no final com as outras propriedades
                    //return Json(listaVersaoDocumento);
                }
                else

                    listaVersaoDocumento.FirstOrDefault(l => l.Value == ((int)versaoDocumento).ToString()).Selected = true;

                // Data de entrega
                if (dataEntrega == null || !dataEntrega.HasValue)
                    dataEntrega = DateTime.Now.Date;

                dataPrazoEntrega = null;


            }

            if (situacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoEntrega || situacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)
            {
                if (arquivoAnexado != null)
                {
                    // Forma de entrega
                    if (formaEntregaDocumento != null)
                        listaFormaEntregaDocumento.FirstOrDefault(l => l.Value == ((int)formaEntregaDocumento).ToString()).Selected = true;

                    // Situação da entrega
                    if ((versaoDocumento == null || versaoDocumento == VersaoDocumento.Nenhum) && VersaoDocumentoExigido != VersaoDocumento.Nenhum)
                        listaVersaoDocumento.FirstOrDefault(l => l.Value == ((int)VersaoDocumentoExigido).ToString()).Selected = true;
                    else
                        listaVersaoDocumento.FirstOrDefault(l => l.Value == ((int)versaoDocumento).ToString()).Selected = true;
                }
                else
                {
                    // Data de entrega
                    dataEntrega = null;
                }

                if (situacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)
                {
                    if (seqDocumentoRequerido.HasValue && !dataPrazoEntrega.HasValue)
                    {
                        dataPrazoEntrega = this.AcompanhamentoProcessoControllerService.BuscarDataLimiteEntregaDocumentoRequerido(seqDocumentoRequerido.Value);
                    }

                }
                else
                {
                    dataPrazoEntrega = null;
                }

                if (situacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoEntrega)
                {
                    observacao = null;
                    exibirObservacaoParaInscrito = false;
                }

            }

            if (situacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido || situacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)
            {
                if (string.IsNullOrEmpty(observacao))
                    exibirObservacaoParaInscrito = false;
            }

            // Retorna os dados múltiplos
            return Json(new
            {
                FormaEntregaDocumento = listaFormaEntregaDocumento,
                Observacao = observacao,
                VersaoDocumento = listaVersaoDocumento,
                DataEntrega = dataEntrega.HasValue ? dataEntrega.Value.ToString("MM/dd/yyyy") : string.Empty,
                DataPrazoEntrega = dataPrazoEntrega.HasValue ? dataPrazoEntrega.Value.ToString("MM/dd/yyyy") : string.Empty,
                ExibirObservacaoParaInscrito = exibirObservacaoParaInscrito.ToString(),
            });
        }

        #endregion [ Dependency]

        #endregion Registro de Documentação Entregue

        #region Alterar Dados Inscrito

        [HttpGet]
        [SMCAuthorize(UC_INS_003_01_09.ALTERACAO_DADOS_INSCRITO)]
        public ActionResult AlterarDados(SMCEncryptedLong seqInscrito, SMCEncryptedLong origem, string backUrl)
        {
            var model = InscritoControllerService.BuscarNomesDadosInscrito(seqInscrito);
            this.PreencherModelo(model);
            model.Origem = origem;
            model.BackUrl = backUrl;
            ConfigurarVisibilidadeCamposPorProcesso(model);
            return View("_AlterarDados", model);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_003_01_09.ALTERACAO_DADOS_INSCRITO)]
        public ActionResult AlterarDados(AlterarDadosInscritoViewModel modelo)
        {
            this.PreencherModelo(modelo);

            // Verifica se a confirmação de email é igual ao email
            if (modelo.Email != modelo.EmailConfirmacao)
            {
                SetErrorMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Validacao_Email, "", SMCMessagePlaceholders.Centro);
            }
            else
            {
                InscritoControllerService.AlterarInscrito(modelo);
                SetSuccessMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Sucesso_Alteracao_Inscrito, MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);

                // Atualiza os dados do inscrito para apresentar na tela
                // Necessário para atualizar a formatação de nome que foi realizada em backend e não 
                // está atualizada no modelo
                var model = InscritoControllerService.BuscarNomesDadosInscrito(modelo.Seq);
                this.PreencherModelo(model);
                model.Origem = modelo.Origem;
                ConfigurarVisibilidadeCamposPorProcesso(model);

                if(!string.IsNullOrEmpty(modelo.BackUrl))
                {
                    model.BackUrl = modelo.BackUrl;
                }

                return View("_AlterarDados", model);
            }
            return View("_AlterarDados", modelo);
        }

        [SMCAuthorize(UC_INS_003_01_09.ALTERACAO_DADOS_INSCRITO)]
        public ActionResult CancelarAlterarDados(SMCEncryptedLong origem, string backUrl)
        {
            if (string.IsNullOrEmpty(backUrl))
            {
                return RedirectToAction("ConsultarInscricaoProcesso", "AcompanhamentoProcesso", new { area = "INS", seqProcesso = origem });
            }
            return Redirect(backUrl);/* RedirectToAction("Index", "AcompanhamentoInscrito", new { area = "INS" });*/
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_003_01_09.ALTERACAO_DADOS_INSCRITO)]
        public ActionResult AlterarDadosSincronizar(AlterarDadosInscritoViewModel modelo)
        {
            this.PreencherModelo(modelo);

            // Verifica se a confirmação de email é igual ao email
            if (modelo.Email != modelo.EmailConfirmacao)
            {
                SetErrorMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Validacao_Email, "", SMCMessagePlaceholders.Centro);
            }
            else
            {
                InscritoControllerService.AlterarInscritoSincronizarGDM(modelo);
                SetSuccessMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Sucesso_Alteracao_Inscrito, MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);

                // Atualiza os dados do inscrito para apresentar na tela
                // Necessário para atualizar a formatação de nome que foi realizada em backend e não 
                // está atualizada no modelo
                var model = InscritoControllerService.BuscarNomesDadosInscrito(modelo.Seq);
                this.PreencherModelo(model);
                model.Origem = modelo.Origem;
                ConfigurarVisibilidadeCamposPorProcesso(model);
                return View("_AlterarDados", model);
            }
            return View("_AlterarDados", modelo);
        }

        #endregion Alterar Dados Inscrito

        #region Observação Inscrição

        [SMCAuthorize(UC_INS_003_01_08.ALTERACAO_SITUACAO)]
        public ActionResult ObservacaoInscricao(SMCEncryptedLong seqInscricao)
        {
            var model = InscricaoService.BuscarObservacaoInscricao(seqInscricao).Transform<ObservacaoInscricaoViewModel>();
            return PartialView("_ObservacaoInscricao", model);
        }

        [SMCAuthorize(UC_INS_003_01_08.ALTERACAO_SITUACAO)]
        public ActionResult SalvarObservacao(ObservacaoInscricaoViewModel model)
        {
            InscricaoService.AlterarObservacaoInscricao(model.Transform<ObservacaoInscricaoData>());
            SetSuccessMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Observacao);
            return null;
        }

        #endregion Observação Inscrição

        #region Métodos de Apoio

        [SMCAllowAnonymous]
        public ActionResult ExibirJustificativaSituacao(SMCEncryptedLong seqInscricao)
        {
           var modelo = AcompanhamentoProcessoControllerService.BuscarJustificativaSituacao(seqInscricao.Value);

            return PartialView("_JustificativaSituacao", modelo);
        }


        /// <summary>
        /// Preenche as listas de select para o modelo.
        /// </summary>
        /// <param name="modelo">Modelo a ser preenchido</param>
        [NonAction]
        private void PreencherModelo(AlterarDadosInscritoViewModel modelo)
        {
            // Busca a lista de estados para UF do RG
            modelo.EstadosIdentidade = BuscarEstadosSelect();
            // Busca a lista de paises para nacionalidade
            modelo.Paises = BuscarPaisesSelect();

            // Monta a lista de tipo de endereço eletrônico excluindo o tipo email
            if (modelo.TiposEnderecoEletronico.SMCIsNullOrEmpty())
            {
                modelo.TiposEnderecoEletronico = new List<SMCDatasourceItem>();
                foreach (TipoEnderecoEletronico item in Enum.GetValues(typeof(TipoEnderecoEletronico)))
                {
                    if (item != TipoEnderecoEletronico.Email && item != TipoEnderecoEletronico.Nenhum)
                    {
                        var seq = Convert.ToInt64(item);
                        var descricao = SMCEnumHelper.GetDescription(item) ?? "";
                        SMCDatasourceItem datasourceItem = new SMCDatasourceItem()
                        {
                            Seq = seq,
                            Descricao = descricao,
                        };
                        modelo.TiposEnderecoEletronico.Add(datasourceItem);
                    }
                }
            }
        }

        #endregion Métodos de Apoio

        #region Métodos auxiliares do SGF

        public ActionResult RelatorioFormulario(long seqDadoFormulario, long seqInscricao, long seqFormularioSGF, long seqVisaoSGF)
        {
            InscricaoDadoFormularioViewModel model = new InscricaoDadoFormularioViewModel
            {
                SeqVisao = seqVisaoSGF,
                SeqFormulario = seqFormularioSGF,
                SeqInscricao = seqInscricao
            };

            model.DadosCampos = InscricaoService.BuscarDadoFormulario(seqDadoFormulario).DadosCampos.TransformList<DadoCampoViewModel>();

            return PartialView("_SGFReport", model);
        }

        #endregion Métodos auxiliares do SGF

        #region Liberar alteração inscricao

        [SMCAuthorize(UC_INS_003_01_06.LIBERAR_ALTERACAO_INSCRICAO)]
        public ActionResult LiberarAlteracaoInscricao(InscricaoViewModel model)
        {
            try
            {
                this.AcompanhamentoProcessoControllerService.ValidarLiberacaoAlteracaoInscricao(model.SeqInscricao);
                var mensagemConfirmacao = MontarMensagemConfirmacaoLiberacaoAlteracaoInscricao(model.SeqInscricao);

                Assert("MensagemConfirmacao", model, mensagemConfirmacao, () => { return true; });

                this.AcompanhamentoProcessoControllerService.LiberarAlteracaoInscricao(model.SeqInscricao);

                SetSuccessMessage(Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Sucesso_Liberacao_Alteracao_Inscricao,
                    MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);

                return SMCRedirectToAction("ConsultaInscricao", routeValues: new { seqInscricao = new SMCEncryptedLong(model.SeqInscricao), origem = model.Origem, backUrl = model.BackURL });
                
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message, MessagesResource.Titulo_Erro, SMCMessagePlaceholders.Centro);
                return BackToAction();
            }
        }

        private string MontarMensagemConfirmacaoLiberacaoAlteracaoInscricao(long seqInscricao)
        {
            var msgLiberacaoOfertaNaoVigente = Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Liberacao_Alteracao_Inscricao_Oferta_Nao_Vigente;
            var msgLiberacaoCandidatoBoletoPago = Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Liberacao_Alteracao_Inscricao_Candidato_Boleto_Pago;
            var msgLiberacaoCandidatoBoletoNaoPago = Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Liberacao_Alteracao_Inscricao_Candidato_Boleto_Nao_Pago;

            var possuiOfertaVigente = this.AcompanhamentoProcessoControllerService.PossuiOfertaVigente(seqInscricao);
            var possuiBoletoPago = this.AcompanhamentoProcessoControllerService.PossuiBoletoPago(seqInscricao);
            var possuiBoleto = this.AcompanhamentoProcessoControllerService.PossuiBoleto(seqInscricao);
            var mensagemConfirmacao = Views.AcompanhamentoProcesso.App_LocalResources.UIResource.Mensagem_Confirmacao_Liberacao_Alteracao_Inscricao;
            var mensagensComplemento = new StringBuilder();

            /*3.1. Se a oferta selecionada pelo candidato não estiver mais vigente ou estiver desativada ou cancelada,
             * concatenar ao texto da mensagem, em negrito, o parágrafo:

                “A oferta selecionada pelo candidato não está mais vigente ou válida. Se a situação da oferta
                permanecer desta forma, o candidato terá que escolher outra opção para concluir sua inscrição.” */
            if (!possuiOfertaVigente)
            {
                mensagensComplemento.Append($" <b>{msgLiberacaoOfertaNaoVigente}</b>");
            }
            /*3.2. Se o candidato possuir boleto pago, concatenar ao texto da mensagem, em negrito, o parágrafo:
            “O candidato já pagou o boleto, portanto as taxas não poderão ser alteradas.”*/
            if (possuiBoletoPago)
            {
                mensagensComplemento.Append($" <b>{msgLiberacaoCandidatoBoletoPago}</b>");
            }
            else if (possuiBoleto)
            {
                /*3.3. Se o candidato possuir boleto e este ainda não estiver pago, concatenar ao texto da mensagem o parágrafo:
                Caso o candidato altere as informações de taxa, seu boleto atual será cancelado e um novo boleto será gerado,
                quando a inscrição for novamente finalizada. “O sistema reconhecerá somente o pagamento do novo boleto gerado.”*/
                mensagensComplemento.Append($" {msgLiberacaoCandidatoBoletoNaoPago}");
            }

            return string.Format(mensagemConfirmacao, mensagensComplemento.ToString());
        }

        #endregion Liberar alteração inscricao

        /// <summary>
        /// Busca uma lista de paises
        /// </summary>
        /// <returns>Lista de países</returns>
        [NonAction]
        private List<SMCDatasourceItem> BuscarPaisesSelect()
        {
            PaisData[] paises = LocalidadeService.BuscarPaisesValidosCorreios();
            List<SMCDatasourceItem> lista = new List<SMCDatasourceItem>();
            foreach (var pais in paises)
            {
                lista.Add(new SMCDatasourceItem(pais.Codigo, pais.Nome));
            }
            return lista;
        }

        /// <summary>
        /// Busca uma lista de estados
        /// </summary>
        /// <returns>Lista de estados</returns>
        [NonAction]
        private List<SMCSelectListItem> BuscarEstadosSelect()
        {
            UFData[] ufs = LocalidadeService.BuscarUfs();
            List<SMCSelectListItem> lista = new List<SMCSelectListItem>();
            foreach (var uf in ufs)
            {
                lista.Add(new SMCSelectListItem(uf.Codigo, uf.Codigo));
            }
            return lista;
        }

        /// <summary>
        /// Configura a visibilidade dos campos por processo
        /// </summary>
        /// <param name="modelo">Modelo dos dados inscrito</param>
        private void ConfigurarVisibilidadeCamposPorProcesso(AlterarDadosInscritoViewModel modelo)
        {
            if (modelo.Origem.HasValue)
            {
                List<ProcessoCampoInscritoData> camposInscritoProcesso = ProcessoCampoInscritoService.BuscarCamposIncritosPorProcesso(modelo.Origem.Value);

                foreach (var campo in camposInscritoProcesso)
                {
                    if (campo.CampoInscrito == CampoInscrito.CPF)
                    {
                        modelo.ExibirCPF = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.DataNascimento)
                    {
                        modelo.ExibirDataNascimento = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Email)
                    {
                        modelo.ExibirEmail = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Endereco)
                    {
                        modelo.ExibirEndereco = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.EstadoCivil)
                    {
                        modelo.ExibirEstadoCivil = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Filiacao)
                    {
                        modelo.ExibirFiliacao = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Nacionalidade)
                    {
                        modelo.ExibirNacionalidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Naturalidade)
                    {
                        modelo.ExibirNaturalidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Nome)
                    {
                        modelo.ExibirNome = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.NumeroIdentidade)
                    {
                        modelo.ExibirNumeroIdentidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.OrgaoEmissorIdentidade)
                    {
                        modelo.ExibirOrgaoEmissorIdentidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.OutrosEndereçosEletronicos)
                    {
                        modelo.ExibirOutrosEnderecosEletronicos = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.PaisOrigem)
                    {
                        modelo.ExibirPaisOrigem = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Passaporte)
                    {
                        modelo.ExibirPassaporte = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Sexo)
                    {
                        modelo.ExibirSexo = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Telefone)
                    {
                        modelo.ExibirTelefone = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.UfIdentidade)
                    {
                        modelo.ExibirUfIdentidade = true;
                    }
                }
            }
            else
            {
                modelo.ExibirCPF = true;
                modelo.ExibirDataNascimento = true;
                modelo.ExibirEmail = true;
                modelo.ExibirEndereco = true;
                modelo.ExibirEstadoCivil = true;
                modelo.ExibirFiliacao = true;
                modelo.ExibirNacionalidade = true;
                modelo.ExibirNaturalidade = true;
                modelo.ExibirNome = true;
                modelo.ExibirNumeroIdentidade = true;
                modelo.ExibirOrgaoEmissorIdentidade = true;
                modelo.ExibirOutrosEnderecosEletronicos = true;
                modelo.ExibirPaisOrigem = true;
                modelo.ExibirPassaporte = true;
                modelo.ExibirSexo = true;
                modelo.ExibirTelefone = true;
                modelo.ExibirUfIdentidade = true;
            }
        }
    }
}