using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoTemplateProcessoDesativadoException : SMCApplicationException
    {
        public AlteracaoTemplateProcessoDesativadoException(string descricaoTemplate)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoTemplateProcessoDesativadoException", System.Threading.Thread.CurrentThread.CurrentCulture),
            descricaoTemplate))
        { }
    }
}
