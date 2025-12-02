using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ItemHierarquiaOfertaFilterSpecification : SMCSpecification<ItemHierarquiaOferta>
    {        

        public long SeqTipoHierarquiaOferta { get; set; }

        public long? SeqProcesso { get; set; }

        public override Expression<Func<ItemHierarquiaOferta, bool>> SatisfiedBy()
        {
            AddExpression(t => t.SeqTipoHierarquiaOferta == this.SeqTipoHierarquiaOferta);
            AddExpression(SeqProcesso, t => t.TipoHierarquiaOferta.Processo.Any(p=>p.Seq == this.SeqProcesso));

            return GetExpression();
        }
    }
}
