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
    public class TipoProcessoSituacaoViewModel : SMCViewModelBase, ISMCMappable
    {

        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqTipoProcesso { get; set; }

        [SMCHidden]
        public long SeqSituacao { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCReadOnly]
        public string DescricaoSGF { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCRequired]
        public string Descricao { get; set; }

        [SMCHidden]
        public string Token { get; set; }
    }
}