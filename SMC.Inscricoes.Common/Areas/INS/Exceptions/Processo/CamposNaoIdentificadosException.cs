using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Processo
{
    public class CamposNaoIdentificadosException : SMCApplicationException
    {
        public CamposNaoIdentificadosException(string menssage)
            : base(string.Format(ExceptionsResource.ERR_CamposNaoIdentificadosException, menssage))
        {
        }
    }
}
