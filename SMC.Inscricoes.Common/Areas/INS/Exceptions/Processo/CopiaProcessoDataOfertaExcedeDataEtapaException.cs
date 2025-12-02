using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class CopiaProcessoDataOfertaExcedeDataEtapaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de CopiaProcessoDataOfertaExcedeDataEtapaException
        /// </summary>
        public CopiaProcessoDataOfertaExcedeDataEtapaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "CopiaProcessoDataOfertaExcedeDataEtapaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {
        }
    }
}
