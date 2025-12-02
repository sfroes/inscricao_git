using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class LiberacaoEtapaTaxaPorOfertaException : SMCApplicationException
    {
        public LiberacaoEtapaTaxaPorOfertaException(string descricaoTaxa, string nomeGrupoOferta) 
            : base(string.Format(ExceptionsResource.LiberacaoEtapaTaxaPorOfertaException, descricaoTaxa, nomeGrupoOferta))
        { }
    }
}
