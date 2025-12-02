using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SitucaoExigeJustificativaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaExistenteException
        /// </summary>
        public SitucaoExigeJustificativaException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            "SitucaoExigeJustificativaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
