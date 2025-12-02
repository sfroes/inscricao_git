using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Resources;

namespace SMC.Inscricoes.Common.Exceptions
{
    public class ArquivoAnexadoExpurgoDadosException : SMCApplicationException
    {
        public ArquivoAnexadoExpurgoDadosException(string nome)
            : base(string.Format(ExceptionsResource.ArquivoAnexadoExpurgoDadosException, nome))
        { }
    }
}