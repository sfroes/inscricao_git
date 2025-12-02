using SMC.Framework.Model;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    public interface IFonteExternaService : IDisposable
    {
        List<SMCDatasourceItem> ListarProjetoVinculadoInscricao(string seqProcessoInscricao, string seqUsuarioSAS);
    }
}
