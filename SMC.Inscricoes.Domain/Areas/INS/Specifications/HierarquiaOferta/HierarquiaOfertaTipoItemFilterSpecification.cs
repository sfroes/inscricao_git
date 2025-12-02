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
    public class HierarquiaOfertaTipoItemFilterSpecification : SMCSpecification<HierarquiaOferta>
    {
        public HierarquiaOfertaTipoItemFilterSpecification()
        {
            SetOrderBy(o => o.ItemHierarquiaOferta.TipoItemHierarquiaOferta.Descricao);
            SetOrderBy(o => o.DescricaoCompleta);
        }

        public long SeqProcesso { get; set; }

        public long? SeqTipoItem { get; set; }

        public string DescricaoHierarquia { get; set; }

        public override Expression<Func<HierarquiaOferta, bool>> SatisfiedBy()
        {
            AddExpression(x => x.SeqProcesso == SeqProcesso);
            AddExpression(SeqTipoItem, x => x.ItemHierarquiaOferta.SeqTipoItemHierarquiaOferta == SeqTipoItem);
            AddExpression(DescricaoHierarquia, x => x.DescricaoCompleta.ToLower().Contains(DescricaoHierarquia.ToLower()));
            AddExpression(h => !(h is Oferta));

            return GetExpression();
        }
    }
}
