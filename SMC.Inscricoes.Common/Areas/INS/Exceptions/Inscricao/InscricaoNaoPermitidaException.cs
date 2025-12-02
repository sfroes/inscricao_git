using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    /// <summary>
    /// Exceção disparada quando alguma regra impede uma inscrição de proseguir.
    /// </summary>
    public class InscricaoNaoPermitidaException : SMCApplicationException
    {
        public InscricaoNaoPermitidaException(string tipoProcesso, string artigo)
            : base (string.Format(ExceptionsResource.InscricaoNaoPermitidaException, tipoProcesso, artigo))
        { }
    }
}
