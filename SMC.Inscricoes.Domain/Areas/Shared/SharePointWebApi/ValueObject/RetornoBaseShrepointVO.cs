using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class RetornoBaseShrepointVO
    {
        public string ErroMessage { get; set; }

        public string Mensagem { get; set; }

        public HttpStatusCode StatusCode { get; set; }

    }
}
