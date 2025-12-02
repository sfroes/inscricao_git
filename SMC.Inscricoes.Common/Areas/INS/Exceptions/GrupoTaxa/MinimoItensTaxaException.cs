using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class MinimoItensTaxaException : SMCApplicationException
    {
        public MinimoItensTaxaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "MinimoItensTaxaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
