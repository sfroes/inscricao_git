using Microsoft.SqlServer.Server;
using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoOferta
{
    public class CheckinRealizadoException : SMCApplicationException
    {
        public CheckinRealizadoException(string descricaoOferta)
            : base(string.Format(ExceptionsResource.ERR_CheckinRealizadoException, descricaoOferta))
        { }
    }
}