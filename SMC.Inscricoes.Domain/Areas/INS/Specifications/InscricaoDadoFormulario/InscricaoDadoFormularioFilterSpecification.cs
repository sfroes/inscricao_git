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
    public class InscricaoDadoFormularioFilterSpecification : SMCSpecification<InscricaoDadoFormulario>
    {
        public long? Seq { get; set; }
        public long? SeqInscricao { get; set; }

        public long? SeqFormulario { get; set; }

        public long? SeqVisao { get; set; }

        public long? SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        public override Expression<Func<InscricaoDadoFormulario, bool>> SatisfiedBy()
        {
            AddExpression(SeqInscricao, x => x.SeqInscricao == this.SeqInscricao);
            AddExpression(Seq, x => x.Seq == this.Seq.Value);
            AddExpression(SeqFormulario, x => x.SeqFormulario == this.SeqFormulario.Value);
            AddExpression(SeqVisao, x => x.SeqVisao == this.SeqVisao.Value);
            AddExpression(SeqConfiguracaoEtapaPaginaIdioma, x => x.ConfiguracaoEtapaPaginaIdioma.SeqConfiguracaoEtapaPagina == this.SeqConfiguracaoEtapaPaginaIdioma.Value);

            return GetExpression();


        }
    }
}
