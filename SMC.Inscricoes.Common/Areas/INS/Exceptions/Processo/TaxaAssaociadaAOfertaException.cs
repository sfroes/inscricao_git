using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Processo
{
    public class TaxaAssaociadaAOfertaException : SMCApplicationException
    {
        public TaxaAssaociadaAOfertaException(string descricaoTipoTaxa)
            : base(string.Format(ExceptionsResource.TaxaAssaociadaAOfertaException, descricaoTipoTaxa))
        {

        }
    }
}
