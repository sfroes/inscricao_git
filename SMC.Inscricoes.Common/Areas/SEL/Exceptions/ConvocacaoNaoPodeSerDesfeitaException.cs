using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class ConvocacaoNaoPodeSerDesfeitaException : SMCApplicationException
    {
        public ConvocacaoNaoPodeSerDesfeitaException()
            : base(Resources.ExceptionsResource.ConvocacaoNaoPodeSerDesfeitaException)
        {
        }
    }
}