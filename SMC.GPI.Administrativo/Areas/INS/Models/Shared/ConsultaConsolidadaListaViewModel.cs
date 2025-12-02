using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConsultaConsolidadaListaViewModel : SMCViewModelBase, ISMCMappable
    {
        //Qtd. Inscrições, Oferta não selecionada, Inscrições Iniciadas, Inscrições Finalizadas,
        //Pagas, Documentação entregue, Confirmadas, Não Confirmadas,  Deferidas, Indeferidas, Canceladas.
        [SMCHidden]
        public long Seq { get; set; }

        [SMCOrder(0)]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCValueEmpty("-")]
        public string Descricao { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCOrder(1)]
        public int Total { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCOrder(3)]
        public int Iniciadas { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCOrder(4)]
        public int Finalizadas { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCOrder(5)]
        public int Pagas { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCOrder(6)]
        public int DocumentacoesEntregues { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCOrder(7)]
        public int Confirmadas { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCOrder(8)]
        public int NaoConfirmadas { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCOrder(9)]
        public int Deferidos { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCOrder(10)]
        public int Indeferidos { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCOrder(11)]
        public int Canceladas { get; set; }
    }
}