using System.Net;

namespace SMC.Inscricoes.Rest.Models
{
    public class ResultBase
    {
        public bool success { get; set; }
        public string errorMessage { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }
}
