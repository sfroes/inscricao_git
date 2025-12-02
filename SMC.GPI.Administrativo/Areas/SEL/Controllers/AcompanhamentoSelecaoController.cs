using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Excel;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Ioc;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.OpenXml;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.Areas.SEL.Models;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.SEL.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Data;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Interfaces;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using SMC.Localidades.ServiceContract.Areas.LOC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IUnidadeResponsavelService = SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces.IUnidadeResponsavelService;

namespace SMC.GPI.Administrativo.Areas.SEL.Controllers
{
    public class AcompanhamentoSelecaoController : SMCControllerBase
    {
        #region Services

        private ISelecaoService SelecaoService
        {
            get { return Create<ISelecaoService>(); }
        }

        private IGrupoOfertaService GrupoOfertaService
        {
            get { return Create<IGrupoOfertaService>(); }
        }

        private IProcessoService ProcessoService
        {
            get { return Create<IProcessoService>(); }
        }

        private IInscricaoService InscricaoService
        {
            get { return Create<IInscricaoService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return Create<ISituacaoService>(); }
        }

        private IUnidadeResponsavelService UnidadeResponsavelService
        {
            get { return Create<IUnidadeResponsavelService>(); }
        }

        private ITipoProcessoService TipoProcessoService
        {
            get { return Create<ITipoProcessoService>(); }
        }

        private ILocalidadeService LocalidadeService
        {
            get { return Create<ILocalidadeService>(); }
        }

        #endregion Services

        #region Consulta Posição Consolidada por Processo

        [SMCAuthorize(UC_SEL_001_01_01.CONSULTA_POSICAO_CONSOLIDADA_SELECAO_PROCESSO)]
        public ActionResult Index(AcompanhamentoSelecaoFiltroViewModel filtro = null)
        {
            filtro.UnidadesResponsaveis = UnidadeResponsavelService.BuscarUnidadesResponsaveisKeyValue();
            filtro.TiposProcessos = TipoProcessoService.BuscarTiposProcessoKeyValue().TransformList<SMCDatasourceItem>();
            return View(filtro);
        }

        [SMCAuthorize(UC_SEL_001_01_01.CONSULTA_POSICAO_CONSOLIDADA_SELECAO_PROCESSO)]
        public ActionResult ListarPosicaoConsolidada(AcompanhamentoSelecaoFiltroViewModel filtro = null, int? exportAction = null)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            if (!exportAction.HasValue)
            {
                var lista = SelecaoService.ConsultaPosicaoConsolidada(filtro.Transform<AcompanhamentoSelecaoFiltroData>())
                                        .Transform<SMCPagerData<AcompanhamentoSelecaoListaViewModel>>();
                return PartialView("_ListarPosicaoConsolidada", new SMCPagerModel<AcompanhamentoSelecaoListaViewModel>(lista, filtro.PageSettings, filtro));
            }
            else
            {
                filtro.PageSettings.PageSize = Int32.MaxValue;
                var lista = SelecaoService.ConsultaPosicaoConsolidada(filtro.Transform<AcompanhamentoSelecaoFiltroData>())
                                        .Transform<SMCPagerData<AcompanhamentoSelecaoListaViewModel>>();

                byte[] data = SMCGridExporter.ExportGridModelToExcel(this.ControllerContext, "PosicaoProcessos", lista);
                return File(data, "application/download", "PosicaoConsolidadaSelecao.xlsx");
            }
        }

        #endregion Consulta Posição Consolidada por Processo

        #region Consulta Posição Consolidada da Seleção por Grupo de Ofertas e por Oferta

        [SMCAuthorize(UC_SEL_001_01_02.CONSULTA_POSICAO_CONSOLIDADA_SELECAO_GRUPO_OFERTAS)]
        public ActionResult ConsultarProcessoPorGrupoOferta(PosicaoConsolidadaPorGrupoOfertaFiltroViewModel filtro)
        {
            var cabecalho = SelecaoService.ConsultaPosicaoConsolidada(new AcompanhamentoSelecaoFiltroData() { SeqProcesso = filtro.SeqProcesso }).Single();

            var model = SMCMapperHelper.Create<PosicaoConsolidadaPorGrupoOfertaFiltroViewModel>(cabecalho);

            model.SeqProcesso = filtro.SeqProcesso;
            model.SeqItemHierarquiaOferta = filtro.SeqItemHierarquiaOferta;
            model.SeqOferta = filtro.SeqOferta;
            model.SeqGrupoOferta = filtro.SeqGrupoOferta;

            model.GrupoOfertas = GrupoOfertaService.BuscaGruposOfertaKeyValue(filtro.SeqProcesso).TransformList<SMCDatasourceItem>();

            return View(model);
        }

