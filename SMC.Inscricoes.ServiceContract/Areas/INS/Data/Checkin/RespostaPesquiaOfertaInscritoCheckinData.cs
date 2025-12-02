using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class RespostaPesquiaOfertaInscritoCheckinData : ISMCMappable
    {
        public string Nome { get; set; }
        public string GuidInscricao { get; set; }
        public bool CheckinEfetuado { get; set; }
    }
}
