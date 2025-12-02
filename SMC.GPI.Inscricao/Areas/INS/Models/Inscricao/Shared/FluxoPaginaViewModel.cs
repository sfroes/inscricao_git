using SMC.Formularios.UI.Mvc.Model;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class FluxoPaginaViewModel : TemplateFluxoPaginaViewModel
    {
        public long? SeqPaginaIdioma { get; set; }

        public bool ExibeConfirmacaoInscricao { get; set; }

        public bool ExibeComprovanteInscricao { get; set; }

        public string Alerta { get; set; }
    }
}