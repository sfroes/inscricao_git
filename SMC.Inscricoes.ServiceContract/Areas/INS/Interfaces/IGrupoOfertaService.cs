using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IGrupoOfertaService : ISMCService
    {       

        /// <summary>
        /// Busca a posição consolidada para cada grupo de oferta de um processo de acordo com o filtro informado
        /// </summary>
        /// <param name="filtro">Filtro da pesquisa</param>
        /// <returns>Lista de grupos de oferta com a posição consolidada sumarizada</returns>
        SMCPagerData<PosicaoConsolidadaGrupoOfertaData> BuscarPosicaoConsolidadaGruposOferta(PosicaoConsolidadaGrupoOfertaFiltroData filtro);

        /// <summary>
        /// Retorna o par de chave/valor para os grupos de oferta do processo informado
        /// </summary>        
        IEnumerable<SMCDatasourceItem> BuscaGruposOfertaKeyValue(long SeqProcesso);

        /// <summary>
        /// Retorna os grupos de oferta para exibição em lista.
        /// </summary>
        SMCPagerData<GrupoOfertaListaData> BuscarGruposOferta(GrupoOfertaFiltroData filtroData);

        /// <summary>
        /// Retorna um grupo de oferta
        /// </summary>
        GrupoOfertaData BuscarGrupoOferta(long seqGrupoOferta);

        /// <summary>
        /// Salva um grupo de oferta.
        /// </summary>
        long SalvarGrupoOferta(GrupoOfertaData grupoOfertaData);

        /// <summary>
        /// Exclui um grupo de oferta
        /// </summary>
        void ExcluirGrupoOferta(long seqGrupoOferta);

        /// <summary>
        /// Retorna o Guid do Processo atravez do grupo de oferta
        /// </summary>
        /// <param name="seqGrupoOferta"></param>
        /// <returns></returns>
        Guid BuscarSeqProcessoPorSeqGrupoOferta(long seqGrupoOferta);
    }
}
