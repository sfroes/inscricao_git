using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class TipoDocumentoDesativadoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de TipoDocumentoDesativadoException
        /// </summary>
        public TipoDocumentoDesativadoException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "TipoDocumentoDesativadoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
