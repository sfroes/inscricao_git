using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    public interface ICodigoAutorizacaoService : ISMCService
    {
        List<SMCDatasourceItem> BuscarCodigosAutorizacaoKeyValue(long seqUnidadeResponsavel);

        List<SMCDatasourceItem> BuscarCodigosAutorizacaoPorProcessoSelect(long seqProcesso);

        long SalvarCodigoAutorizacao(CodigoAutorizacaoData codigoAutorizacao);
    }
}
