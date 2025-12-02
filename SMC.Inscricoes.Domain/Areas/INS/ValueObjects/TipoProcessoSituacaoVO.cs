using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class TipoProcessoSituacaoVO : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqTipoProcesso { get; set; }
                
        [SMCMapProperty("Descricao")]
        public string DescricaoSGF { get; set; }

        public string DescricaoInformada { get; set; }

        public bool ExibeJustificativa { get; set; }
    }
}
