using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class ProcessoGrupoOfertaQRCodeData : ISMCMappable
    {
        public long SeqGrupoOferta { get; set; }
        public long SeqProcesso { get; set; }
        public string DescricaoGrupoOferta { get; set; }
    }
}
