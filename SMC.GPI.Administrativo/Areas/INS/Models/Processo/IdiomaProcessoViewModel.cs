using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class IdiomaProcessoViewModel : SMCViewModelBase, ISMCMappable
    {

        public IdiomaProcessoViewModel()
        {
            Padrao = true;
        }

        [SMCOrder(1)]
        [SMCHidden]
        [SMCKey]
        [SMCSize(SMCSize.Grid4_24)]
        public long Seq { get; set; }

        [SMCOrder(2)]
        [SMCHidden]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqProcesso { get; set; }

        [SMCOrder(3)]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCSelect(IgnoredEnumItems = new object[] { SMCLanguage.English, SMCLanguage.French, SMCLanguage.German, SMCLanguage.Italian, SMCLanguage.Spanish })]
        [SMCRequired]
        [SMCDescription]
        [SMCConditionalReadonly("Seq", SMCConditionalOperation.NotEqual, 0, PersistentValue = true)]
        public SMCLanguage Idioma { get; set; }

        [SMCOrder(4)]
        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect]
        public bool Padrao { get; set; }

        [SMCOrder(5)]
        [SMCRequired]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCHidden(SMCViewMode.List)]
        [SMCHtml]
        public string DescricaoComplementar { get; set; }

        [SMCOrder(6)]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        public string LabelCodigoAutorizacao { get; set; }

        [SMCOrder(7)]
        [SMCRequired]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        public string LabelGrupoOferta { get; set; }

        [SMCOrder(8)]
        [SMCRequired]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        public string LabelOferta { get; set; }
    }
}