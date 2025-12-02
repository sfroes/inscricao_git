namespace SMC.Inscricoes.Common.Areas.INS.Constants
{
    public class WEB_API_REST
    {
        /// <summary>
        /// Chave com o token de autenticação cifrado
        /// </summary>
        public const string TOKEN_BOLETO_KEY = "TokenBoleto";

        /// <summary>
        /// Chave da base url no web config
        /// </summary>
        public const string BASE_URL_KEY = "BaseUrlRest";

        /// <summary>
        /// Timeout para geração de boleto em milisegundos
        /// </summary>
        public const string CANCELLATION_TIME_KEY = "WebApiRestCancellationTime";

        /// <summary>
        /// Realiza registo e emissão de boletos para alunos
        /// </summary>
        public const string EMITIR_BOLETO_INSCRITO = "Boleto.WebApi/BLT/Boleto/EmitirBoletoInscrito";
    }
}
