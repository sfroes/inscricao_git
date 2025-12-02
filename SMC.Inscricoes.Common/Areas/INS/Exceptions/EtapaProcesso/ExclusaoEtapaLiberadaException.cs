using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoEtapaLiberadaException : SMCApplicationException
    {
        public ExclusaoEtapaLiberadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
