using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class NaoEhProfessorDaCasaException : SMCApplicationException
    {
        public NaoEhProfessorDaCasaException(string telefones)
            : base(string.Format(ExceptionsResource.ResourceManager.GetString("ERR_NaoEhProfessorDaCasaException", System.Threading.Thread.CurrentThread.CurrentCulture), telefones))
        {
        }
    }
}