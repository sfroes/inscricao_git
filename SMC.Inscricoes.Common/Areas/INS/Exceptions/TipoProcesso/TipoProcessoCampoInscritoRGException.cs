using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.TipoProcesso
{
    public class TipoProcessoCampoInscritoRGException : SMCApplicationException
    {
        public TipoProcessoCampoInscritoRGException()
            : base(ExceptionsResource.ERR_TipoProcessoCampoInscritoRGException)
        {
        }
    }
}
