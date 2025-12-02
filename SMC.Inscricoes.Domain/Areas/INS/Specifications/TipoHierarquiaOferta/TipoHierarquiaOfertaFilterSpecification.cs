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
    public class TipoHierarquiaOfertaFilterSpecification : SMCSpecification<TipoHierarquiaOferta>
    {

        public TipoHierarquiaOfertaFilterSpecification()
        {
            this.SetOrderBy(x => x.Descricao);
        }

        public long? Seq { get; set; }

        public string Descricao { get; set; }


        public override Expression<Func<TipoHierarquiaOferta, bool>> SatisfiedBy()
        {
            return t => (!this.Seq.HasValue || t.Seq == this.Seq.Value)
                     && (String.IsNullOrEmpty(Descricao)
                        || t.Descricao.ToLower().Contains(this.Descricao.ToLower()));
        }
    }
}
