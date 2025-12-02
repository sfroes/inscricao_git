using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class DesfazerLancamentoEtapaInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de DesfazerLancamentoEtapaInvalidaException
        /// </summary>
        public DesfazerLancamentoEtapaInvalidaException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString("DesfazerLancamentoEtapaInvalidaException",
                System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
