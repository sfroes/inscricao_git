using SMC.Framework.Model;
using SMC.Framework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    public interface ITipoTaxaService : ISMCService
    {
        List<SMCDatasourceItem> BuscarTiposTaxaSelect();        
    }
}
