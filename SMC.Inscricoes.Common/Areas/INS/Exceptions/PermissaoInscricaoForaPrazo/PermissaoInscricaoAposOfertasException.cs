using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class PermissaoInscricaoAposOfertasException : SMCApplicationException
    {
        public PermissaoInscricaoAposOfertasException() 
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "PermissaoInscricaoAposOfertasException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
