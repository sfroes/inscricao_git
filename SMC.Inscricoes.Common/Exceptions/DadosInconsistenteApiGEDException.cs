using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Inscricoes.Common.Resources;

namespace SMC.Inscricoes.Common.Exceptions
{
    public class DadosInconsistenteApiGEDException : SMCApplicationException
    {
        public DadosInconsistenteApiGEDException() 
            : base(ExceptionsResource.DadosInconsistenteApiGEDException)
        { }
    }
}
