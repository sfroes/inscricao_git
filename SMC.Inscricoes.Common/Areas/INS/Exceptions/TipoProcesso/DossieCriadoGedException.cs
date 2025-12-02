using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.TipoProcesso
{
    public class DossieCriadoGedException : SMCApplicationException
    {
        public DossieCriadoGedException()
            : base(ExceptionsResource.ERR_DossieCriadoGedException)
        {
        }
    }
}