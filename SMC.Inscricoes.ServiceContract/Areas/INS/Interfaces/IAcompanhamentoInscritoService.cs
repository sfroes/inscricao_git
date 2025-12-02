using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    public interface IAcompanhamentoInscritoService : ISMCService
    {
        SMCPagerData<AcompanhamentoInscritoListaData> BuscarInscrito(AcompanhamentoInscritoFiltroData filtro);
    }
}
