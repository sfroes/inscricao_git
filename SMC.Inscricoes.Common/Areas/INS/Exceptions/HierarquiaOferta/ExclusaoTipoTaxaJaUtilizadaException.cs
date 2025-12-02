using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoTipoTaxaJaUtilizadaException : SMCApplicationException
    {
        public ExclusaoTipoTaxaJaUtilizadaException(string taxa)
            : base(string.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoTipoTaxaJaUtilizadaException", System.Threading.Thread.CurrentThread.CurrentCulture), taxa))
        { }
    }
}
