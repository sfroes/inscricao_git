using SMC.Framework.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoDocumentoCadastradoSpecification : SMCSpecification<InscricaoDocumento>
    {
        public long SeqDocumentoRequerido { get; set; }

        public override Expression<Func<InscricaoDocumento, bool>> SatisfiedBy()
        {
            return x => x.SeqDocumentoRequerido == SeqDocumentoRequerido;
        }
    }
}
