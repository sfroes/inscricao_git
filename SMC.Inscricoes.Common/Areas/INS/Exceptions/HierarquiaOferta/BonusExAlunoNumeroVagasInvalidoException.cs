using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class BonusExAlunoNumeroVagasInvalidoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de BonusExAlunoNumeroVagasInvalidoException
        /// </summary>
        public BonusExAlunoNumeroVagasInvalidoException()
            : base(ExceptionsResource.BonusExAlunoNumeroVagasInvalidoException)
        {
        }
    }
}
