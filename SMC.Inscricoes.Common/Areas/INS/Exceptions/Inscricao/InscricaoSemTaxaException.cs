using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoSemTaxaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoSemTaxaException
        /// </summary>
        public InscricaoSemTaxaException()
            : base(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricaoSemTaxaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
