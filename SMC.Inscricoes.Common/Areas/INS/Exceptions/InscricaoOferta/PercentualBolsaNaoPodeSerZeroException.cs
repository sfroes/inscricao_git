using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoOferta
{
    public class PercentualBolsaNaoPodeSerZeroException : SMCApplicationException
    {
        public PercentualBolsaNaoPodeSerZeroException()
           : base(ExceptionsResource.PercentualBolsaNaoPodeSerZeroException)
        {
        }
    }
}