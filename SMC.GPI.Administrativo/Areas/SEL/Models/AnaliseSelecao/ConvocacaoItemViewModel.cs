using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class ConvocacaoItemViewModel : SMCViewModelBase
    {
        [SMCSize(SMCSize.Grid8_24)]
        [SMCReadOnly]
        public long SeqInscricao { get; set; }

        [SMCHidden]
        public long SeqInscricaoOferta { get; set; }
        
        [SMCIgnoreProp]
        public string Situacao { get; set; }

        [SMCIgnoreProp]
        public string TokenSituacao { get; set; }

        [SMCIgnoreProp]
        public string TokenEtapa { get; set; }

        [SMCSize(SMCSize.Grid16_24)]
        [SMCReadOnly]
        public string Nome { get; set; }        
    }
}