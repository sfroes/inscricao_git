using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AssociarItemHierarquiaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        [SMCReadOnly]
        [SMCSize(SMCSize.Grid4_24)]
        public long Seq { get; set; }

        [SMCHidden]
       // [SMCSize(SMCSize.Grid4_24)]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long? SeqPai { get; set; }

        [SMCReadOnly]
       // [SMCSize(SMCSize.Grid12_24)]
        public string DescricaoPai { get; set; }

        [SMCSelect("ItemsHierarquiaOferta", AutoSelectSingleItem = true, NameDescriptionField = "DescricaoItemHierarquiaOferta")]
        [SMCRequired]
        //[SMCSize(SMCSize.Grid20_24)]
        public long SeqItemHierarquiaOferta { get; set; }

        public List<SMCDatasourceItem> ItemsHierarquiaOferta { get; set; }

       // [SMCSize(SMCSize.Grid24_24)]
        [SMCRequired]
        public string Nome { get; set; }

        public bool PossuiIntegracao { get; set; }

        [SMCHidden]
        public bool PossuiCalculoBolsaSocial { get; set; }

        [SMCHidden]
        public bool BolsaExAluno { get; set; }
    }
}