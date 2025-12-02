using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface ITipoHierarquiaOfertaService : ISMCService
    {
        List<SMCDatasourceItem> BuscarTiposHierarquiaOfertaKeyValue();

        SMCPagerData<TipoHierarquiaOfertaListaData> BuscarTiposHierarquiaOferta(TipoHierarquiaOfertaFiltroData filtro);

        TipoHierarquiaOfertaData BuscarTipoHierarquiaOferta(long seqTipoHierarquiaOferta);

        long SalvarTipoHierarquiaOferta(TipoHierarquiaOfertaData tipoHierarquiaOferta);

        void ExcluirTipoHieraquiaOferta(long seqTipoHierarquiaOferta);

        List<ItemHierarquiaOfertaData> BuscarItemsHieraquiaOferta(long seqTipoHierarquiaOferta);

        long SalvarItemHierarquiaOferta(ItemHierarquiaOfertaData item);

        void ExcluirItemHierarquiaOferta(long seqItemHierarquiaOferta);

        ItemHierarquiaOfertaData BuscarItemHieraquiaOferta(long seqItemHierarquiaOferta);
        
    }
}
