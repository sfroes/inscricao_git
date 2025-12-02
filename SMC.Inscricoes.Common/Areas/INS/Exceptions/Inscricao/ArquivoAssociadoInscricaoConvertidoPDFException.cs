using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao
{
    public class ArquivoAssociadoInscricaoConvertidoPDFException : SMCApplicationException
    {
        public ArquivoAssociadoInscricaoConvertidoPDFException()
          : base(ExceptionsResource.ERR_ArquivoAssociadoInscricaoConvertidoPDFException)
        { }
    }
}
