using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class TipoDocumentoFilterSpecification : SMCSpecification<ViewTipoDocumento>
    {
        public TipoDocumentoFilterSpecification()
        {
            SetOrderBy(x => x.Descricao);
        }

        public long? Seq { get; set; }

        public List<long> Seqs { get; set; }

        public string Descricao { get; set; }

        public TipoEmissao? TipoEmissao { get; set; }

        public override Expression<Func<ViewTipoDocumento, bool>> SatisfiedBy()
        {
            AddExpression(Seq, t => t.Seq == this.Seq.Value);
            AddExpression(Descricao, t => t.Descricao.StartsWith(Descricao));
            AddExpression(Seqs, x => Seqs.Contains(x.Seq));
            AddExpression(TipoEmissao, t => t.TipoEmissao == this.TipoEmissao);
            return GetExpression();
        }
    }
}
