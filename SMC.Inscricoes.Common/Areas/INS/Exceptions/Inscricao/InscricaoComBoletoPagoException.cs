using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoComBoletoPagoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoComBoletoPagoException
        /// </summary>
        public InscricaoComBoletoPagoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricaoComBoletoPagoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
