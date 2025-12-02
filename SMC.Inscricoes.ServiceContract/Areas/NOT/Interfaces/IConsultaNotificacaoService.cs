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
    public interface IConsultaNotificacaoService : ISMCService
    {
        SMCPagerData<ConsultaNotificacaoListaData> BuscarNotificacoes(ConsultaNotificacaoFiltroData consultaNotificacaoFiltroData);

        ConsultaNotificacaoData BuscarNotificacao(long seqNotificacao);
    }
}
