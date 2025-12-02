using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface ITipoProcessoService : ISMCService
    {
        /// <summary>
        /// Retorna todos os tipos de processo com apenas seq e descricao preenchidos
        /// Serviço utilizado pelo Novo SGA nas configurações de Campanha (Exposto WCF)
        /// </summary> 
        /// <param name="seqUnidadeResponsavel">Parametro opcional para filtrar por unidade responsável</param>
        /// <returns>Lista de Tipos de processo para apresentação em SELECT</returns>
        [OperationContract]
        List<SMCDatasourceItem> BuscarTiposProcessoKeyValue(long? seqUnidadeResponsavel = null);

        /// <summary>
        /// Busca os tipoProcessoSituacao de destino a partir de um tipo processo situaão de origem
        /// (usado para mudança de situação de inscrições)
        /// </summary>        
        SMCDatasourceItem[] BuscarTipoProcessoSitucaoDestinoKeyValue(long seqTipoProcessoSituacao, long? seqProcesso = null, bool throwWhenEmpty = true);

        /// <summary>
        /// Busca as situações destino permitidas para uma inscrição.
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <param name="tokenSituacao">Token da situação atual</param>
        /// <param name="tokenEtapa">Token da etapa</param>
        /// <param name="verificaPermiteRetornar">Flag de permissão para retornar</param>
        /// <param name="retornaSituacaoSGF">Flag para retornar a situação do SGF ou do tipo-processo-situação</param>
        /// <returns>Lista de situações</returns>
        SMCDatasourceItem[] BuscarTipoProcessoSitucaoDestinoPorToken(long seqProcesso, string tokenSituacao, string tokenEtapa, bool verificaPermiteRetornar = false, bool retornaSituacaoSGF = false);

        /// <summary>
        /// Busca o par chave e valor (Seq e Descriçãod da situação) para o sequencial informado
        /// </summary>        
        SMCDatasourceItem BuscarTipoProcessoSituacaoKeyValue(long seqTipoProcessoSituacao);

        /// <summary>
        /// Retorna a lista de tipos de processo para exibição em lista.
        /// </summary>
        SMCPagerData<TipoProcessoListaData> BuscarTiposProcesso(TipoProcessoFiltroData filtroData);

        long SalvarTipoProcesso(TipoProcessoData modelo);

        TipoProcessoData BuscarTipoProcesso(long seqTipoProcesso);

        void ExcluirTipoProcesso(long seqTipoProcesso);

        List<SMCDatasourceItem> BuscarTemplatesProcessoAssociados(long seqTipoProcesso);

        List<SMCDatasourceItem> BuscarTiposTaxaAssociados(long seqTipoProcesso);

        TipoProcessoSituacaoData BuscarTipoProcessoSituacao(long seqTipoProcessoSituacao);

        bool VerificaPossuiConsistencia(TipoProcessoConsistenciaData filtro);

        TipoProcessoSituacaoData BuscarTipoProcessoSituacaoAnterior(long seqInscricao);

        /// <summary>
        /// Buscar tipo de processo de um processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial Processo</param>
        /// <returns>Tipo de processo</returns>
        TipoProcessoData BuscarTipoProcessoPorProcesso(long seqProcesso);

        /// <summary>
        /// Busca tipo de processo de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Tipo de processo</returns>
        TipoProcessoData BuscarTipoProcessoPorInscricao(long seqIncricao);

        /// <summary>
        /// Verifica se tem integração com o GPC
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns></returns>
        bool VerificaIntegraGPC(long seqProcesso);

        /// <summary>
        /// Busca a descrição dos tipos de documentos configurados no tipo de processo do processo.
        /// </summary>
        /// <param name="seqTipoProcesso">Sequencial do tipo de processo</param>
        /// <returns>Itens para o select de tipo de documento</returns>
        List<SMCDatasourceItem> BuscarTiposDocumentoSelect(long seqTipoProcesso);

        /// <summary>
        /// Confere se a flag “Habilita gestão de eventos” está ativada no tipo processo selecionado.
        /// </summary>
        /// <param name="seqTipoProcesso">Sequencial do tipo de processo</param>
        /// <returns>Verdadeiro ou falso para exibição dos campos de data de evento</returns>
        bool ConferirHabilitaDatasEvento(long seqTipoProcesso);

         
        /// <summary>
        /// Confere se a flag “Habilita gestão de eventos” está ativada no tipo processo selecionado.
        /// </summary>
        /// <param name="seqTipoProcesso">Sequencial do tipo de processo</param>
        /// <returns>Verdadeiro ou falso para exibição do campo Habilita GED</returns>
        bool ConferirHabilitaGestaoEvento(long seqTipoProcesso);
    }
}
