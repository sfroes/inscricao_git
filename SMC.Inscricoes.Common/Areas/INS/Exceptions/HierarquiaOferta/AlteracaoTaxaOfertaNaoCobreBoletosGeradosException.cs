using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoTaxaOfertaNaoCobreBoletosGeradosException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoTaxaOfertaNaoCobreBoletosGeradosException
        /// </summary>
        public AlteracaoTaxaOfertaNaoCobreBoletosGeradosException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoTaxaOfertaNaoCobreBoletosGeradosException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
