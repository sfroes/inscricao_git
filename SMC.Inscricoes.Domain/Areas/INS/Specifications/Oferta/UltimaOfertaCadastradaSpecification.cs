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
    public class UltimaOfertaCadastradaSpecification : SMCSpecification<Oferta>
    {
        public UltimaOfertaCadastradaSpecification(long seqProcesso)
        {
            // Filtro para buscar apenas um resultado (utilizado para buscar a data da ultima oferta cadastrada)
            SetOrderByDescending(o => o.DataInclusao);            
            MaxResults = 1;

            SeqProcesso = seqProcesso;
        }

        public long SeqProcesso { get; set; }

        public override Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            return i => i.SeqProcesso == SeqProcesso;
        }
    }
}
