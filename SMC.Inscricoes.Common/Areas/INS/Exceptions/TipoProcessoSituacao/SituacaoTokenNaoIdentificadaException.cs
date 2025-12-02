using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SituacaoTokenNaoIdentificadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de SituacaoTokenNaoIdentificadaException
        /// </summary>
        public SituacaoTokenNaoIdentificadaException(string token)
            : base(string.Format(
             SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "SituacaoTokenNaoIdentificadaException", System.Threading.Thread.CurrentThread.CurrentCulture), token))
        {

        }
    }
}
