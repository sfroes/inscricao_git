using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlterarOfertaTaxaParametroCreiException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlterarOfertaTaxaParametroCreiException
        /// </summary>
        public AlterarOfertaTaxaParametroCreiException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlterarOfertaTaxaParametroCreiException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
