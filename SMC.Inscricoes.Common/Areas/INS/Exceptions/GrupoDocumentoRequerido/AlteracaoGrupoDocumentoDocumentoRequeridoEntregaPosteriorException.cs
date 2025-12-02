using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoGrupoDocumentoDocumentoRequeridoEntregaPosteriorException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoGrupoDocumentoDocumentoRequeridoEntregaPosteriorException
        /// </summary>
        public AlteracaoGrupoDocumentoDocumentoRequeridoEntregaPosteriorException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoGrupoDocumentoDocumentoRequeridoEntregaPosteriorException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
