using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.RES.Exceptions
{
    public class UnidadeResponsavelTipoHierarquiaExcludeException : SMCApplicationException
    {
        public UnidadeResponsavelTipoHierarquiaExcludeException(string tipoHierarquia, string unidadeResponsavel, string tipoProcesso)
            : base(string.Format(
            SMC.Inscricoes.Common.Areas.RES.Resources.ExceptionsResource.ResourceManager.GetString(
            "UnidadeResponsavelTipoHierarquiaExcludeException", System.Threading.Thread.CurrentThread.CurrentCulture), tipoHierarquia, unidadeResponsavel, tipoProcesso))
        { }
    }
}
