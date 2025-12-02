using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class HierarquiaOfertaComGrupoFiltroData : ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public long? SeqGrupoOferta { get; set; }

        [SMCKeyModel]
        public long[] SeqOfertas { get; set; }
    }
}
