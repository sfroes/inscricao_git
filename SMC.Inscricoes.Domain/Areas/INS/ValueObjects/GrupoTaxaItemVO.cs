using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class GrupoTaxaItemVO : ISMCMappable
    {        
        public long Seq { get; set; }
        
        public long SeqGrupoTaxa { get; set; }

        public long SeqTaxa { get; set; }
        
        public long SeqTipoTaxa { get; set; }

        public string DescTipoTaxa { get; set; }

    }
}
