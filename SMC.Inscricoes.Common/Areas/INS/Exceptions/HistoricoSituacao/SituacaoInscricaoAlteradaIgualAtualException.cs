using SMC.Framework.Exceptions;
using System;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SituacaoInscricaoAlteradaIgualAtualException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de SituacaoInscricaoAlteradaIgualAtualException
        /// </summary>
        public SituacaoInscricaoAlteradaIgualAtualException(string situacaoAlterada, string nomeInscrito)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "SituacaoInscricaoAlteradaIgualAtualException", System.Threading.Thread.CurrentThread.CurrentCulture)
            , situacaoAlterada, nomeInscrito))
        {
        }
    }
}