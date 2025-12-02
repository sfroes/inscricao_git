using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.Framework;
using SMC.Financeiro.ServiceContract.BLT.Data;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IAcompanhamentoProcessoControllerService
    {

        #region Consulta Posição Consolidada por Processo

        /// <summary>
        /// Busca a posição consolidada dos processo de acordo com os filtros informados
        /// </summary>
        /// <param name="filtros">Filtros para pesquisa</param>
        /// <returns>Lista consolidada de processos</returns>
        SMCPagerModel<ConsultaConsolidadaProcessoListaViewModel> BuscarPosicaoConsolidadaProcessos(ConsultaConsolidadaProcessoFiltroViewModel filtros);

        /// <summary>
        /// Busca a Posição Consolidada de um processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns></returns>
        ConsultaConsolidadaProcessoCabecalhoViewModel BuscarPosicaoConsolidadaProcesso(long seqProcesso);

        /// <summary>
        /// Buscar a posição consolidada dos processos por grupo de oferta
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        /// <returns>Lista consolidada de ofertas / grupo de ofertas</returns>
        SMCPagerModel<ConsultaConsolidadaGrupoOfertaListaViewModel> BuscarPosicaoConsolidadaPorGrupoOferta(ConsultaConsolidadaGrupoOfertaFiltroViewModel filtros);


        #endregion

        #region Consulta de Inscrições do Processo

        /// <summary>
        /// Busca as inscrições do processo pelos filtros 
        /// </summary>
        /// <param name="filtros">Filtros para pesquisa de inscrições do processo</param>
        /// <returns>Lista de inscrições do processo</returns>
        SMCPagerModel<ConsultaInscricaoProcessoListaViewModel> BuscarInscricoesProcesso(ConsultaInscricaoProcessoFiltroViewModel filtros);

        /// <summary>
        /// Busca as informações de cabeçalho das inscrições do processo pelo sequencial 
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns></returns>
        ProcessoCabecalhoViewModel BuscarCabecalhoProcesso(long seqProcesso);

        SMCPagerModel<ConsultaInscricaoProcessoInscritoListaViewModel> BuscarInscricoesProcessoExcel(ConsultaInscricaoProcessoFiltroViewModel filtros);

        #endregion

        #region Análise de Inscição em Lote

        /// <summary>
        /// Busca as inscrições do processo pelos filtros, para analise em lote 
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        SMCPagerModel<AnaliseInscricaoLoteListaViewModel> BuscarInscricoesProcesso(AnaliseInscricaoLoteFiltroViewModel filtros);
        
        /// <summary>
        /// Busca as inscrições para alteração de situação, conforme seleção do usuário
        /// </summary>
        /// <param name="seqInscricoesSelecionadas">Seq´s selecionados pelo usuário no grid de inscrições</param>
        /// <returns>Lista de inscrições para alteração de situação</returns>
        AlteracaoSituacaoViewModel BuscarInscricoesAlteracaoSituacao(InscricaoSelecionadaViewModel filtro);

        /// <summary>
        /// Salta das alterações de situação das inscrições
        /// </summary>
        /// <param name="modelo">Modelo contendo os dados para alteração</param>
        void SalvarAlteracaoSituacaoLote(AlteracaoSituacaoViewModel modelo);
        
        #endregion

		#region RegistroDocumentacaoEntregue

        /// <summary>
        /// Busca o registro de documentação para uma determinada inscrição
        /// </summary>        
		RegistroDocumentacaoViewModel BuscarRegistroDocumentacao(long seqInscricao);

        List<DocumentoInscricaoViewModel> BuscarDocumentosInscricao(long seqInscricao);

        DocumentoEntregueViewModel SalvarDocumentoInscricao(DocumentoEntregueViewModel modelo);        

        void DuplicarDocumentoEntregue(long seqInscricaoDocumento);

        bool VerificarSituacaoRegistrarEntregaDocumentos(long seqInscricao);

        SumarioDocumentosEntreguesViewModel BuscarDocumentosEntreguesInscricao(long seqInscricao);
        
        /// <summary>
        /// Exclui um documento para uma inscrição
        /// A operação só deve ser possível se o documento em questão permirtir a entrega de mais de um documento
        /// e se houver outro documento do mesmo tipo já entregue, acs contrário deve lançar uma exceção com
        /// a mensagem 
        /// "Para o documento {0} é necessário existir ao menos um registroPara o documento <descrição do tipo de documento excluído>" é necessário existir ao menos um registro"
        /// </summary>
        /// <param name="seqInscricaoDocumento"></param>
        void ExcluirInscricaoDocumento(long seqInscricaoDocumento);

        SMCFile DownloadDocumentos(long seqInscricao);
        #endregion

        #region Alteração de Situação

        List<SMCDatasourceItem> BuscarMotivosSituacao(long seqTipoProcessoSituacao);

        #endregion

        #region Histório de Situação
        List<HistoricoSituacaoListaViewModel> BuscarSituacoesInscricao(long seqInscricao);

        HistoricoSituacaoViewModel BuscarHistoricoSituacao(long seqHistoricoSituacao);

        long SalvarAlteracaoSituacao(HistoricoSituacaoViewModel model);
        #endregion

        #region Consulta de Inscrição

        /// <summary>
        /// Busca os dados do inscrito do usuário logado para consulta
        /// </summary>
        /// <returns>Dados do inscrito do usuário logado</returns>
        DadosInscritoViewModel BuscarDadosInscrito(long seqInscrito);        

        /// <summary>
        /// Busca a inscrição para a tela de visulização de inscrição
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void PreencherInscricaoRelatorio(InscricaoViewModel model);

        
        /// <summary>
        /// Busca os títulos existentes para um inscrição
        /// </summary>        
        SMCPagerModel<TituloInscricaoViewModel> BuscarTitulosInscricao(long seqInscricao);

        // GET: /BoletoBancario/ImagemBanco/?codigoBanco=1
        byte[] BuscarImagemBanco(int codigoBanco);        

        // GET: /BoletoBancario/ImagemCodigoBarras/codigoBarras=123456
        byte[] BuscarImagemCodigoBarras(string codigoBarras);

        #endregion

        SMCUploadFile BuscarArquivo(long seqArquivoAnexado);

        BoletoData BuscarTitulo(int seqTitulo);

        bool VerificaHistoricoInscricaoConfirmada(long seqInscricaoDocumento, long seqInscricao);

        bool VerificarHistoricoInscricaoConfirmadaEAvancada(long seqInscricaoDocumento, long seqInscricao);

        bool VerificaSituacaoInscricaoDiferenteCandidatoConfirmado(List<long> inscricoes);
    }
}