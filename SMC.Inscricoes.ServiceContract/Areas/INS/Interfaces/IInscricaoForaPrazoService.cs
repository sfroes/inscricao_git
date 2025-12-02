using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Framework.Model;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IInscricaoForaPrazoService : ISMCService
    {
        SMCPagerData<InscricaoForaPrazoListaData> BuscarInscricoesForaPrazo(InscricaoForaPrazoFiltroData filtroData);

        PermissaoInscricaoForaPrazoData BuscarInscricaoForaPrazo(long seq);

        void SalvarPermissoes(PermissaoInscricaoForaPrazoData data);

        void ExcluirPermissao(long seq);        
    }
}
