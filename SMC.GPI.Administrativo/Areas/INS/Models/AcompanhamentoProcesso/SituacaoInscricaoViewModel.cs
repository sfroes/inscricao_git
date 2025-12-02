using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class SituacaoInscricaoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long SeqInscricao { get; set; }

        [SMCHidden]
        public long SeqInscrito { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        public string NomeInscrito { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        public string Justificativa { get; set; }
    }
}