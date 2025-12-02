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
    public interface IEtapaProcessoService : ISMCService
    {
        /// <summary>
        /// Busca a lista de etapas de um processo filtrada
        /// </summary>
        SMCPagerData<EtapaProcessoListaData> BuscarEtapasProcesso(EtapaProcessoFiltroData filtro);

        EtapaProcessoData BuscarEtapaProcesso(long seqEtapaProcesso);

        void ExcluirEtapaProcesso(long seqEtapaProcesso);

        long SalvarEtapaProcesso(EtapaProcessoData etapaProcesso);

        SMCDatasourceItem[] BuscarEtapasSGFKeyValue(long seqProcesso);

        List<SMCDatasourceItem> BuscarSituacoesPermitidas(long seqEtapaProcesso);

        CabecalhoProcessoEtapaData BuscarCabecalhoProcessoEtapa(long seqEtapaProcesso);

        /// <summary>
        /// Verifica se a inclusão de etapas é permitida
        /// </summary>        
        void VerificarPermissaoCadastrarEtapa(long seqProcesso);

        /// <summary>
        /// Busca a lista de configurações das ofertas selecionadas bem como as taxas
        /// existentes para estas ofertas (DISTINCT)
        /// </summary>        
        List<ProrrogacaoConfiguracaoData> BuscarConfiguracoesProrrogacao(long seqEtapaProcesso
            , long[] seqOfertas);

        /// <summary>
        /// Recupera o sumário de uma prorrogação de processo para ser exibido para o usuário
        /// </summary>
        ProrrogacaoEtapaData SumarioProrrogacao(ProrrogacaoEtapaData etapaProrrogar);

        /// <summary>
        /// Prorroga a etapa informada com os dados passados no DTO
        /// </summary>
        /// <param name="etapaProrrogar"></param>
        void ProrrogarEtapa(ProrrogacaoEtapaData etapaProrrogar);

        /// <summary>
        /// Verifica se é possível prorrogar a etapa informada
        /// </summary>        
        void VerificarPossibilidadeProrrogacao(long seqEtapaProcesso);

        [OperationContract]
        bool ExisteEtapa(long seqProcesso, string token);
    }
}
