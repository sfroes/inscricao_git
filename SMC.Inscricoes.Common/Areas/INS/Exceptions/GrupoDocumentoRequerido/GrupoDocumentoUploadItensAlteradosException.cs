using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class GrupoDocumentoUploadItensAlteradosException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de GrupoDocumentoAlteracaoMinimoException
        /// </summary>
        public GrupoDocumentoUploadItensAlteradosException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "GrupoDocumentoUploadItensAlteradosException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
