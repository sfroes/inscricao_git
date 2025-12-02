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
    public class InscricaoDadoFormularioEmUsoSpecification : SMCSpecification<InscricaoDadoFormulario>
    {
        public long SeqFormulario { get; set; }

        public long? SeqVisaoSGF { get; set; }

        public long? SeqVisaoGestaoSGF { get; set; }

        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        public override Expression<Func<InscricaoDadoFormulario, bool>> SatisfiedBy()
        {
            AddExpression(x => x.SeqFormulario == SeqFormulario);
            AddExpression(x => x.SeqConfiguracaoEtapaPaginaIdioma == SeqConfiguracaoEtapaPaginaIdioma);
            if (SeqVisaoSGF.HasValue || SeqVisaoGestaoSGF.HasValue)
            {
                AddExpression(x => x.SeqVisao == SeqVisaoSGF.Value || x.SeqVisao == SeqVisaoGestaoSGF.Value);
            }

            return GetExpression();
        }
    }
}
