using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class GrupoDocumentoUploadObrigatorioInvalidoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de GrupoDocumentoUploadObrigatorioInvalidoException
        /// </summary>
        public GrupoDocumentoUploadObrigatorioInvalidoException(string operacao)
            : base(String.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "GrupoDocumentoUploadObrigatorioInvalidoException", System.Threading.Thread.CurrentThread.CurrentCulture), operacao))
        {

        }
    }
}
