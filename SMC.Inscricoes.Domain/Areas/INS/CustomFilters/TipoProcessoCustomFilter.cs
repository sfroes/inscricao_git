using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SMC.Framework.DataAnnotations;
using SMC.Inscricoes.Common;

namespace SMC.Inscricoes.Domain.Areas.INS.CustomFilters
{
    public class TipoProcessoCustomFilter : SMCCustomFilter<TipoProcesso>
    {
        [SMCFilterParameter(FILTERS.UNIDADE_RESPONSAVEL, true)]
        public long[] SeqUnidadeResponsavel { get; set; }

        public override Expression<Func<TipoProcesso, bool>> SatisfiedBy()
        {
            return x => x.UnidadeResponsavelTipoProcesso.Any(f => SeqUnidadeResponsavel.Contains(f.SeqUnidadeResponsavel));
        }
    }
}
