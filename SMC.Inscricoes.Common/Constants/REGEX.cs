namespace SMC.Inscricoes.Common
{
    /// <summary>
    /// Constantes de expressões regulares
    /// </summary>
    public class REGEX
    {
        /// <summary>
        /// Expressão regular para validar um nome com duas ou mais palavras.
        /// Equivalente à expressão:
        /// ^[\l]+['\-\.\l]* [\l]+[ '\-\l]*$
        /// </summary>
        public const string NOME = @"^[a-zA-ZáâãéêíîóôõúûçÁÂÃÉÊÍÎÓÔÕÚÛÇ]+[`´'\-\.a-zA-ZáâãéêíîóôõúûçÁÂÃÉÊÍÎÓÔÕÚÛÇ]* [a-zA-ZáâãéêíîóôõúûçÁÂÃÉÊÍÎÓÔÕÚÛÇ]+[ `´'\-\.a-zA-ZáâãéêíîóôõúûçÁÂÃÉÊÍÎÓÔÕÚÛÇ]*$";

        /// <summary>
        /// Expressão regular para validação de tokens.
        /// Token deve utilizar apenas letras (A a Z), números (1 a 9) ou underline (_). Mínimo requerido de 3 caracteres.
        /// </summary>
        public const string TOKEN = @"^[A-Z1-9_]{3,}$";

        /// <summary>
        /// Expressão regular para validação de tokens.
        /// Sigla deve utilizar apenas letras (A a Z). Mínimo requerido de 3 caracteres.
        /// </summary>
        public const string SIGLA = @"^[A-Z]{3,}$";

        /// <summary>
        /// Expressão regular para validação de tags.
        /// Tag deve utilizar apenas letras (A a Z), números (1 a 9) ou underline (_) e os dois primeiros caracteres devem ser obrigatoriamente "{{" e os dois ultimos caracteres obrigatoriamente "}}".
        /// </summary>
        public const string TAG = @"^\{\{[A-Z1-9_]{3,}\}\}$";

    }
}