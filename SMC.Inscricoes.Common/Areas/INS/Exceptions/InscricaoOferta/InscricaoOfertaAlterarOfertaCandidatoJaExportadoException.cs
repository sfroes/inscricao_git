using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoOferta
{
    public class InscricaoOfertaAlterarOfertaCandidatoJaExportadoException : SMCApplicationException
    {
        public InscricaoOfertaAlterarOfertaCandidatoJaExportadoException(string nomeCandidato, string descricaoOferta)
            : base(string.Format(Resources.ExceptionsResource.ResourceManager.GetString(
                "InscricaoOfertaAlterarOfertaCandidatoJaExportadoException", System.Threading.Thread.CurrentThread.CurrentCulture), nomeCandidato, descricaoOferta))
        { }
    }
}