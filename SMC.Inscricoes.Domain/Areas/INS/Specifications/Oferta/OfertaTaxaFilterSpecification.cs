using SMC.Framework.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class OfertaTaxaFilterSpecification : OfertaFilterSpecification
    {


        public OfertaTaxaFilterSpecification()
        {
            //Ordenações padrão
            //SetOrderBy(x => x.GrupoOferta.Nome);
            //SetOrderBy(x => x.Nome);
            //SetOrderByDescending(x => x.DataInicio);
        }

        public long? SeqTipoTaxa { get; set; }
    }
}
