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
    public interface ITipoProcessoCampoInscritoService : ISMCService
    {
        /// <summary>
        /// Busca os tipos de processo campos inscrito por tipo de processo.
        /// </summary>
        /// <param name="seqTipoProcesso">O sequencial do tipo de processo.</param>
        /// <returns>Uma lista de tipos de processo campos inscrito.</returns>
        List<TipoProcessoCampoInscritoData> BuscarTiposProcessoCamposInscritoPorTipoProcesso(long seqTipoProcesso);
    }
}
