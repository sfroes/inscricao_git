using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class ProcessoHierarquiaQRcodeData : ISMCMappable
    {
        public long SeqProcesso { get; set; }
        public long SeqGrupoOferta { get; set; }
        public long SeqHierarquia { get; set; }
        public string DescricaoHierarquia { get; set; }
    }
}
