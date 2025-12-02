using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class GrupoDocumentoAlteradoItemAlteracoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de GrupoDocumentoAlteracaoMinimoException
        /// </summary>
        public GrupoDocumentoAlteradoItemAlteracoException(string TipoDocumento)
            : base(String.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "GrupoDocumentoAlteradoItemAlteracoException", System.Threading.Thread.CurrentThread.CurrentCulture),TipoDocumento))
        {

        }
    }
}
