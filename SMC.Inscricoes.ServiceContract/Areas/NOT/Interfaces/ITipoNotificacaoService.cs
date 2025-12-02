using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces
{
    public interface ITipoNotificacaoService : ISMCService
    {
        SMCPagerData<TipoNotificacaoListaData> BuscarTipoNotificacoes(TipoNotificacaoFiltroData filtro);

        TipoNotificacaoData BuscarTipoNotificacao(long seqNotificacao);

        long SalvarTipoNotificacao(TipoNotificacaoData tipoNotificacaoData);

        void Excluir(long seqTipoNotificacao);
    }
}
