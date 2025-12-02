using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class PermissaoInscricaoDataTaxaOfertaCoincidenteException : SMCApplicationException
    {
        public PermissaoInscricaoDataTaxaOfertaCoincidenteException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "PermissaoInscricaoDataTaxaOfertaCoincidenteException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}