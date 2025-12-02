using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class QuantidadeTaxaExcedidaNasOfertasDoProcessoException : SMCApplicationException
    {
        public QuantidadeTaxaExcedidaNasOfertasDoProcessoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "QuantidadeTaxaExcedidaNasOfertasDoProcessoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
