using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class GrupoDocumentoMinimoItensInvalidoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de GrupoDocumentoMinimoItensInvalidoException
        /// </summary>
        public GrupoDocumentoMinimoItensInvalidoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "GrupoDocumentoMinimoItensInvalidoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
