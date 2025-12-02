using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class RespostaPesquiaOfertaCheckinData : ISMCMappable
    {
        public string DescricaoOferta { get; set; }
        public long SeqOferta { get; set; }
        public List<RespostaPesquiaOfertaInscritoCheckinData> Inscritos { get; set; }
    }
}
