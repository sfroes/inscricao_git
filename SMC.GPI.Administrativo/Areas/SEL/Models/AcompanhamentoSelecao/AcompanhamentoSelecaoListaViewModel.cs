using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class AcompanhamentoSelecaoListaViewModel : SMCViewModelBase
    {
        public long Seq { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCValueEmpty("-")]
        public string Descricao { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public int CandidatosConfirmados { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public int CandidatosDesistentes { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public int CandidatosReprovados { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public int CandidatosSelecionados { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public int CandidatosExcedentes { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public int Convocados { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public int ConvocadosDesistentes { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public int ConvocadosConfirmados { get; set; }
    }
}