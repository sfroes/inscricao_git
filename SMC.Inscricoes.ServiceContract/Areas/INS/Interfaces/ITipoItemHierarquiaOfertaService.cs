using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface ITipoItemHierarquiaOfertaService : ISMCService
    {
        /// <summary>
        /// Busca os Tipos de Item de uma Hierarquia Oferta
        /// Serviço utilizado pelo Novo SGA nas configurações de Campanha (Exposto WCF)
        /// </summary>
        /// <returns>Lista de Tipo Item Hierarquia Oferta para apresentar em um SELECT</returns>
        [OperationContract]
        List<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaSelect();

        List<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaPorProcessoSelect(long seqProcesso, bool? habilitaOferta = null);
    }
}
