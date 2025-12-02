using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoDocumentoRequeridoEtapaLiberadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ExclusaoDocumentoRequeridoEtapaLiberadaException
        /// </summary>
        public ExclusaoDocumentoRequeridoEtapaLiberadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoDocumentoRequeridoEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
