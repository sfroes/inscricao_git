using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoPaginaEtapaLiberadaException : SMCApplicationException
    {
        public ExclusaoPaginaEtapaLiberadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoPaginaEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
