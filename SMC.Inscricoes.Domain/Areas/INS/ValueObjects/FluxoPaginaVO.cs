using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class FluxoPaginaVO : ISMCMappable
    {
        public long SeqConfiguracaoEtapaPagina { get; set; }

        public int Ordem { get; set; }

        public string Token { get; set; }

        public string Titulo { get; set; }

        public long? SeqPaginaIdioma { get; set; }

        public long? SeqFormularioSGF { get; set; }

        public long? SeqVisaoSGF { get; set; }

        public bool ExibeConfirmacaoInscricao { get; set; }

        public bool ExibeComprovanteInscricao { get; set; }
    }
}
