using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class OpcaoOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqOferta { get; set; }

        public int NumeroOpcao { get; set; }

        public string DescricaoOferta { get; set; }

        public string DescricaoOfertaOriginal { get; set; }

        public string Justificativa { get; set; }

        public string HierarquiaCompleta { get; set; }

        public string SituacaoOferta { get; set; }

        public string NumeroOpcaoFormatado { get; set; }

        public long SeqProcesso { get; set; }

        public List<long> InscricaoOferta { get; set; }

        public long SeqInscricao { get; set; }
        public long SeqInscrito { get; set; }

        public bool ExibeOpcoes { get; set; }

        public long SeqInscricaoOferta { get; set; }

        public string BackURL { get; set; }

        public string NomeInscrito { get; set; }
    }
}
