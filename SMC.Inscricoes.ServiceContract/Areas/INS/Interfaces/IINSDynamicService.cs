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
    public interface IINSDynamicService : ISMCService
    {
    }
}
