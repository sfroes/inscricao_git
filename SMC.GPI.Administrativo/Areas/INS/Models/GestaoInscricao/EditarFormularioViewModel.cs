using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class EditarFormularioViewModel : SMCViewModelBase
    {
        public SMCEncryptedLong Seq { get; set; }

        public SMCEncryptedLong SeqFormulario { get; set; }

        public SMCEncryptedLong SeqVisao { get; set; }

        [SMCHidden]
        public SMCEncryptedLong SeqInscricao { get; set; }

        public string Origem { get; set; }
    }
}