        [SMCAuthorize(UC_SEL_001_01_02.CONSULTA_POSICAO_CONSOLIDADA_SELECAO_GRUPO_OFERTAS)]
        public ActionResult ListarProcessoGrupoOferta(PosicaoConsolidadaPorGrupoOfertaFiltroViewModel filtro = null, int? exportAction = null)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            if (!exportAction.HasValue)
            {
                var lista = SelecaoService.ConsultaPosicaoConsolidadaGrupoOferta(filtro.Transform<PosicaoConsolidadaPorGrupoOfertaFiltroData>())
                                        .Transform<SMCPagerData<PosicaoConsolidadaPorGrupoOfertaListaViewModel>>();
                var model = new SMCPagerModel<PosicaoConsolidadaPorGrupoOfertaListaViewModel>(lista, filtro.PageSettings, filtro);
                return PartialView("_ListarProcessoGrupoOferta", model);
            }
            else
            {
                filtro.PageSettings.PageSize = Int32.MaxValue;
                var lista = SelecaoService.ConsultaPosicaoConsolidadaGrupoOferta(filtro.Transform<PosicaoConsolidadaPorGrupoOfertaFiltroData>())
                                        .Transform<SMCPagerData<PosicaoConsolidadaPorGrupoOfertaListaViewModel>>();

                List<List<string>> matriz = new List<List<string>>();
                foreach (var grupo in lista)
                {
                    var linha = new List<string>();
                    linha.Add("Grupo de Oferta");
                    linha.Add(grupo.Descricao);
                    matriz.Add(linha);
                    var linhaCabecalho = new List<String>
                    {
                        "Descrição",
                        "Candidatos Confirmados",
                        "Candidatos Desistentes",
                        "Candidatos Reprovados",
                        "Candidatos Selecionados",
                        "Candidatos Excedentes",
                        "Convocados",
                        "Convocados Desistentes",
                        "Convocados Confirmados"
                    };
                    matriz.Add(linhaCabecalho);
                    foreach (var oferta in grupo.PosicoesConsolidadasOfertas)
                    {
                        var linhaOferta = new List<String>
                        {
                            oferta.Descricao,
                            oferta.CandidatosConfirmados.ToString(),
                            oferta.CandidatosDesistentes.ToString(),
                            oferta.CandidatosReprovados.ToString(),
                            oferta.CandidatosSelecionados.ToString(),
                            oferta.CandidatosExcedentes.ToString(),
                            oferta.Convocados.ToString(),
                            oferta.ConvocadosDesistentes.ToString(),
                            oferta.ConvocadosConfirmados.ToString()
                        };
                        matriz.Add(linhaOferta);
                    }
                    var linhaTotalGrupo = new List<String>
                    {
                        "Total Grupo",
                        grupo.CandidatosConfirmados.ToString(),
                        grupo.CandidatosDesistentes.ToString(),
                        grupo.CandidatosReprovados.ToString(),
                        grupo.CandidatosSelecionados.ToString(),
                        grupo.CandidatosExcedentes.ToString(),
                        grupo.Convocados.ToString(),
                        grupo.ConvocadosDesistentes.ToString(),
                        grupo.ConvocadosConfirmados.ToString()
                    };
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

                var excel = SMCMatrixExcel.Convert(dicMatriz);
                var workBook = SMCIocContainer.Resolve<ISMCWorkbookService, byte[]>(work => work.RenderXlsFromMatrix(excel));

                return File(workBook, "application/download", "SelecoesProcesso.xlsx");
            }
        }

        #endregion Consulta Posição Consolidada da Seleção por Grupo de Ofertas e por Oferta

        #region Consulta Candidados do Processo

