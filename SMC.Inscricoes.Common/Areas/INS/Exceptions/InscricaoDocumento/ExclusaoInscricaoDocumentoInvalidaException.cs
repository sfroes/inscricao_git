using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoInscricaoDocumentoInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaExistenteException
        /// </summary>
        public ExclusaoInscricaoDocumentoInvalidaException(string descricaoTipoDocumento)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoInscricaoDocumentoInvalidaException", System.Threading.Thread.CurrentThread.CurrentCulture), descricaoTipoDocumento))
        {

        }
    }
}
