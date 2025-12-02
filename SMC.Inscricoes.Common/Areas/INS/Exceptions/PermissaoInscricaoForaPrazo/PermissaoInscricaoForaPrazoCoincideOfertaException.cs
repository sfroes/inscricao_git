using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class PermissaoInscricaoForaPrazoCoincideOfertaException : SMCApplicationException
    {
        public PermissaoInscricaoForaPrazoCoincideOfertaException() 
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "PermissaoInscricaoForaPrazoCoincideOfertaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
