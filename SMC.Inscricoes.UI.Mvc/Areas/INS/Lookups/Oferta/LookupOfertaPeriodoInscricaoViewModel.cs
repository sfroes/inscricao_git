using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SMC.Framework.DataAnnotations;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupOfertaPeriodoInscricaoViewModel : ISMCMappable
    {

        [SMCHidden]
        [SMCKey]
        [SMCMapProperty("SeqOferta")]
        public long Seq { get; set; }

        [SMCMapProperty("SeqHierarquiaOfertaPai")]
        [SMCHidden]
        public long SeqPai { get; set; }

        //[SMCHidden]
        //public long SeqProcesso { get; set; }

        [SMCDescription]
        public string Descricao { get; set; }

        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]        
        public DateTime? DataInicio { get; set; }

        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        public DateTime? DataFim { get; set; }

        public bool ExigePagamentoTaxa { get; set; }

        public bool ExigeEntregaDocumentaaco { get; set; }

        public int Vagas { get; set; }

        public int InscricoesConfirmadas { get; set; }

        [SMCHidden]
        public bool EOferta { get; set; }

        [SMCHidden]
        public bool IsLeaf { get; set; }

    }
}
