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
    public class InscricaoNoPrazoSpecification : SMCSpecification<Inscricao>
    {
        public long SeqProcesso { get; set; }

        public DateTime? DataFim { get; set; }

        public DateTime? DataInicio { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return o => o.SeqProcesso == SeqProcesso &&
                        (!DataFim.HasValue || o.DataInscricao > DataFim.Value || o.DataInclusao > DataFim.Value) &&
                        (!DataInicio.HasValue || o.DataInscricao < DataInicio.Value || o.DataInclusao < DataInicio.Value);
        }
    }
}
