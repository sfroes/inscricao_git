using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SituacaoInscricaoAlteradaAnteriormenteException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaExistenteException
        /// </summary>
        public SituacaoInscricaoAlteradaAnteriormenteException(string nomeInscrito,string situacaoOrigem,string situacaoDestino)
            : base(String.Format(            
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "SituacaoInscricaoAlteradaAnteriormenteException", System.Threading.Thread.CurrentThread.CurrentCulture)
            ,nomeInscrito,situacaoOrigem,situacaoDestino))
        {

        }
    }
}
