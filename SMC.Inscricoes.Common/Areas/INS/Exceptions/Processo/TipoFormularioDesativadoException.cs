using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class TipoFormularioDesativadoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de TipoFormularioDesativadoException
        /// </summary>
        public TipoFormularioDesativadoException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "TipoFormularioDesativadoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
