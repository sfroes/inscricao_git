using SMC.Formularios.UI.Mvc.Models;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class EditarFormularioDadoViewModel : DadoFormularioViewModel
    {
        [SMCHidden]
        public long SeqInscricao { get; set; }

        public string Origem { get; set; }
    }
}