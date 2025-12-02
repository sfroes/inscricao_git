using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoDocumentoRequeridoUploadEmGrupoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoDocumentoRequeridoUploadEmGrupoException
        /// </summary>
        public AlteracaoDocumentoRequeridoUploadEmGrupoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoDocumentoRequeridoUploadEmGrupoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
