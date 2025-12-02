using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Processo
{
    public class ExisteInscricaoComTaxaDentreOsBoletos : SMCApplicationException
    {
        public ExisteInscricaoComTaxaDentreOsBoletos(string descricaoTipoTaxa)
            : base(string.Format(ExceptionsResource.Err_ExisteInscricaoComTaxaDentreOsBoletos,descricaoTipoTaxa))
        {

        }
    }
}
