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
    public class InscricaoCodigoAutorizacaoFilterSpecification : SMCSpecification<InscricaoCodigoAutorizacao>
    {
        public long? SeqInscricao { get; set; }

        public long? SeqCodigoAutorizacao { get; set; }

        public override Expression<Func<InscricaoCodigoAutorizacao, bool>> SatisfiedBy()
        {
            return i => (!this.SeqInscricao.HasValue || i.SeqInscricao == this.SeqInscricao.Value)
                     && (!this.SeqCodigoAutorizacao.HasValue || i.SeqCodigoAutorizacao == this.SeqCodigoAutorizacao.Value);
        }
    }
}
