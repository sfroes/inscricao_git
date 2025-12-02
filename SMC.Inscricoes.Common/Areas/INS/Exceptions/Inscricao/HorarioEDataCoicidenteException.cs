using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao
{
    public class HorarioEDataCoicidenteException : SMCApplicationException
    {
        public HorarioEDataCoicidenteException(string processoIdioma)
            : base(string.Format(ExceptionsResource.HorarioEDataCoicidenteException, processoIdioma))
        {
        }
    }
}