using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracoDocumentoRequeridoSexoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracoDocumentoRequeridoSexoException
        /// </summary>
        public AlteracoDocumentoRequeridoSexoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracoDocumentoRequeridoSexoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
