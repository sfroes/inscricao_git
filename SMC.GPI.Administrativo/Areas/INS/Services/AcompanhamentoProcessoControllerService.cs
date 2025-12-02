using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Financeiro.BLT.Common;
using SMC.Financeiro.ServiceContract.Areas.HUB.Data;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.BLT.Data;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Logging;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Localidades.ServiceContract.Areas.LOC.Data;
using SMC.Localidades.ServiceContract.Areas.LOC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class AcompanhamentoProcessoControllerService : SMCControllerServiceBase
    {
        #region Services

        private IProcessoService ProcessoService
        {
            get { return this.Create<IProcessoService>(); }
        }

        private IGrupoOfertaService GrupoOfertaService
        {
            get { return this.Create<IGrupoOfertaService>(); }
        }

        private IInscricaoService InscricaoService
        {
            get { return this.Create<IInscricaoService>(); }
        }

        private IArquivoAnexadoService ArquivoAnexadoService
        {
            get { return this.Create<IArquivoAnexadoService>(); }
        }

        private ITipoProcessoService TipoProcessoService
        {
            get { return this.Create<ITipoProcessoService>(); }
        }

        private ILocalidadeService LocalidadeService
        {
            get { return this.Create<ILocalidadeService>(); }
        }

        private IInscritoService InscritoService
        {
            get { return this.Create<IInscritoService>(); }
        }

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        private IDocumentoRequeridoService DocumentoRequeridoService
        {
            get { return this.Create<IDocumentoRequeridoService>(); }
        }

        #endregion Services

        #region Posição consolidada por processo

        public SMCPagerModel<ConsultaConsolidadaProcessoListaViewModel> BuscarPosicaoConsolidadaProcessos(ConsultaConsolidadaProcessoFiltroViewModel filtros)
        {
            ProcessoFiltroData filtroData = SMCMapperHelper.Create<ProcessoFiltroData>(filtros);
            SMCPagerData<PosicaoConsolidadaProcessoData> datas = ProcessoService.BuscarPosicaoConsolidadaProcesso(filtroData);
            SMCPagerData<ConsultaConsolidadaProcessoListaViewModel> model = SMCMapperHelper.Create<SMCPagerData<ConsultaConsolidadaProcessoListaViewModel>>(datas);
            return new SMCPagerModel<ConsultaConsolidadaProcessoListaViewModel>(model, filtros.PageSettings, filtros);
        }

        public ConsultaConsolidadaProcessoCabecalhoViewModel BuscarPosicaoConsolidadaProcesso(long seqProcesso)
        {
            var cabecalho = SMCMapperHelper.Create<ConsultaConsolidadaProcessoCabecalhoViewModel>
                (ProcessoService.BuscarCabecalhoProcesso(seqProcesso));

            cabecalho.PosicaoConsolidada =
                SMCMapperHelper.Create<ConsultaConsolidadaProcessoListaViewModel>(
                    ProcessoService.BuscarPosicaoConsolidadaProcesso(seqProcesso));
            return cabecalho;
        }

        #endregion Posição consolidada por processo

        #region Posição consolidada por grupo de oferta e oferta

        public SMCPagerModel<ConsultaConsolidadaGrupoOfertaListaViewModel> BuscarPosicaoConsolidadaPorGrupoOferta(ConsultaConsolidadaGrupoOfertaFiltroViewModel filtros)
        {
            PosicaoConsolidadaGrupoOfertaFiltroData filtroData =
                SMCMapperHelper.Create<PosicaoConsolidadaGrupoOfertaFiltroData>(filtros);
            SMCPagerData<PosicaoConsolidadaGrupoOfertaData> datas =
                GrupoOfertaService.BuscarPosicaoConsolidadaGruposOferta(filtroData);
            SMCPagerData<ConsultaConsolidadaGrupoOfertaListaViewModel> model =
                SMCMapperHelper.Create<SMCPagerData<ConsultaConsolidadaGrupoOfertaListaViewModel>>(datas);
            return new SMCPagerModel<ConsultaConsolidadaGrupoOfertaListaViewModel>(model, filtros.PageSettings, filtros);
        }

        #endregion Posição consolidada por grupo de oferta e oferta

        #region Listar Inscrições de um processo

        public SMCPagerModel<ConsultaInscricaoProcessoListaViewModel> BuscarInscricoesProcesso(ConsultaInscricaoProcessoFiltroViewModel filtros)
        {
            SituacaoInscricaoProcessoFiltroData filtroData =
                SMCMapperHelper.Create<SituacaoInscricaoProcessoFiltroData>(filtros);
            SMCPagerData<SituacaoInscricaoProcessoData> datas =
                InscricaoService.BuscarSituacaoInscricaoProcesso(filtroData);
            SMCPagerData<ConsultaInscricaoProcessoListaViewModel> model =
                SMCMapperHelper.Create<SMCPagerData<ConsultaInscricaoProcessoListaViewModel>>(datas);
            return new SMCPagerModel<ConsultaInscricaoProcessoListaViewModel>(model, filtros.PageSettings, filtros);
        }
        public JustificativaSituacaoInscricaoViewModel BuscarJustificativaSituacao(long seqInscricao)
        {
            return InscricaoService.BuscarJustificativaSituacao(seqInscricao).Transform<JustificativaSituacaoInscricaoViewModel>();
        }

        public SMCPagerModel<ConsultaInscricaoProcessoInscritoListaViewModel> BuscarInscricoesProcessoExcel(ConsultaInscricaoProcessoFiltroViewModel filtros)
        {
            SituacaoInscricaoProcessoFiltroData filtroData =
                SMCMapperHelper.Create<SituacaoInscricaoProcessoFiltroData>(filtros);
            SMCPagerData<SituacaoInscricaoInscritoProcessoData> datas =
                InscricaoService.BuscarSituacaoInscricaoInscritoProcesso(filtroData);
            SMCPagerData<ConsultaInscricaoProcessoInscritoListaViewModel> model =
                SMCMapperHelper.Create<SMCPagerData<ConsultaInscricaoProcessoInscritoListaViewModel>>(datas);
            return new SMCPagerModel<ConsultaInscricaoProcessoInscritoListaViewModel>(model, filtros.PageSettings, filtros);
        }

        #endregion Listar Inscrições de um processo

        #region Análise de  Inscrições em Lote

        public SMCPagerModel<AnaliseInscricaoLoteListaViewModel> BuscarInscricoesProcesso(AnaliseInscricaoLoteFiltroViewModel filtros)
        {
            SituacaoInscricaoProcessoFiltroData filtroData =
              SMCMapperHelper.Create<SituacaoInscricaoProcessoFiltroData>(filtros);
            SMCPagerData<SituacaoInscricaoProcessoData> datas =
                InscricaoService.BuscarSituacaoInscricaoProcesso(filtroData);
            SMCPagerData<AnaliseInscricaoLoteListaViewModel> model =
                SMCMapperHelper.Create<SMCPagerData<AnaliseInscricaoLoteListaViewModel>>(datas);
            return new SMCPagerModel<AnaliseInscricaoLoteListaViewModel>(model, filtros.PageSettings, filtros);
        }

        #endregion Análise de  Inscrições em Lote

        #region Shared

        public ProcessoCabecalhoViewModel BuscarCabecalhoProcesso(long seqProcesso)
        {
            var cabecalho = SMCMapperHelper.Create<ProcessoCabecalhoViewModel>
                (ProcessoService.BuscarCabecalhoProcesso(seqProcesso));
            return cabecalho;
        }

        #endregion Shared

        #region Alteração de situação de inscrição

        /// <summary>
        /// Busca as inscrições para alteração de situação, conforme seleção do usuário
        /// </summary>
        /// <param name="seqInscricoesSelecionadas">Seq´s selecionados pelo usuário no grid de inscrições</param>
        /// <returns>Lista de inscrições para alteração de situação</returns>
        public AlteracaoSituacaoViewModel BuscarInscricoesAlteracaoSituacao(InscricaoSelecionadaViewModel filtro)
        {
            AlteracaoSituacaoViewModel vm = new AlteracaoSituacaoViewModel { Lote = filtro.Lote };

            var filtroData = SMCMapperHelper.Create<InscricaoSelecionadaData>(filtro);
            vm.Inscricoes = InscricaoService.BuscarInscricoesPorSequencial(filtroData)
                .TransformMasterDetailList<DetalheAlteracaoSituacaoViewModel>();
            vm.SeqTipoProcessoSituacao = filtro.SeqTipoProcessoSituacao;
            vm.SeqProcesso = filtro.SeqProcesso;
            vm.SituacoesProcesso = TipoProcessoService.BuscarTipoProcessoSitucaoDestinoKeyValue(filtro.SeqTipoProcessoSituacao, filtro.SeqProcesso, false)
                .TransformList<SMCDatasourceItem>();
            vm.DescricaoSituacaoAtual = TipoProcessoService.BuscarTipoProcessoSituacaoKeyValue(filtro.SeqTipoProcessoSituacao).Descricao;
            return vm;
        }

        public void SalvarAlteracaoSituacaoLote(AlteracaoSituacaoViewModel modelo)
        {
            var data = modelo.Transform<AlteracaoSituacaoData>();
            data.SeqInscricoes = modelo.Inscricoes.Select(x => x.SeqInscricao).ToList();
            InscricaoService.AlterarSituacaoInscricoes(data);
        }

        public List<SMCDatasourceItem> BuscarMotivosSituacao(long seqTipoProcessoSituacao)
        {
            if (seqTipoProcessoSituacao == -1)
                return new List<SMCDatasourceItem>();

            var tipoProcesso = TipoProcessoService.BuscarTipoProcessoSituacao(seqTipoProcessoSituacao);
            return SituacaoService.BuscarMotivosSituacao(tipoProcesso.SeqSituacao).TransformList<SMCDatasourceItem>();
        }

        #endregion Alteração de situação de inscrição

        #region Histório de Situação

        public List<HistoricoSituacaoListaViewModel> BuscarSituacoesInscricao(long seqInscricao)
        {
            return InscricaoService.BuscarSituacoesInscricao(seqInscricao)
                            .TransformList<HistoricoSituacaoListaViewModel>();
        }

        public HistoricoSituacaoViewModel BuscarHistoricoSituacao(long seqHistoricoSituacao)
        {
            return InscricaoService.BuscarHistoricoSituacao(seqHistoricoSituacao)
                            .Transform<HistoricoSituacaoViewModel>();
        }

        public long SalvarAlteracaoSituacao(HistoricoSituacaoViewModel modelo)
        {
            InscricaoService.AlterarMotivoEJustificativaSituacao(modelo.Seq, modelo.Justificativa, modelo.SeqMotivoSGF);
            return modelo.Seq;
        }

        #endregion Histório de Situação

        #region Documentos Inscrição

        public RegistroDocumentacaoViewModel BuscarRegistroDocumentacao(long seqInscricao)
        {
            var resumoInscricao = SMCMapperHelper.Create<RegistroDocumentacaoViewModel>(
                this.InscricaoService.BuscarInscricaoResumida(seqInscricao,false));

            resumoInscricao.DocumentosEntregues = this.BuscarDocumentosEntreguesInscricao(seqInscricao);

            // Preenche os default model dos mestre detalhes com valores padrões
            resumoInscricao?.DocumentosEntregues?.DocumentosObrigatorios?.ForEach(d => d.InscricaoDocumentos.DefaultModel = new InscricaoDocumentoViewModel 
            { 
                VersaoDocumentoExigido = d.VersaoDocumentoExigido, 
                Obrigatorio = d.Obrigatorio, 
                SeqDocumentoRequerido = d.SeqDocumentoRequerido, 
                SeqInscricao = d.SeqInscricao, 
                SeqTipoDocumento = d.SeqTipoDocumento, 
                SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega, 
                SituacaoEntregaDocumentoInicial = SituacaoEntregaDocumento.AguardandoEntrega,
                ExibirExibirObservacaoParaInscrito = d.Obrigatorio,
                ExibirInformacaoExibirObservacaoParaInscrito = d.Obrigatorio
                
            });

            resumoInscricao?.DocumentosEntregues?.DocumentosOpcionais?.ForEach(d => d.InscricaoDocumentos.DefaultModel = new InscricaoDocumentoViewModel 
            { 
                VersaoDocumentoExigido = d.VersaoDocumentoExigido, 
                Obrigatorio = d.Obrigatorio, 
                SeqDocumentoRequerido = d.SeqDocumentoRequerido, 
                SeqInscricao = d.SeqInscricao, 
                SeqTipoDocumento = d.SeqTipoDocumento, 
                SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega, 
                SituacaoEntregaDocumentoInicial = SituacaoEntregaDocumento.AguardandoEntrega 
            });

            resumoInscricao?.DocumentosEntregues?.GruposDocumentos?.SelectMany(g => g.DocumentosRequeridosGrupo)?.ToList().ForEach(d => d.InscricaoDocumentos.DefaultModel = new InscricaoDocumentoViewModel 
            { 
                VersaoDocumentoExigido = d.VersaoDocumentoExigido,
                Obrigatorio = d.Obrigatorio, 
                SeqDocumentoRequerido = d.SeqDocumentoRequerido,
                SeqInscricao = d.SeqInscricao, 
                SeqTipoDocumento = d.SeqTipoDocumento, 
                SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega, 
                SituacaoEntregaDocumentoInicial = SituacaoEntregaDocumento.AguardandoEntrega,
                ExibirExibirObservacaoParaInscrito = d.Obrigatorio,
                ExibirInformacaoExibirObservacaoParaInscrito = d.Obrigatorio
            });

            return resumoInscricao;
        }

        public SumarioDocumentosEntreguesViewModel BuscarDocumentosEntreguesInscricao(long seqInscricao)
        {
            var docs = this.InscricaoService.BuscarSumarioDocumentosEntregue(seqInscricao);
            var ret = docs.Transform<SumarioDocumentosEntreguesViewModel>();
           
            return ret;
        }

        public List<DocumentoInscricaoViewModel> BuscarDocumentosInscricao(long seqInscricao)
        {
            var docs = this.InscricaoService.BuscarDocumentosInscricao(seqInscricao);
            return docs.TransformList<DocumentoInscricaoViewModel>();
        }

        public DocumentoEntregueViewModel SalvarDocumentoInscricao(DocumentoEntregueViewModel modelo)
        {
            return SMCMapperHelper.Create<DocumentoEntregueViewModel>(
                InscricaoService.SalvarDocumentoInscricao(
                    SMCMapperHelper.Create<InscricaoDocumentoData>(modelo)));
        }

        /// <summary>
        /// Salva todos os documentos entregues
        /// </summary>
        /// <param name="SumarioDocumentosEntreguesViewModel"></param>
        /// <returns>SeqProcesso</returns>
        public long SalvarSumarioDocumentosEntreguesInscricao(SumarioDocumentosEntreguesViewModel documentosEntregues)
        {
            return this.InscricaoService.SalvarSumarioDocumentosEntreguesInscricao(documentosEntregues.Transform<SumarioDocumentosEntreguesData>());
        }

        public bool ValidarSituacaoAtualCandidatoOfertasConfirmadas(SumarioDocumentosEntreguesViewModel documentosEntregues)
        {
            return this.InscricaoService.ValidarSituacaoAtualCandidatoOfertasConfirmadas(documentosEntregues.Transform<SumarioDocumentosEntreguesData>());
        }

        public bool ValidarSituacaoAtualCandidatoOfertasDeferidas(SumarioDocumentosEntreguesViewModel documentosEntregues)
        {
            return this.InscricaoService.ValidarSituacaoAtualCandidatoOfertasDeferidas(documentosEntregues.Transform<SumarioDocumentosEntreguesData>());
        }

        public void DuplicarDocumentoEntregue(long seqInscricaoDocumento)
        {
            InscricaoService.DuplicarEntregaDocumento(seqInscricaoDocumento);
        }

        public bool VerificarSituacaoRegistrarEntregaDocumentos(long seqInscricao)
        {
            return InscricaoService.VerificarSituacaoRegistrarEntregaDocumentos(seqInscricao);
        }

        /// <summary>
        /// Exclui um documento para uma inscrição
        /// A operação só deve ser possível se o documento em questão permirtir a entrega de mais de um documento
        /// e se houver outro documento do mesmo tipo já entregue, acs contrário deve lançar uma exceção com
        /// a mensagem
        /// "Para o documento {0} é necessário existir ao menos um registroPara o documento <descrição do tipo de documento excluído>" é necessário existir ao menos um registro"
        /// </summary>
        /// <param name="seqInscricaoDocumento"></param>
        public void ExcluirInscricaoDocumento(long seqInscricaoDocumento)
        {
            InscricaoService.ExcluirInscricaoDocumento(seqInscricaoDocumento);
        }

        public SMCFile DownloadDocumentos(long seqInscricao)
        {
            var registroDocumentos = this.BuscarDocumentosInscricao(seqInscricao);

            List<SMCFile> documentos = new List<SMCFile>();
            foreach (var item in registroDocumentos)
            {
                if (item.SeqArquivoAnexado.HasValue)
                {
                    var arquivo = this.BuscarArquivo(item.SeqArquivoAnexado.Value);

                    if (arquivo.FileData == null)
                    {
                        documentos.Add(new SMCFile
                        {
                            Nome = arquivo.Name + "_expurgo.pdf",
                            Conteudo = CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF,
                            Description = arquivo.Description,
                            Tipo = "application/pdf",
                            Tamanho = CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF.Length
                        });
                    }
                    else
                    {
                        documentos.Add(new SMCFile
                        {
                            Nome = arquivo.Name,
                            Conteudo = arquivo.FileData,
                            Description = arquivo.Description,
                            Tipo = arquivo.Type,
                            Tamanho = (int)arquivo.Size
                        });
                    }
                }
            }

            if (documentos.Count > 0)
            {
                byte[] zip = SMCFileHelper.ZipFiles(documentos.ToArray());
                return new SMCFile() { Conteudo = zip, Nome = "ArquivosAnexados.zip" };
            }
            return null;
        }

        #endregion Documentos Inscrição

        #region Consulta de Inscrição

        public void PreencherInscricaoRelatorio(InscricaoViewModel model)
        {
            var resumoInscricao = this.InscricaoService.BuscarInscricaoResumida(model.SeqInscricao, false);

            model.SeqInscrito = resumoInscricao.SeqInscrito;
            model.SeqProcesso = resumoInscricao.SeqProcesso;
            model.SeqArquivoComprovante = resumoInscricao.SeqArquivoComprovante;
            model.DescricaoGrupoOferta = resumoInscricao.DescricaoGrupoOferta;
            model.NumeroOpcoesDesejadas = resumoInscricao.NumeroOpcoesDesejadas;
            model.Ofertas = resumoInscricao.DescricaoOfertas.TransformList<OfertaInscricaoViewModel>();
            model.DadosInscrito = this.BuscarDadosInscrito(resumoInscricao.SeqInscrito);
            model.Observacao = resumoInscricao.Observacao;
            model.SituacaoInscrito = resumoInscricao.SituacaoInscrito;
            model.TokenSituacaoInscrito = resumoInscricao.TokenSituacaoInscrito;
            model.CandidatoComBoletoPago = resumoInscricao.CandidatoComBoletoPago;
            model.OfertaVigente = resumoInscricao.OfertaVigente;
            model.Documentos = new SMCPagerModel<DocumentoInscricaoViewModel>(
                this.BuscarDocumentosInscricao(model.SeqInscricao));
            model.RecebeuBolsa = resumoInscricao.RecebeuBolsa;
            model.BolsaExAluno = TipoProcessoService.BuscarTipoProcessoPorProcesso(resumoInscricao.SeqProcesso).BolsaExAluno;

            var codigosData = InscricaoService.BuscarInscricaoCodigoAutorizacao(model.SeqInscricao);
            foreach (var codigo in codigosData)
            {
                model.CodigosAutorizacao.Add(codigo.Codigo);
            }
            model.Titulos = this.BuscarTitulosInscricao(model.SeqInscricao);
            var dadosFormularios = this.InscricaoService.BuscarInscricaoDadoFormulario(model.SeqInscricao);
            foreach (var dado in dadosFormularios)
            {
                model.Formularios.Add(new InscricaoDadoFormularioListaViewModel()
                {
                    SeqDadoFormulario = dado.Seq,
                    SeqInscricao = dado.SeqInscricao,
                    TituloFormulario = dado.TituloFormulario,
                    SeqFormularioSGF = dado.SeqFormulario,
                    SeqVisaoSGF = dado.SeqVisao,
                    Editable = dado.Editable
                });
            }
        }

        /// <summary>
        /// Busca os dados do inscrito do usuário logado para consulta
        /// </summary>
        /// <returns>Dados do inscrito do usuário logado</returns>
        public DadosInscritoViewModel BuscarDadosInscrito(long seqInscrito)
        {
            InscritoData data = InscritoService.BuscarInscrito(seqInscrito);

            // Cria o viewModel a partir do data
            DadosInscritoViewModel model = SMCMapperHelper.Create<DadosInscritoViewModel>(data);

            // Busca o pais da nacionalidade
            PaisData paisData = LocalidadeService.BuscarPais(data.CodigoPaisNacionalidade);
            model.PaisNacionalidade = paisData != null ? paisData.Nome : string.Empty;

            // Busca o nome da cidade da naturalidade
            if (data.CodigoCidadeNaturalidade.HasValue && !string.IsNullOrEmpty(data.UfNaturalidade))
            {
                CidadeData dataCidade = LocalidadeService.BuscarCidade(data.CodigoCidadeNaturalidade.Value, data.UfNaturalidade);
                model.CidadeNaturalidade = dataCidade != null ? dataCidade.Nome : string.Empty;
            }

            if (model.Enderecos != null)
            {
                // Ajusta paises do endereço
                foreach (var endereco in model.Enderecos)
                {
                    paisData = LocalidadeService.BuscarPais(endereco.SeqPais);
                    endereco.DescPaisSelecionado = paisData.Nome;
                }
            }

            return model;
        }

        // GET: /BoletoBancario/ImagemBanco/?codigoBanco=1
        public byte[] BuscarImagemBanco(int codigoBanco)
        {
            return FinanceiroService.BuscarImagemBanco(codigoBanco);
        }

        // GET: /BoletoBancario/ImagemCodigoBarras/codigoBarras=123456
        public byte[] BuscarImagemCodigoBarras(string codigoBarras)
        {
            return FinanceiroService.BuscarImagemCodigoBarras(codigoBarras);
        }

        public BoletoData BuscarTitulo(int seqTitulo)
        {
            SMCLogger.Information(
                String.Format("Iniciando o serviço do financeiro - GPI.Administrativo - Título :{0} ", seqTitulo));
            var boleto = FinanceiroService.BuscarBoletoCrei(new BoletoFiltroData()
            {
                SeqTitulo = seqTitulo,
                Crei = true,
                Sistema = SistemaBoleto.GPI
            });
            SMCLogger.Information(
             String.Format("Finalizando o serviço do financeiro - GPI.Administrativo - Título :{0} ", seqTitulo));
            boleto.ImagemBanco = SMCImageHelper.ImageToBase64(BuscarImagemBanco(boleto.Banco.Numero));
            boleto.ImagemCodigoBarras = SMCImageHelper.ImageToBase64(BuscarImagemCodigoBarras(boleto.CodigoBarras));

            return boleto;
        }

        /// <summary>
        /// Busca os títulos existentes para um inscrição
        /// </summary>
        public SMCPagerModel<TituloInscricaoViewModel> BuscarTitulosInscricao(long seqInscricao)
        {
            var listaVM = this.InscricaoService.BuscarTitulosInscricao(seqInscricao)
                .TransformList<TituloInscricaoViewModel>();
            return new SMCPagerModel<TituloInscricaoViewModel>(listaVM);
        }

        public void ExcluirInscricao(long seqInscricao)
        {
            this.InscricaoService.ExcluirInscricao(seqInscricao);
        }

        public bool PossuiBoletoPago(long seqInscricao)
        {
            return InscricaoService.PossuiBoletoPago(seqInscricao);
        }

        public bool PossuiBoleto(long seqInscricao)
        {
            return InscricaoService.PossuiBoleto(seqInscricao);
        }

        public bool PossuiOfertaVigente(long seqInscricao)
        {
            return InscricaoService.PossuiOfertaVigente(seqInscricao);
        }

        #endregion Consulta de Inscrição

        #region Liberar Alteracao da Inscricao

        public void LiberarAlteracaoInscricao(long seqInscricao)
        {
            InscricaoService.LiberarAlteracaoInscricao(seqInscricao);
        }

        /// <summary>
        /// Validar RN_INS_141 Liberação da alteração de inscrição
        /// Regras 1 e 2
        /// </summary>
        /// <param name="seqInscricao"></param>
        public void ValidarLiberacaoAlteracaoInscricao(long seqInscricao)
        {
            InscricaoService.ValidarLiberacaoAlteracaoInscricao(seqInscricao);
        }

        #endregion Liberar Alteracao da Inscricao

        public SMCUploadFile BuscarArquivo(long seq)
        {
            return ArquivoAnexadoService.BuscarArquivoAnexado(seq);
        }

        public bool VerificaHistoricoInscricaoConfirmada(long seqInscricaoDocumento, long seqInscricao)
        {
            var historicosInscricao = InscricaoService.BuscarSituacoesInscricaoParaValidacaoDeTokens(seqInscricao);
            // Verifica candidato com situação atual como INSCRICAO_CONFIRMADA
            if (historicosInscricao.Any(f => f.Atual && f.TokenSituacao == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA && f.TokenEtapa == TOKENS.ETAPA_INSCRICAO))
            {
                var historicosInscricaoOferta = InscricaoService.BuscarSituacoesInscricaoOferta(seqInscricao);
                // Caso todas as situações do historico de situação da inscrição oferta atual estejam com o token CANDIDADO_CONFIRMADO
                if (historicosInscricaoOferta.Where(f => f.Atual).All(f => f.TokenSituacao == TOKENS.SITUACAO_CANDIDATO_CONFIRMADO))
                {
                    if (InscricaoService.VerificaDocumentacaoEntregue(seqInscricao))
                    {
                        return InscricaoService.VerificaDocumentoObrigatorio(seqInscricaoDocumento, seqInscricao);
                    }
                }
            }
            return false;
        }

        public bool VerificarHistoricoInscricaoConfirmadaEAvancada(long seqInscricaoDocumento, long seqInscricao)
        {
            var historicos = InscricaoService.BuscarSituacoesInscricaoParaValidacaoDeTokens(seqInscricao);
            var historicosInscricaoOferta = InscricaoService.BuscarSituacoesInscricaoOferta(seqInscricao);
            if (historicos.Any(f => f.Atual && (f.TokenSituacao == TOKENS.SITUACAO_INSCRICAO_DEFERIDA ||
                                                (f.TokenSituacao == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA && historicosInscricaoOferta
                                                                                .Any(x => x.Atual && x.TokenSituacao != TOKENS.SITUACAO_CANDIDATO_CONFIRMADO)))))
            {
                if (InscricaoService.VerificaDocumentacaoEntregue(seqInscricao))
                {
                    return InscricaoService.VerificaDocumentoObrigatorio(seqInscricaoDocumento, seqInscricao);
                }
            }
            return false;
        }

        public bool VerificaSituacaoInscricaoDiferenteCandidatoConfirmado(List<long> inscricoes)
        {
            return InscricaoService.VerificaSituacaoInscricoesOfertaNaSituacao(inscricoes, TOKENS.SITUACAO_CANDIDATO_CONFIRMADO);
        }

        public DateTime? BuscarDataLimiteEntregaDocumentoRequerido(long seqDocumentoRequerido)
        {
            return DocumentoRequeridoService.BuscarDataLimiteEntregaDocumentoRequerido(seqDocumentoRequerido);
        }

    }
}