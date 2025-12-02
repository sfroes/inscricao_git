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
    public interface IGrupoTaxaService : ISMCService
    {       

        /// <summary>
        /// Retorna o par de chave/valor para os grupos de Taxa do processo informado
        /// </summary>        
        IEnumerable<SMCDatasourceItem> BuscaGrupoTaxa(long SeqProcesso);

        ///// <summary>
        ///// Retorna os grupos de Taxa para exibição em lista.
        ///// </summary>        
        List<GrupoTaxaListaData> BuscarGruposTaxa(GrupoTaxaFiltroData filtros);

        IEnumerable<SMCDatasourceItem> BuscarGrupoTaxaItemPorGrupoTaxaSelect(long? seqGrupoTaxa);


        /// <summary>
        /// Salva um grupo de Taxa.
        /// </summary>
        long SalvarGrupoTaxa(GrupoTaxaData grupoTaxaData);

        /// <summary>
        /// Exclui um grupo de Taxa
        /// </summary>
        void ExcluirGrupoTaxa(long seqGrupoTaxa);

        /// <summary>
        /// Retorna todos os grupos de taxas vinculados a um processo
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        List<GrupoTaxaData> BuscarGruposTaxaPorSeqProcesso(long seqProcesso);
    }
}
