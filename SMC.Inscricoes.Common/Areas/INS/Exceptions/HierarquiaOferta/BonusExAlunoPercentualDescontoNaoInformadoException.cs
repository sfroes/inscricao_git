using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class BonusExAlunoPercentualDescontoNaoInformadoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de OfertaVencimentoInvalidoException
        /// </summary>
        public BonusExAlunoPercentualDescontoNaoInformadoException()
            : base(ExceptionsResource.BonusExAlunoPercentualDescontoNaoInformadoException)
        {
        }
    }
}
