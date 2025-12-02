using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TaxaProcessoViewModel : SMCViewModelBase, ISMCMappable
    {

        [SMCHidden]
        public string SeqTipoTaxaDescricao { get; set; }

        [SMCHidden]
        public long? Seq { get; set; }

        [SMCHidden]
        public long? SeqProcesso { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCSelect("TiposTaxa", NameDescriptionField = nameof(SeqTipoTaxaDescricao))]
        [SMCDependency("SeqTipoProcesso", "BuscarTiposTaxa", "Processo", true)]
        [SMCRequired]
        public long SeqTipoTaxa { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCSelect]
        [SMCRequired]
        public TipoCobranca TipoCobranca { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        [SMCMaxLength(100)]
        public string DescricaoComplementar { get; set; }

        [SMCHidden]
        public string Descricao { get; set; }
        [SMCHidden]
        [SMCMapProperty("TipoCobranca")]
        public TipoCobranca TipoCobrancaOriginal
        {
            get; set;
        }
    }
}