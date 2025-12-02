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
    public interface IOfertaService : ISMCService
    {
        List<HierarquiaOfertaData> BuscarArvoreOfertaCompleta(long seqProcesso, long? seqPai, long[] expandedNodes);

        List<HierarquiaOfertaData> BuscarArvoreHierarquiaOfertaProcesso(long seqProcesso, long? seqGrupoOferta);

        List<HierarquiaOfertaData> BuscarArvoreHierarquiaOfertaProcessoComGrupo(long seqProcesso, long? seqGrupoOferta);

        HierarquiaOfertaData[] BuscarArvoreOfertas(LookupOfertaFiltroData filtro);

        HierarquiaOfertaData[] BuscarArvoreOfertasInscricao(long seqGrupoOferta);

        SelecaoOfertaInscricaoData[] BuscarArvoreSelecaoOfertasInscricao(long seqGrupoOferta);
        SelecaoOfertaInscricaoData[] BuscarHierarquiaCompletaAngular(long seqOferta);

        SMCDatasourceItem[] BuscarOfertaKeyValue(long seq);

        SMCDatasourceItem[] BuscarSelecaoOfertasInscricaoKeyValue(long seqOferta);
        SMCDatasourceItem[] BuscarDescricaoSelecaoOfertasInscricaoSeqsOfertas(long[] seqsOfertas);

        List<HierarquiaOfertaData> BuscarHierarquiaOfertaComGrupo(HierarquiaOfertaComGrupoFiltroData filtro);

        /// <summary>
        /// Retorna os sequenciais das ofertas vigentes e ativas para um grupo de oferta
        /// </summary>    
        List<long> BuscarSeqOfertasVigentesAtivas(long SeqGrupoOferta);

        /// <summary>
        /// Verifica se uma das raizes da hierarquia de ofertas do processo é oferta
        /// </summary>                
        bool VerificarRaizHierarquiaOfertaPermiteOferta(long seqProcesso);

        /// <summary>
        /// Verifica se uma das raizes da hierarquia de ofertas do processo é oferta
        /// </summary>
        bool VerificarRaizHierarquiaOfertaPermiteItem(long seqProcesso);

        /// <summary>
        /// Busca uma oferta com os dados completos
        /// </summary>        
        OfertaData BuscarOferta(long seqOferta);

        /// <summary>
        /// Busca uma oferta com os dados completos
        /// </summary>        
        HierarquiaOfertaData BuscarHierarquiaOferta(long seqHierarquiaOferta);

        /// <summary>
        /// Salva uma hierarquia de oferta( item da árvore de hierarquia de ofertas) de um processo
        /// </summary>        
        long SalvarHierarquiaOferta(HierarquiaOfertaData hierarquiaOferta);

        /// <summary>
        /// Salva uma oferta( item da árvore de hierarquia de ofertas) de um processo
        /// </summary>        
        long SalvarOferta(OfertaData oferta);

        /// <summary>
        /// Exclui um item de hierarquia de oferta
        /// </summary>        
        void ExcluirHierarquiaOferta(long seqHierarquiaOferta);

        /// <summary>
        /// Exclui um item de hierarquia de oferta
        /// </summary>        
        void ExcluirOferta(long seqOferta);

        SMCPagerData<OfertaTaxaData> BuscarOfertasTaxas(OfertaTaxaFiltroData filtro);

        List<SMCDatasourceItem<string>> BuscarPeriodosOfertas(long seqProcesso);

        IEnumerable<SMCDatasourceItem> BuscarOfertasPeriodoTaxaKeyValue(OfertaPeriodoTaxaFiltroData filtro);

        void ExcluirTaxaOfertaEmLote(long seqTipoTaxa, List<long> seqOfertas);

        SMCDatasourceItem[] BuscarOfertasKeyValue(List<long> seqOfertas);

        void IncluirTaxasLote(IncluirTaxaEmLoteData modelo);

        /// <summary>
        /// Busca o sdetalhes da oferta e a quantidade de inscrições confirmadas para a oferta
        /// Inicialmente usado na tela de prorrogação de etapa do processo
        /// </summary>
        IEnumerable<OfertaPeriodoInscricaoData> BuscarOfertasInscricoes(long[] seqOfertas);

        List<OfertaPeriodoInscricaoData> BuscarContagemInscrioesDasOfertasPorSituacao(long[] seqOfertas, string[] tokens);

        OfertaPeriodoInscricaoData BuscarOfertaInscricoes(long seqOferta);

        /// <summary>
        /// Verifica se a inclusão de itens de hierarquia de oferta é permitida
        /// </summary>        
        void VerificarPermissaoCadastrarHierarquia(long seqProcesso);

        string BuscarDescricaoTaxa(long seqTaxa);

        OfertaData BuscarUltimaOfertaCadastrada(long seqProcesso);

        /// <summary>
        /// Buscar a lista de ofertas a serem consolidadas no checkin
        /// </summary>
        /// <param name="filtro">Parametros dos filtros</param>
        /// <returns>List de ofertas</returns>
        SMCPagerData<AcompanhamentoInscritoCheckinListaData> BuscarPosicaoConsolidadaCheckin(AcompanhamentoCheckinFiltroData filtro);
    }
}
