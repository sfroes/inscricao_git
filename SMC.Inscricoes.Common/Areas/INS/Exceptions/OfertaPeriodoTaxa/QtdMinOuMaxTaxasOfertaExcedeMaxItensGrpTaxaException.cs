using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class QtdMinOuMaxTaxasOfertaExcedeMaxItensGrpTaxaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de CadastroOfertaPeriodoTaxaNaoPermitidoException
        /// </summary>       
        public QtdMinOuMaxTaxasOfertaExcedeMaxItensGrpTaxaException(string descTipoTaxa, string descGrupoTaxa)
        : base(string.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "QtdMinOuMaxTaxasOfertaExcedeMaxItensGrpTaxaException", System.Threading.Thread.CurrentThread.CurrentCulture),
            descTipoTaxa, descGrupoTaxa))
        { }
    }
}