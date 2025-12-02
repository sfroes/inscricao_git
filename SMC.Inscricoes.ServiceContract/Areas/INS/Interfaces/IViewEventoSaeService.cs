using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IViewEventoSaeService : ISMCService
    {
        List<SMCDatasourceItem> BuscarEventosSaeSelect(long seqUnidadeResponsavel, int? ano);

    }
}
