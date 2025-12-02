using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class GrupoDocumentoAumentoMinimoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de GrupoDocumentoAumentoMinimoException
        /// </summary>
        public GrupoDocumentoAumentoMinimoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "GrupoDocumentoAumentoMinimoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
