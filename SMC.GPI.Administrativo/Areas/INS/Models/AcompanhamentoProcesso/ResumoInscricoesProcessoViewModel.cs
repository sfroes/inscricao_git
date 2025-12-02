using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ResumoInscricoesProcessoViewModel : SMCViewModelBase,ISMCMappable
    {

        public string Descricao { get; set; }


        public int Total { get; set; }

        public int InscricoesIniciadas { get; set; }


        public int InscricoesFinalizadas { get; set; }


        public int InscricoesCanceladas { get; set; }


        public int InscricoesConfirmadas { get; set; }

        public int InscricoesDeferidas { get; set; }


        public int InscricoesIndeferidas { get; set; }

    }
}
