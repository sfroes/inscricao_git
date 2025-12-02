using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoBoletoTitulo
{
    public class EnderecoObrigatorioParaBoletoException : SMCApplicationException
    {
        public EnderecoObrigatorioParaBoletoException()
          : base(Resources.ExceptionsResource.ERR_EnderecoObrigatorioParaBoletoException)
        {
        }
    }
}
