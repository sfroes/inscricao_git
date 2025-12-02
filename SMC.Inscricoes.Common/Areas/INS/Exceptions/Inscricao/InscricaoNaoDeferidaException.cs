using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoNaoDeferidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoNaoDeferidaException
        /// </summary>
        public InscricaoNaoDeferidaException(string situacaoInscricao, string tipoProcesso, string nomeInscrito)
             : base(string.Format(ExceptionsResource.InscricaoNaoDeferidaException, situacaoInscricao, tipoProcesso, nomeInscrito))
        { }
    }
}
