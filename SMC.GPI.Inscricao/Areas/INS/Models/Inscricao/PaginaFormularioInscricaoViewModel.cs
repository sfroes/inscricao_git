using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common;
using System.Linq;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaFormularioInscricaoViewModel : PaginaViewModel, ISMCMappable
    {
        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_FORMULARIO_INSCRICAO;
            }
        }

        [SMCHidden]
        public long SeqDadoFormulario { get; set; }

        private long? seqPaginaIdioma;

        [SMCHidden]
        public long? SeqPaginaIdioma
        {
            get
            {
                if (!seqPaginaIdioma.HasValue && this.FluxoPaginas.Count > 0 && this.SeqConfiguracaoEtapaPagina > 0)
                {
                    var fluxo = this.FluxoPaginas.Where(p => p.SeqConfiguracaoEtapaPagina.Equals(this.SeqConfiguracaoEtapaPagina)).SingleOrDefault() as FluxoPaginaViewModel;
                    seqPaginaIdioma = fluxo != null ? fluxo.SeqPaginaIdioma : null;
                }
                return seqPaginaIdioma;
            }
            set
            {
                seqPaginaIdioma = value;
            }
        }

        [SMCHidden]
        public long? SeqFormularioSGF
        {
            get
            {
                if (this.FluxoPaginas.Count > 0 && this.SeqConfiguracaoEtapaPagina > 0)
                {
                    var fluxo = this.FluxoPaginas.Where(p => p.SeqConfiguracaoEtapaPagina.Equals(this.SeqConfiguracaoEtapaPagina)).SingleOrDefault();
                    return fluxo != null ? fluxo.SeqFormularioSGF : null;
                }
                return null;
            }
            set
            {
                if (this.FluxoPaginas.Count > 0 && this.SeqConfiguracaoEtapaPagina > 0)
                {
                    var fluxo = this.FluxoPaginas.Where(p => p.SeqConfiguracaoEtapaPagina.Equals(this.SeqConfiguracaoEtapaPagina)).SingleOrDefault();
                    if (fluxo != null)
                        fluxo.SeqFormularioSGF = value;
                }
            }
        }

        [SMCHidden]
        public long? SeqVisaoSGF
        {
            get
            {
                if (this.FluxoPaginas.Count > 0 && this.SeqConfiguracaoEtapaPagina > 0)
                {
                    var fluxo = this.FluxoPaginas.Where(p => p.SeqConfiguracaoEtapaPagina.Equals(this.SeqConfiguracaoEtapaPagina)).SingleOrDefault();
                    return fluxo != null ? fluxo.SeqVisaoSGF : null;
                }
                return null;
            }
            set
            {
                if (this.FluxoPaginas.Count > 0 && this.SeqConfiguracaoEtapaPagina > 0)
                {
                    var fluxo = this.FluxoPaginas.Where(p => p.SeqConfiguracaoEtapaPagina.Equals(this.SeqConfiguracaoEtapaPagina)).SingleOrDefault();
                    if (fluxo != null)
                        fluxo.SeqVisaoSGF = value;
                }
            }
        }
    }
}