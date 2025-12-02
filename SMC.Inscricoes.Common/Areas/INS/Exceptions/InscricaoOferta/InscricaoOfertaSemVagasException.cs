using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoOfertaSemVagasException : SMCApplicationException
    {
        public InscricaoOfertaSemVagasException(string descricaoOferta, string pagina, string labelOferta)
            : base(string.Format(ExceptionsResource.InscricaoOfertaSemVagasException, descricaoOferta, pagina, labelOferta))
        { }
    }
}
