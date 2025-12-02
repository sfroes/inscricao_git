using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoDocumentoRequeridoComInscricaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ExclusaoDocumentoRequeridoComInscricaoException
        /// </summary>
        public ExclusaoDocumentoRequeridoComInscricaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoDocumentoRequeridoComInscricaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
