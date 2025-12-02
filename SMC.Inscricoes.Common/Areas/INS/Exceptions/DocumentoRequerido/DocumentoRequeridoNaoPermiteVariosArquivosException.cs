using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DocumentoRequeridoNaoPermiteVariosArquivosException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de DocumentoRequeridoNaoPermiteVariosArquivosException
        /// </summary>
        public DocumentoRequeridoNaoPermiteVariosArquivosException(string descricaoTipo)
            : base(string.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DocumentoRequeridoNaoPermiteVariosArquivosException", System.Threading.Thread.CurrentThread.CurrentCulture), descricaoTipo))
        {

        }
    }
}
