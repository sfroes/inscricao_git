using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class PaginaVO : ISMCMappable
    {
        public long SeqConfiguracaoEtapaPagina { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public string Titulo { get; set; }

        public int Ordem { get; set; }

        public List<SecaoPaginaVO> Secoes { get; set; }

    }
}
