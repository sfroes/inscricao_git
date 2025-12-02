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
    public class ArquivoSecaoPaginaFilterSpecification : SMCSpecification<ArquivoSecaoPagina>
    {
        public ArquivoSecaoPaginaFilterSpecification()
        {
            SetOrderBy(f => f.Ordem);
        }
        

        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }
        public long SeqSecaoPaginaSGF { get; set; }


        public override Expression<Func<ArquivoSecaoPagina, bool>> SatisfiedBy()
        {
            return c => c.SeqSecaoPaginaSGF == SeqSecaoPaginaSGF &&
                c.SeqConfiguracaoEtapaPaginaIdioma == SeqConfiguracaoEtapaPaginaIdioma;
        }
    }
}
