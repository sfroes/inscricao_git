using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.RES.ValueObjects
{
    public class UnidadeResponsavelTipoFormularioVO : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqUnidadeResponsavel { get; set; }

        public long SeqTipoFormularioSGF { get; set; }

        public string DescricaoTipoFormulario { get; set; }

        public bool Ativo { get; set; }
    }
}
