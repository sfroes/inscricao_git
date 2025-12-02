using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.RES.Exceptions
{
    public class UnidadeResponsavelTipoProcessoAlterException : SMCApplicationException
    {
        public UnidadeResponsavelTipoProcessoAlterException(string tipoProcesso, string unidadeResponsavel)
            : base(string.Format(
            SMC.Inscricoes.Common.Areas.RES.Resources.ExceptionsResource.ResourceManager.GetString(
            "UnidadeResponsavelTipoProcessoAlterException", System.Threading.Thread.CurrentThread.CurrentCulture), tipoProcesso, unidadeResponsavel))
        { }
    }
}
