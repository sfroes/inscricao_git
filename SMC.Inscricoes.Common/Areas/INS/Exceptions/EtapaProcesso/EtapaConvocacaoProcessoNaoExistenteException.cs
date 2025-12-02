using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class EtapaConvocacaoProcessoNaoExistenteException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoNaoExistenteException
        /// </summary>
        public EtapaConvocacaoProcessoNaoExistenteException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            "EtapaConvocacaoProcessoNaoExistenteException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
