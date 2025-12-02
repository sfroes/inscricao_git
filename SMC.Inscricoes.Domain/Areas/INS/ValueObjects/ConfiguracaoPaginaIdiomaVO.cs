using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ConfiguracaoPaginaIdiomaVO : ISMCMappable
    {

        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        public long SeqUnidadeResponsavel { get; set; }

        public long SeqPaginaEtapaSGF { get; set; }

        public string Titulo { get; set; }

        public bool ExibeFormulario { get; set; }

        public long? SeqTipoFormulario { get; set; }

        public long? SeqFormulario { get; set; }

        public long? SeqVisao { get; set; }        
        
        public long? SeqVisaoGestao { get; set; }

        public string PaginaToken { get; set; }

        public long SeqProcesso { get; set; }
    }
}
