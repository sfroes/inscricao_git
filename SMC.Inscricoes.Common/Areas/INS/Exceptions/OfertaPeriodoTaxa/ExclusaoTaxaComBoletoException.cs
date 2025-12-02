using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoTaxaComBoletoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ExclusaoTaxaComBoletoException
        /// </summary>
        public ExclusaoTaxaComBoletoException(string taxa)
            : base(String.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoTaxaComBoletoException", System.Threading.Thread.CurrentThread.CurrentCulture),taxa))
        {

        }
    }
}
