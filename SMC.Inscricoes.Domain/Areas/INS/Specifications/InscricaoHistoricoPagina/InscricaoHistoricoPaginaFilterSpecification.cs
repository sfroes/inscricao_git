using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoHistoricoPaginaFilterSpecification : SMCSpecification<InscricaoHistoricoPagina>
    {
        public long SeqInscricao { get; set; }

        public override Expression<Func<InscricaoHistoricoPagina, bool>> SatisfiedBy()
        {
            return i => i.SeqInscricao == this.SeqInscricao;
        }
    }
}
