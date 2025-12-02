using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoOferta
{
    public class InscricaoOfertaAlterarOfertaLimiteDescontoDiferenteException : SMCApplicationException
    {
        public InscricaoOfertaAlterarOfertaLimiteDescontoDiferenteException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
                "InscricaoOfertaAlterarOfertaLimiteDescontoDiferenteException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}