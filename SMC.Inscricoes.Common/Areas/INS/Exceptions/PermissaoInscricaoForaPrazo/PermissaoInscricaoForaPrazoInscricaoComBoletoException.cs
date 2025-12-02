using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class PermissaoInscricaoForaPrazoInscricaoComBoletoException : SMCApplicationException
    {
        public PermissaoInscricaoForaPrazoInscricaoComBoletoException() 
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "PermissaoInscricaoForaPrazoInscricaoComBoletoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