        [SMCAuthorize(UC_SEL_001_01_03.CONSULTA_CANDIDATOS_PROCESSO)]
        public ActionResult ConsultaSelecoesProcesso(ConsultaCandidatosProcessoFiltroViewModel filtro = null, SMCEncryptedLong seqOferta = null)
        {
            if (seqOferta != null)
            {
                if (filtro.SeqOferta == null)
                    filtro.SeqOferta = new GPILookupViewModel();
                filtro.SeqOferta.Seq = seqOferta;
            }

            var cabecalho = SelecaoService.BuscarCabecalhoSelecaoProcesso(filtro.SeqProcesso);
            filtro.TipoProcesso = cabecalho.TipoProcesso;
            filtro.Descricao = cabecalho.Descricao;

            filtro.SituacoesProcesso = ProcessoService.BuscarSituacoesProcessoPorEtapaKeyValue(filtro.SeqProcesso, TOKENS.ETAPA_SELECAO, TOKENS.ETAPA_CONVOCACAO)
                                                        .TransformList<SMCDatasourceItem>();
            filtro.GrupoOfertas = GrupoOfertaService.BuscaGruposOfertaKeyValue(filtro.SeqProcesso).TransformList<SMCDatasourceItem>();

            return View(filtro);
        }

        [SMCAuthorize(UC_SEL_001_01_03.CONSULTA_CANDIDATOS_PROCESSO)]
        public ActionResult ListarCandidatosProcesso(ConsultaCandidatosProcessoFiltroViewModel filtro = null, int? exportAction = null)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            if (!exportAction.HasValue)
            {
                var lista = SelecaoService.BuscarCandidatosProcesso(filtro.Transform<ConsultaCandidatosProcessoFiltroData>())
                                        .Transform<SMCPagerData<ConsultaCandidatosProcessoListaViewModel>>();
                var model = new SMCPagerModel<ConsultaCandidatosProcessoListaViewModel>(lista, filtro.PageSettings, filtro);
                return PartialView("_ListarCandidatosProcesso", model);
            }
            else
            {
                filtro.PageSettings.PageSize = Int32.MaxValue;
                var lista = SelecaoService.BuscarCandidatosProcesso(filtro.Transform<ConsultaCandidatosProcessoFiltroData>())
                                        .Transform<SMCPagerData<ConsultaCandidatosProcessoListaExcelViewModel>>();

                List<List<string>> matriz = new List<List<string>>();
                var cabecalho = new List<string>();
                cabecalho.AddRange(new List<string> { "Nº inscrição", "Candidato", "RG", "CPF", "Data de nascimento", "Data inscrição", "Opção", "Oferta", "Situação", "Motivo", "1ª Nota / % Bolsa", "2ª Nota / % Financiamento", "Classificação", "E-mail", "Telefones", "Celular", "Logradouro", "Nº", "Complemento", "Bairro", "CEP", "Cidade", "UF", "Pais" });
                matriz.Add(cabecalho);
                var paises = LocalidadeService.BuscarPaisesValidosCorreios();
                foreach (var item in lista)
                {
                    var linha = new List<string>
                    {
                        item.SeqInscricao.ToString(),
                        item.Candidato,
                        item.NumeroIdentidade,
                        item.Cpf,
                        item.DataNascimento.ToString("dd/MM/yyyy"),
                        item.DataInscricao.ToString(),
                        item.Opcao,
                        item.Oferta,
                        item.Situacao,
                        item.Motivo,
                        item.Nota.HasValue ? item.Nota.Value.ToString() : "",
                        item.SegundaNota.HasValue ? item.SegundaNota.Value.ToString() : "",
                        item.Classificacao.HasValue ? item.Classificacao.Value.ToString() : "",
                        item.Email
                    };

                    var telefones = new List<string>();
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

                            string numeroFormatado = string.Format("{0}: +{1} ({2}) {3}", SMCEnumHelper.GetDescription((TipoTelefone)telefone.TipoTelefone)
                                , telefone.CodigoPais, telefone.CodigoArea, telefone.Numero);

                            telefones.Add(numeroFormatado);

                            if (celular == null && telefone.TipoTelefone == TipoTelefone.Celular)
                            {
                                celular = numeroFormatado;
                            }
                        }
                    }
                    linha.Add(string.Join("\r\n", telefones));
                    linha.Add(celular ?? "");
                    var endereco = item.Enderecos?.FirstOrDefault(f => f.Correspondencia == true);
                    if (endereco != null)
                    {
                        linha.AddRange(new[]
                        {
                            endereco.Logradouro,
                            endereco.Numero,
                            endereco.Complemento,
                            endereco.Bairro,
                            endereco.Cep,
                            endereco.NomeCidade,
                            endereco.SiglaUf,
                            paises.FirstOrDefault(f=>f.Codigo == endereco.SeqPais)?.Nome ?? ""
                        });
                    }
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
                var dicMatriz = new Dictionary<string, string[,]>
                {
                    { "PosicaoProcessos", planilha }
                };

