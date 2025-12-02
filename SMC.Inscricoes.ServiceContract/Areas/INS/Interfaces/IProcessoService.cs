using System;
using System.Collections.Generic;
using System.ServiceModel;
using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IProcessoService : ISMCService
    {
        #region Home Processo

        /// <summary>
        /// Busca os processos que estão com etapa de inscrição em aberto por idioma
        /// </summary>
        /// <param name="filtro">Filtro para pesquisa</param>
        /// <returns>Lista de etapas de inscrição de processos em aberto</returns>
        SMCPagerData<EtapaProcessoAbertoListaData> BuscarProcessosComInscricoesEmAberto(EtapaProcessoAbertoFiltroData filtro);

        /// <summary>
        /// Busca as informações de um processo para exibição na home
        /// </summary>
        /// <param name="uidProcesso">Uid do processo a ser recuperado</param>
        /// <param name="idioma">Idioma para recuperar as informações</param>
        /// <returns>Informações de um processo para exibição na home</returns>
        ProcessoHomeData BuscarProcessoHome(Guid uidProcesso, SMCLanguage idioma, long? seqInscrito);

        string BuscarCssProcesso(Guid uidProcesso);

        #endregion Home Processo

        #region Posição consolidada

        /// <summary>
        /// Busca a posição consolidada para cada processo de acordo com o filtro informado
        /// </summary>
        /// <param name="filtro">Filtro da pesquisa</param>
        /// <returns>Lista de processos com a posição consolidada sumarizada</returns>
        SMCPagerData<PosicaoConsolidadaProcessoData> BuscarPosicaoConsolidadaProcesso(ProcessoFiltroData filtro);

        /// <summary>
        /// Busca a posição consolidada de um processo
        /// </summary>
        /// <returns>Lista de processos com a posição consolidada sumarizada</returns>
        PosicaoConsolidadaProcessoData BuscarPosicaoConsolidadaProcesso(long seqProcesso);

        #endregion Posição consolidada

        #region Busca dados processo

        /// <summary>
        /// Buscar as quantidades de oferta do processo
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração de etapa do processo</param>
        /// <returns>Quantidades de oferta do processo</returns>
        QuantidadeOfertaProcessoData BuscarQuantidadeOfertaProcesso(long seqConfiguracaoEtapa);

        /// <summary>
        /// Busca as informações de um processo para um cabeçalho
        /// </summary>
        ProcessoCabecalhoData BuscarCabecalhoProcesso(long seqProcesso);

        /// <summary>
        /// Busca as informações de processo de acordo com os filtros
        /// </summary>
        SMCPagerData<ProcessoListaData> BuscarProcessos(ProcessoFiltroData filtro);

        /// <summary>
        /// Busca um processo completo para edição
        /// </summary>
        ProcessoData BuscarProcesso(long seqProcesso);

        CopiaProcessoData BuscarProcessoCopia(long seqProcesso);

        /// <summary>
        /// Busca os sequenciais dos templates dos processos dado um conjunto de sequenciais de processos
        /// </summary>
        /// <param name="seqsProcessos">Sequenciais dos processos</param>
        /// <returns>Lista dos templates dos processos</returns>
        [OperationContract]
        List<ProcessoTemplateProcessoSGFData> BuscarSeqsTemplatesProcessosSGF(long[] seqsProcessos);

        #endregion Busca dados processo

        #region Situacao Processo

        /// <summary>
        /// Retorna as situçãoes que são permitidas para uma inscrição no processo informado
        /// </summary>
        IEnumerable<SMCDatasourceItem> BuscarSituacoesProcessoKeyValue(long seqProcesso);

        /// <summary>
        /// Retorna os TipoProcessoSituação permitidos para uma inscrição no processo e etapas informados
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns>Sequencial e descrição dos TipoProcessoSituação</returns>
        IEnumerable<SMCDatasourceItem> BuscarSituacoesProcessoPorEtapaKeyValue(long seqProcesso, params string[] tokensEtapa);

        /// <summary>
        /// Verifica se o processo possui integração com outro sistema.
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        bool VerificaProcessoPossuiIntegracao(long seqProcesso);

        #endregion Situacao Processo

        /// <summary>
        /// Busca a descrição de ofertas para um determinado processo num idioma pela configuracao etapa
        /// </summary>
        string BuscarDescricaoOfertaProcesso(long seqConfiguracaoEtapa, SMCLanguage idioma);

        #region CRUD

        /// <summary>
        /// Salva um processo no banco
        /// </summary>
        long SalvarProcesso(ProcessoData processo);

        /// <summary>
        /// Exclui um processo do sistema
        /// </summary>
        void ExcluirProcesso(long seqProcesso);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        CopiaProcessoRetornoData CopiarProcesso(CopiaProcessoData copiaProcessoData);

        #endregion CRUD

        /// <summary>
        /// Busca os tipo de item de hierarquia oferta para um processo
        /// </summary>
        List<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaKeyValue(long seqProcesso, long? seqPai, bool HabilitaCadastroOferta);

        /// <summary>
        /// Busca  as taxas configuradas para um processo
        /// </summary>
        IEnumerable<SMCDatasourceItem> BuscarTaxasKeyValue(long seqProcesso);

        /// <summary>
        /// Busca o evento financeiro para um processo
        /// </summary>
        int? BuscarEventoProcesso(long seqProcesso);

        [OperationContract]
        SMCPagerData<ProcessoLookupData> BuscarProcessoLookup(ProcessoLookupFiltroData filtro);

        [OperationContract]
        ProcessoLookupData BuscarProcessoLookupSelect(long seq);

        [OperationContract]
        List<long> BuscarSeqsFormulariosDoProcesso(long? seqOferta, long? seqGrupoOferta, long seqProcesso);

        long? BuscarUnidadeResponsavelNotificacao(long seqProcesso);

        /// <summary>
        /// REtorna o sequencial do tipo de template de processo de um determinado processo
        /// </summary>
        long BuscarTipoTemplateProcesso(long seqProcesso);

        /// <summary>
        /// Verifica se é permitido cadastrar período taxa em lote para um processo
        /// </summary>
        void VerificarConsistenciaCadastroPeriodoTaxaEmLote(long seqProcesso);

        /// <summary>
        /// Compara se dois processos possuem algum idioma em comum
        /// </summary>
        /// <returns>
        /// false: se os processos não tiverem NENHUM idioma em comum
        /// true : se os processos tiverem AO MENOS UM idioma em comum
        /// </returns>
        bool CompararIdiomasProcesso(long seqProcessoDestino, long seqProcessoOrigem);

        long[] BuscarInscricoesProcesso(long seqProcesso);

        bool? VerificaTipoTaxaCobraPorQtdOferta(long seqTipoTaxa);

        bool ExibeArvoreFechada(long seqProcesso);

        #region GTI Now

        /// <summary>
        /// Serviço utilizado pelo GTINow para consulta de informações
        /// Deve ficar exposto para uso WCF
        /// </summary>
        /// <param name="rangeDias">Periodo em dias para buscar as informações</param>
        /// <returns>Lista de total de inscrições por processo</returns>
        [OperationContract]
        IEnumerable<TotalInscricoesProcessoData> BuscarTotalInscricaoesProcessos(int rangeDias);

        /// <summary>
        /// Serviço utilizado pelo GTINow para consutlar informações
        /// Deve ficar exposto para uso WCF
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo para recuperar informações sumarizadas</param>
        /// <returns>Resumo das informações do processo sumarizada</returns>
        [OperationContract]
        ResumoInscricoesProcessoData BuscarSituacaoInscricoesProcessoSumarizada(long seqProcesso);

        #endregion GTI Now

        [OperationContract]
        void IntegracaoProcesso(long seqProcesso, bool possuiIntegracao);

        /// <summary>
        /// Serviço utilizado pelo SGA durente o processo de cópia de campanha
        /// Retorna se o tipo de processo está ativo na unidade responsavel
        /// Retorna se um tipo de hierarquia está ativo na unidade responsavel
        /// </summary>
        /// <param name="seqProcessos">Sequenciais dos processos para verificação</param>
        /// <param name="seqUnidadeResponsavel">Sequencial da unidade responsavel para verificação</param>
        /// <returns>Retorna uma lista de processos com seus tipos processos e tipos hierarquias e se estes tipos encontra-se ativos na unidade responsável</returns>
        [OperationContract]
        IEnumerable<SituacaoCopiaCampanhaProcessoGpiData> BuscarSituacoesCopiaCampanhaProcessoGpi(long[] seqsUnidadesResponsaveis, long[] seqProcessos);
        
        /// <summary>
        /// Verifica se um tipo de processo de algum processo específico está configurado para integrar com o SGA legado.
        /// </summary>
        bool VerificarIntegracaoLegado(long seqProcesso);

        [OperationContract]
        List<SMCDatasourceItem> BuscarProcessosIntegraGPC(bool integraGPC);

        List<SMCDatasourceItem> BuscarProcessoSelect(ProcessoCandidatoFiltroData filtro);

        /// <summary>
        /// Buscar as configurações de documento disponíveis no GAD para o Sistema/Origem configurado na unidade responsável do processo
        /// </summary>
        List<SMCDatasourceItem<string>> BuscarConfiguracoesAssinaturaGadSelect(long seqUnidadeResponsavel);

        bool ValidarFormulariosAssert(ProcessoData processo);

        bool ValidarAssertDocumentoEmitido(ProcessoData processo);
        List<ProcessoGestaoEventoQRCodeData> BuscarProcessoHierarquiaLeituraQRCode();

        /// <summary>
        /// Busca as de situações do processo
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns>Retorna uma lista de situações do processo</returns>
        List<string> BuscarSituacoesProcesso(long seqProcesso);

        SMCUploadFile BuscarArquivoAnexadoConfigurancaoEmissaoDocumento(long seqArquivo);

        bool ProcessoPossuiTaxa(long seqProcesso);

        bool ValidarUsuarioInscritoAssociadoInscricao(long seqInscricao, long seqInscrito);

        long BuscarSeqProcessoPorSeqInscricao(long seqInscricao);

        bool ValidarPermissaoEmitirDocumentacao(long seqProcesso, long seqInscricao, long seqInscrito, long seqTipoDocumento);
    }
}