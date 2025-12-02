using SMC.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Shared
{
    public static class HttpContextAmbiente
    {
        /// <summary>
        /// Esquema do ambiente http ou https
        /// </summary>
        public static string SchemaAmiente
        {
            get
            {
                switch (SMCServerEnvHelper.GetEnvironment())
                {
                    case SMCServerEnvironment.Desenvolvimento:
                        return "https";
                    case SMCServerEnvironment.Qualidade:
                        return "https";
                    case SMCServerEnvironment.Homologacao:
                        return "https";
                    case SMCServerEnvironment.Producao:
                        return "https";
                    default:
                        return "http";
                }
            }
        }


        /// <summary>
        /// Monta a url baseada no ambiente
        /// </summary>
        /// <param name="url">Url base do ambiente Ex: GPI.Administrativo/INS/AcompanhamentoInscrito</param>
        /// <returns></returns>
        public static string UrlAmbiente(string url)
        {
            switch (SMCServerEnvHelper.GetEnvironment())
            {
                case SMCServerEnvironment.Desenvolvimento:
                    return $"https://web-desenvolvimento.pucminas.br/{url.TrimStart('/')}";
                case SMCServerEnvironment.Qualidade:
                    return $"https://web-qualidade.pucminas.br/{url.TrimStart('/')}";
                case SMCServerEnvironment.Homologacao:
                    return $"https://web-homologacao.pucminas.br/{url.TrimStart('/')}";
                case SMCServerEnvironment.Producao:
                    return $"https://web.sistemas.pucminas.br/{url.TrimStart('/')}";
                default:
                    return $"http://localhost/Dev/{url.TrimStart('/')}";
            }


        }

    }
}
