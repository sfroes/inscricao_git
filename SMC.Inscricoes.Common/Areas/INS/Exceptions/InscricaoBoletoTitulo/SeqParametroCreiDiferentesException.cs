using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SeqParametroCreiDiferentesException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoComOfertaInvalidaException
        /// </summary>
        public SeqParametroCreiDiferentesException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "SeqParametroCreiDiferentesException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
