using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class TemplateProcessoAssociadoProcessoException : SMCApplicationException
    {
        public TemplateProcessoAssociadoProcessoException(string template, string processo)
            : base(String.Format(
                SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
                "TemplateProcessoAssociadoProcessoException", System.Threading.Thread.CurrentThread.CurrentCulture),
                template, processo))
        { }
    }
}
