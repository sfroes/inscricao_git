using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TotalInscricoesProcessoViewModel : SMCViewModelBase,ISMCMappable
    {

        public long SeqProcesso { get; set; }

        public string Descricao { get; set; }

        public int TotalInscricoes { get; set; }

    }
}
