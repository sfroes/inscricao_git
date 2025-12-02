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
    public interface IProcessoCampoInscritoService : ISMCService
    {
        /// Buscar campos inscritos por processo.
        /// </summary>
        /// <param name="seqProcesso">O sequencial do processo.</param>
        /// <returns>Uma lista de campos inscritos.</returns>
        List<ProcessoCampoInscritoData> BuscarCamposIncritosPorProcesso(long seqProcesso);

        /// <summary>
        /// Buscar campos inscritos por UIID do processo.
        /// </summary>
        /// <param name="guid">O UIID do processo.</param>
        /// <returns>Uma lista de campos inscritos.</returns>
        List<ProcessoCampoInscritoData> BuscarCamposInscritosPorUIIDProcesso(Guid guid);
    }
}