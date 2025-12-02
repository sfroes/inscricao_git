using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class RespostaCheckinData : ISMCMappable
    {
        public string Mensagem { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
