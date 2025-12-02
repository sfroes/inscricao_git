using SMC.Framework;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ConfiguracaoEtapaPaginaIdiomaFilterSpecification : SMCSpecification<ConfiguracaoEtapaPaginaIdioma>
    {
        public long? SeqConfiguracaoEtapaPagina { get; set; }

        public long? SeqConfiguracaoEtapa { get; set; }

        public SMCLanguage? Idioma { get; set; }

        public long? SeqFormulario { get; set; }

        public long? SeqProcesso { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public long? SeqOferta { get; set; }

        public bool? ComFormulario { get; set; }

        public override System.Linq.Expressions.Expression<Func<ConfiguracaoEtapaPaginaIdioma, bool>> SatisfiedBy()
        {
            return p => (!SeqConfiguracaoEtapaPagina.HasValue || p.SeqConfiguracaoEtapaPagina == SeqConfiguracaoEtapaPagina)
                    && (!SeqOferta.HasValue || p.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.GruposOferta.Any(g => g.GrupoOferta.Ofertas.Any(o => o.Seq == SeqOferta)))
                    && (!SeqGrupoOferta.HasValue || p.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.GruposOferta.Any(g => g.SeqGrupoOferta == SeqGrupoOferta))
                    && (!Idioma.HasValue || p.Idioma == Idioma.Value)
                    && (!ComFormulario.HasValue || p.SeqFormularioSGF.HasValue == ComFormulario)
                    && (!SeqProcesso.HasValue || p.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.EtapaProcesso.SeqProcesso == SeqProcesso.Value)
                    && (!SeqFormulario.HasValue || p.SeqFormularioSGF == SeqFormulario.Value)
                    && (!SeqConfiguracaoEtapa.HasValue || p.ConfiguracaoEtapaPagina.SeqConfiguracaoEtapa == SeqConfiguracaoEtapa.Value);
        }
    }
}
