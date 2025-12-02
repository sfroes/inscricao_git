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
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IHierarquiaOfertaService : ISMCService
    {
        List<SMCDatasourceItem> BuscarHierarquiaOfertasPorTipoKeyValue(long seqProcesso, long seqTipoItem);

        SMCPagerData<LookupHierarquiaOfertaData> LookupBuscarHierarquiaOfertas(LookupHierarquiaOfertaFiltroData filtro);

        LookupHierarquiaOfertaData LookupBuscarHierarquiaOferta(long seq);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void ExcluirHierarquiaOferta(long seqHierarquiaOferta);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Dictionary<long, long?> AdicionarItemHierarquiaOferta(long seqProcesso, List<ItemOfertaHierarquiaOfertaData> hierarquiasOfertas);
    }
}
