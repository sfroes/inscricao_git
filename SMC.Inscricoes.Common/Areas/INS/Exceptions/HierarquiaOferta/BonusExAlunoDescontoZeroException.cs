using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class BonusExAlunoDescontoZeroException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de BonusExAlunoDescontoZeroException
        /// </summary>
        public BonusExAlunoDescontoZeroException()
            : base(ExceptionsResource.BonusExAlunoDescontoZeroException)
        {
        }
    }
}
