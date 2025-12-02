using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoComOfertaInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoComOfertaInvalidaException
        /// </summary>
        public InscricaoComOfertaInvalidaException(string artigo, string tipoProcesso, string descricaoOferta)
            : base(string.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricaoComOfertaInvalidaException", System.Threading.Thread.CurrentThread.CurrentCulture), artigo, tipoProcesso, descricaoOferta))
        {

        }
    }
}
