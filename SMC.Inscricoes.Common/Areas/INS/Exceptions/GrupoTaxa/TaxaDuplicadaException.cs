using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class TaxaDuplicadaException : SMCApplicationException
    {
        public TaxaDuplicadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "TaxaDuplicadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
