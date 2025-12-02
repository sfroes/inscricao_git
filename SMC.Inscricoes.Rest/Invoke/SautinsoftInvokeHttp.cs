
using Newtonsoft.Json;
using SMC.Framework;
using SMC.Inscricoes.Common.Enums;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace SMC.Inscricoes.Pdf.Invoke
{

    public class SautinsoftInvokeHttp
    {
        private readonly HttpClient httpClient;

        public SautinsoftInvokeHttp()
        {
            //var handler = new HttpClientHandler()
            //{
            //    //ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            //    //{
            //    //    return true;
            //    //}
            //};

            this.httpClient = new HttpClient();
        }

        private string UrlBase
        {
            get
            {
                switch (SMCServerEnvHelper.GetEnvironment())
                {
                    case SMCServerEnvironment.Producao:
                        return ConfigurationManager.AppSettings["UrlProd"];

                    case SMCServerEnvironment.Homologacao:
                        return ConfigurationManager.AppSettings["UrlHom"];

                    case SMCServerEnvironment.Qualidade:
                        return ConfigurationManager.AppSettings["UrlQa"];

                    case SMCServerEnvironment.Desenvolvimento:
                        return ConfigurationManager.AppSettings["UrlDev"];

                    default:
                        return ConfigurationManager.AppSettings["UrlLocal"];

                }
            }
        }
        public T Send<T>(object value, MetodoHttp metodoHttp, string rota)
        {
            try
            {
                //string fullURL = UrlBase + rota;
                string fullURL = ConfigurationManager.AppSettings["Sautinsoft"] + rota;

                HttpResponseMessage result = null;

                if (metodoHttp == MetodoHttp.GET)
                {
                    result = httpClient.GetAsync(fullURL).Result;
                }
                else if (metodoHttp == MetodoHttp.POST)
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                    result = httpClient.PostAsync(fullURL, jsonContent).Result;
                }
                else if (metodoHttp == MetodoHttp.DELETE)
                {
                    result = httpClient.DeleteAsync(fullURL).Result;
                }
                else
                {
                    throw new NotSupportedException($"O método HTTP '{metodoHttp}' não é suportado.");
                }

                var content = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception)
            {
                throw;

            }
        }
    }

}