                var excel = SMCMatrixExcel.Convert(dicMatriz);
                var workBook = SMCIocContainer.Resolve<ISMCWorkbookService, byte[]>(work => work.RenderXlsFromMatrix(excel));
                return File(workBook, "application/download", "PosicaoConsolidadaSelecao.xlsx");
            }
        }

        [SMCAuthorize(UC_SEL_001_01_03.CONSULTA_CANDIDATOS_PROCESSO)]
        public ActionResult ExibirDadosOferta(SMCEncryptedLong seqInscricaoOferta)
        {
            var dadosInscricaoOferta = SelecaoService.BuscarDadosOferta(new OfertaFiltroData() { SeqInscricaoOferta = seqInscricaoOferta });

            return PartialView("_ExibirDadosOferta", dadosInscricaoOferta.Transform<DadosOfertaViewModel>());
        }

        [SMCAuthorize(UC_SEL_001_01_03.CONSULTA_CANDIDATOS_PROCESSO)]
        public ActionResult ExibirMotivoJustificativaSituacao(SMCEncryptedLong seqInscricaoHistoricoSituacao)
        {
            var model = InscricaoService.BuscarSituacaoInscricaoOferta(seqInscricaoHistoricoSituacao)
                                            .Transform<MotivoSituacaoViewModel>();
            if (model.SeqMotivoSituacao.HasValue)
            {
                var motivo = SituacaoService.BuscarDescricaoMotivos(new long[] { model.SeqMotivoSituacao.Value }).FirstOrDefault();
                if (motivo != null)
                {
                    model.Motivo = motivo.Descricao;
                }
            }
            return PartialView("_ExibirMotivoJustificativaSituacao", model);
        }

        #endregion Consulta Candidados do Processo

        #region Alteração de Oferta

        public ActionResult CabecalhoOferta(OfertaFiltroViewModel filtro)
        {
            var model = SelecaoService.BuscarOfertaCabecalho(filtro.Transform<OfertaFiltroData>()).Transform<OfertaCabecalhoViewModel>();
            return PartialView("_CabecalhoOferta", model);
        }

        [HttpPost]
        [SMCAuthorize(UC_SEL_001_01_10.ALTERAR_OFERTA)]
        public ActionResult AlterarOferta(OfertaFiltroViewModel filtro)
        {
            try
            {
                var model = SelecaoService.BuscarOferta(filtro.Transform<OfertaFiltroData>()).Transform<OfertaViewModel>();
                model.SeqProcesso = filtro.SeqProcesso.Value;
                return PartialView("_AlterarOferta", model);
            }
            catch (SMCApplicationException e)
            {
                SetErrorMessage(e.Message);
                if (filtro.BackUrl != null)
                {
                    return SMCRedirectToUrl(filtro.BackUrl);
                }
                else
                {
                    return SMCRedirectToAction("ConsultaSelecoesProcesso", "AcompanhamentoSelecao", new { seqProcesso = (SMCEncryptedLong)filtro.SeqProcesso });
                }
            }
        }

        [HttpPost]
        [SMCAuthorize(UC_SEL_001_01_10.ALTERAR_OFERTA)]
        public ActionResult SalvarOferta(OfertaViewModel model)
        {
            SelecaoService.SalvarAlteracaoOferta(model.Transform<OfertaAlteracaoData>());
            SetSuccessMessage(Views.AcompanhamentoSelecao.App_LocalResources.UIResource.Mensagem_Sucesso_Alteracao_Oferta, target: SMCMessagePlaceholders.Centro);
            return Json(true);
        }

        [SMCAuthorize(UC_SEL_001_01_10.ALTERAR_OFERTA)]
        public bool BloquearJustificativaAlteracaoOferta(SMCEncryptedLong seqOferta, SMCEncryptedLong seqOfertaOriginal)
        {
            return seqOferta?.Value == seqOfertaOriginal?.Value;
        }

        #endregion Alteração de Oferta
    }
}