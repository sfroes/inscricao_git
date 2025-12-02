using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoDocumentoRequeridoObrigatorioException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoDocumentoRequeridoObrigatorioException
        /// </summary>
        public AlteracaoDocumentoRequeridoObrigatorioException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoDocumentoRequeridoObrigatorioException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
