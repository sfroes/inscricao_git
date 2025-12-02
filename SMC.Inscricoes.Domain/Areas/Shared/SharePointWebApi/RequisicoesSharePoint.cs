using Newtonsoft.Json;
using SMC.Framework;
using SMC.Framework.Rest;
using SMC.Framework.Util;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.Shared
{
    public static class RequisicoesSharePoint
    {
        public static T EnviarPostApiGed<T>(object value, string action)
        {
            try
            {
                using (var httpCliente = new HttpClient())
                {
                    string fullUrl = $"{ApiUrlAmbiente.UrlAmbiente}{action}";
                    var data = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                    var resposta = httpCliente.PostAsync(fullUrl, data);
                    resposta.Wait();
                    var retorno = JsonConvert.DeserializeObject<T>(resposta.Result.Content.ReadAsStringAsync().Result);

                    retorno.GetType().GetProperty("StatusCode").SetValue(retorno, resposta.Result.StatusCode);

                    return retorno;
                }
            }
            catch (Exception ex)
            {
                var retorno = JsonConvert.DeserializeObject<T>(ex.Message);
                return retorno;
            }
        }
    }
}
