using SMC.Formularios.UI.Mvc.Controls;
using SMC.Formularios.UI.Mvc.Model;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AnaliseInscricaoLoteFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        #region Datasources
        public List<SMCDatasourceItem> SituacoesProcesso { get; set; }

        public List<SMCDatasourceItem> GruposOferta { get; set; }
        #endregion

        [SMCFilter]
        [SMCSGFFilter]
        [SMCSize(Framework.SMCSize.Grid8_24)]
        public SGFFilterModel FiltroSGF { get; set; }

        [SMCHidden]
        [SMCFilterKey]
        public long SeqProcesso { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCSelect("SituacoesProcesso")]
        [SMCRequired]
        [SMCFilter]
        public long SeqTipoProcessoSituacao { get; set; }

        [SMCSize(SMCSize.Grid14_24)]
        [SMCFilter]
        public string NomeInscrito { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCSelect("GruposOferta")]
        [SMCFilter]
        [SMCSingleFill("oferta")]
        public long SeqGrupoOferta { get; set; }

        [SMCFilter]
        [LookupHierarquiaOferta]
        [SMCSize(SMCSize.Grid9_24)]
        [SMCDependency("SeqProcesso")]
        [SMCSingleFill("oferta")]
        public LookupHierarquiaOfertaViewModel SeqItemHierarquiaOferta { get; set; }

        [SMCFilter]
        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid9_24)]
        [SMCDependency("SeqGrupoOferta")]
        [SMCDependency("SeqItemHierarquiaOferta")]
        [SMCDependency("SeqProcesso")]
        [SMCConditionalReadonly("SeqGrupoOferta", "", RuleName = "R1")]
        [SMCConditionalReadonly("SeqItemHierarquiaOferta", "", RuleName = "R2")]
        [SMCConditionalRule("R1 && R2")]
        public GPILookupViewModel Oferta { get; set; }
    }
}