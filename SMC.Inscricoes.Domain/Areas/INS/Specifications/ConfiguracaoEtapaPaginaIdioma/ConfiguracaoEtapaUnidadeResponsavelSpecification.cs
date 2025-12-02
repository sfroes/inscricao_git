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
    public class ConfiguracaoEtapaUnidadeResponsavelSpecification : SMCSpecification<ConfiguracaoEtapaPaginaIdioma>
    {
        public ConfiguracaoEtapaUnidadeResponsavelSpecification(long seqUnidadeResponsavel, long seqFormularioSGF)
        {
            SeqUnidadeResponsavel = seqUnidadeResponsavel;
            SeqFormularioSGF = seqFormularioSGF;
        }

        public long SeqUnidadeResponsavel { get; set; }

        public long SeqFormularioSGF { get; set; }

        public override Expression<Func<ConfiguracaoEtapaPaginaIdioma, bool>> SatisfiedBy()
        {
            return p => p.SeqFormularioSGF == SeqFormularioSGF &&
                        p.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.EtapaProcesso.Processo.SeqUnidadeResponsavel == SeqUnidadeResponsavel;
        }
    }
}
