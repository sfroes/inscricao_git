using SMC.Framework;

namespace SMC.Inscricoes.Domain.Areas.Shared
{
    public static class ApiUrlAmbiente
    {
        public static string UrlAmbiente
        {
            get
            {
                switch (SMCServerEnvHelper.GetEnvironment())
                {
                    case SMCServerEnvironment.Producao:
                        return "http://web.sistemas.pucminas.br/DadosMestres.WebAPI/Api/";

                    case SMCServerEnvironment.Homologacao:
                        return "http://web-homologacao.pucminas.br/DadosMestres.WebAPI/Api/";

                    case SMCServerEnvironment.Qualidade:
                        return "https://web-qualidade.pucminas.br/DadosMestres.WebAPI/Api/";

                    case SMCServerEnvironment.Desenvolvimento:
                        return "https://web-desenvolvimento.pucminas.br/DadosMestres.WebAPI/Api/";

                    default:
                        return "http://localhost/Dev/SMC.DadosMestres.WebApi/api/";
                }
            }
        }
    }
}
