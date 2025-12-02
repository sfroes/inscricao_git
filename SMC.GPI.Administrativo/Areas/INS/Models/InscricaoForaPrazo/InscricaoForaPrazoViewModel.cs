using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class InscricaoForaPrazoViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqInscritoEdicao { get; set; }

        [SMCRequired]
        [SMCSize(Framework.SMCSize.Grid4_24)]
        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        [SMCMinDateNow]
        public DateTime DataInicio { get; set; }

        [SMCRequired]
        [SMCSize(Framework.SMCSize.Grid4_24)]
        [SMCMinDate("DataInicio")]
        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        public DateTime DataFim { get; set; }

        [SMCMinDate("DataFim")]
        [SMCSize(Framework.SMCSize.Grid4_24)]
        public DateTime? DataVencimento { get; set; }

        [SMCRequired]
        [SMCSize(Framework.SMCSize.Grid24_24)]
        [LookupInscrito]
        public List<LookupInscritoViewModel> Inscritos { get; set; }
    }
}