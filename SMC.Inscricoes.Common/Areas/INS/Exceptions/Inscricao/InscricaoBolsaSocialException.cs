using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoBolsaSocialException : SMCApplicationException
    {
        public InscricaoBolsaSocialException(string grupoOfertas, int semestre, int ano)
            : base(string.Format(ExceptionsResource.InscricaoBolsaSocialException, grupoOfertas, semestre, ano))
        { }
    }
}
