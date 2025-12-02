using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class ConvocadoExportadoException : SMCApplicationException
    {
        public ConvocadoExportadoException() 
            : base(Resources.ExceptionsResource.ConvocadoExportadoException)
        { }
    }
}
