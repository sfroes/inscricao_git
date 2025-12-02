using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class TipoItemHierarquiaOfertaPorProcessoSpecification : SMCSpecification<TipoItemHierarquiaOferta>
    {
        public TipoItemHierarquiaOfertaPorProcessoSpecification(long seqProcesso)
        {
            SeqProcesso = seqProcesso;

            SetOrderBy(o => o.Descricao);
        }

        public long SeqProcesso { get; set; }

        public bool? HabilitaOferta { get; set; }

        public override Expression<Func<TipoItemHierarquiaOferta, bool>> SatisfiedBy()
        {
            return x => x.ItensHierarquiaOferta.Any(f => f.TipoHierarquiaOferta.Processo.Any(g => g.Seq == SeqProcesso)
                                                    && (!HabilitaOferta.HasValue || f.HabilitaCadastroOferta == HabilitaOferta.Value));
        }
    }
}
