using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AssociacaoDuplicadaTaxaException : SMCApplicationException
    {
        public AssociacaoDuplicadaTaxaException(string taxa, string grpTaxa)
            : base(string.Format(
                SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
                "AssociacaoDuplicadaTaxaException", System.Threading.Thread.CurrentThread.CurrentCulture), 
                taxa, grpTaxa))
        { }
    }
}
