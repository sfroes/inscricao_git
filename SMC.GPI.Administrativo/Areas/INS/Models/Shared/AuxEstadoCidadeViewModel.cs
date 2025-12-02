using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace SMC.GPI.Administrativo.Areas.INS.Models.Shared
{
    public class AuxEstadoCidadeViewModel : SMCViewModelBase, ISMCMappable
    {
        [ScriptIgnore]
        [SMCDataSource(SMCStorageType.TempData)]
        public List<SMCSelectListItem> Estados { get; set; }
        [ScriptIgnore]
        [SMCDataSource(SMCStorageType.TempData)]
        public List<SMCSelectListItem> Cidades { get; set; }
        [SMCMaxLength(100)]
        [SMCSelect("Estados")]
        [SMCSize(SMCSize.Grid9_24)]
        public string Estado { get; set; }
        [SMCDependencyAttribute("Estado", "BuscarCidades", "Localidade", true, new string[] { })]
        [SMCSelect("Cidades", NameDescriptionField = "DescCidadeSelecionada")]
        [SMCSize(SMCSize.Grid13_24)]
        public int? SeqCidade { get; set; }
        [SMCHidden]
        [SMCMaxLength(100)]
        public string DescCidadeSelecionada { get; set; }
    }
}