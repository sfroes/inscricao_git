using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class PermissaoInscricaoForaPrazoCoincidenteException : SMCApplicationException
    {
        public PermissaoInscricaoForaPrazoCoincidenteException() 
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "PermissaoInscricaoForaPrazoCoincidenteException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
