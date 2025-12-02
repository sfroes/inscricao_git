using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaInstrucaoInicialViewModel : PaginaViewModel
    {
        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_INSTRUCOES_INICAIS;
            }
        }

    

    }
}