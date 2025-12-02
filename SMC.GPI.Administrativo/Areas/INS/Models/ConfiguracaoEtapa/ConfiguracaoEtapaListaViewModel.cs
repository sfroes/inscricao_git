using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConfiguracaoEtapaListaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqEtapaProcesso { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCSortable]
        public string  Nome { get; set; }
                
        [SMCSortable]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataInicio { get; set; }

        [SMCSortable]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataFim { get; set; }

    }
}