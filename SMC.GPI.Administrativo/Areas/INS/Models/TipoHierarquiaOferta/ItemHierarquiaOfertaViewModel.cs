using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ItemHierarquiaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long SeqTipoHierarquiaOferta { get; set; }

        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long? SeqPai { get; set; }
       
        [SMCReadOnly]
        [SMCSize(SMCSize.Grid20_24)]
        public string DescricaoPai { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid8_24)]
        public bool HabilitaCadastroOferta { get; set; }    

        [SMCSelect("TiposItemHierarquiaOferta")]
        [SMCRequired]
        [SMCSize(SMCSize.Grid20_24)]
        public long SeqTipoItemHierarquiaOferta { get; set; }
        
        public List<SMCDatasourceItem> TiposItemHierarquiaOferta { get; set; }        
  
    }
}