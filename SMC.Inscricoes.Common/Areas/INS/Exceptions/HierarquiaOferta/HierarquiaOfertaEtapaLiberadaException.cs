using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class HierarquiaOfertaEtapaLiberadaException : SMCApplicationException
    {
        public HierarquiaOfertaEtapaLiberadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "HierarquiaOfertaEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
