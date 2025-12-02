using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class PermissaoInscricaoForaPrazoVencimentoMenorOfertaException : SMCApplicationException
    {
        public PermissaoInscricaoForaPrazoVencimentoMenorOfertaException() 
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "PermissaoInscricaoForaPrazoVencimentoMenorOfertaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
