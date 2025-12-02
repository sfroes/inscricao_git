using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoPermissaoInscricaoForaPrazoComInscricaoException : SMCApplicationException
    {
        public AlteracaoPermissaoInscricaoForaPrazoComInscricaoException() 
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoPermissaoInscricaoForaPrazoComInscricaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
