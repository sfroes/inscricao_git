using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DocumentoRequeridoPermiteEntregaPosteriorAlteradoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de DocumentoRequeridoPermiteEntregaPosteriorAlteradoException
        /// </summary>
        public DocumentoRequeridoPermiteEntregaPosteriorAlteradoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DocumentoRequeridoPermiteEntregaPosteriorAlteradoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
