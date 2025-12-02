using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class LiberacaoEtapaCodigoSemPaginaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de LiberacaoEtapaCodigoSemPaginaException
        /// </summary>
        public LiberacaoEtapaCodigoSemPaginaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "LiberacaoEtapaCodigoSemPaginaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
