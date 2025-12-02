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
    public class DetalheLancamentoNotaClassificacaoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long SeqInscricao { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCReadOnly]
        public string NomeInscrito { get; set; }

        
        [SMCSize(SMCSize.Grid6_24)]
        [SMCMinValue(1)]
        public int? NumeroClassificacao { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCMaxValue(100)]
        [SMCMinValue(0)]
        public decimal? NotaGeral { get; set; }

    }
}