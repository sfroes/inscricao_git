using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class BonusExAlunoNumeroVagasNaoInformadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de OfertaVencimentoInvalidoException
        /// </summary>
        public BonusExAlunoNumeroVagasNaoInformadaException()
            : base(ExceptionsResource.BonusExAlunoNumeroVagasNaoInformadoException)
        {
        }
    }
}
