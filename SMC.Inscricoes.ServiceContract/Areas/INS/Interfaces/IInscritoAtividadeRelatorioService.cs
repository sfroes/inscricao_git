using SMC.DadosMestres.Common.Constants;
using SMC.Framework.Service;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de InscritoAtividade para o Relatorio
    /// </summary>

    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IInscritoAtividadeRelatorioService : ISMCService
    {
        /// <summary>
        /// Busca lista de Inscritos na Atividade pelo filtro informado
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa</param>
        /// <returns>Lista de trabalhos</returns>   
        [OperationContract]
        List<InscritoAtividadeRelatorioListaData> BuscarInscritosAtividades(InscritoAtividadeRelatorioFiltroData filtro);
    }
}
