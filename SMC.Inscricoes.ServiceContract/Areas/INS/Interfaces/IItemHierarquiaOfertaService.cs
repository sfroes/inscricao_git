using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Interface com os serviços de integração do GPI.
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IItemHierarquiaOfertaService : ISMCService
    {
        /// <summary>
        /// Busca a hierarquia dos itens de oferta de um processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo seletivo</param>
        /// <returns>Lista com os itens da hierarquia de oferta</returns>
        [OperationContract]
        List<ItemHierarquiaOfertaArvoreData> BuscarItensHierarquiaOfertaArvore(long seqProcesso);
    }
}
