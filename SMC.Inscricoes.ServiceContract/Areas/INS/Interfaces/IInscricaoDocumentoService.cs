using SMC.Framework.Model;
using SMC.Framework.Service;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    public interface IInscricaoDocumentoService : ISMCService
    {
        List<SMCDatasourceItem<string>> BuscarSituacoesEntregaDocumento(long seqInscricao, long seqDocumentoRequerido);
    }
}