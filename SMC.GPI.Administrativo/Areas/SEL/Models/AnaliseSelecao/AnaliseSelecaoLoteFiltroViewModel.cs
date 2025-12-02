using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class AnaliseSelecaoLoteFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        #region Cabeçalho
        public string TipoProcesso { get; set; }

        public string Descricao { get; set; }
        #endregion

        #region DataSources        
        public List<SMCDatasourceItem> SituacoesProcesso { get; set; }

        public List<SMCDatasourceItem> GrupoOfertas { get; set; }
        #endregion

        #region Filtro
        [SMCFilterKey]
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCFilter]
        [SMCSelect("SituacoesProcesso")]
        [SMCSize(SMCSize.Grid5_24)]
        public long? SeqTipoProcessoSituacao { get; set; }

        [SMCFilter]
        [SMCSelect]
        [SMCDependency("SeqSituacao", "BuscarMotivos", "AcompanhamentoProcesso", "INS", true)]
        [SMCSize(SMCSize.Grid6_24)]
        public long? SeqMotivo { get; set; }

        [SMCFilter]
        [SMCSelect("GrupoOfertas")]
        [SMCSize(SMCSize.Grid5_24)]
        [SMCSingleFill("oferta")]
        public long? SeqGrupoOferta { get; set; }

        [SMCFilter]
        [LookupHierarquiaOferta]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCDependency("SeqProcesso")]
        [SMCSingleFill("oferta")]
        [SMCConditionalRequired("SeqGrupoOferta", "")]
        public LookupHierarquiaOfertaViewModel SeqItemHierarquiaOferta { get; set; }

        [SMCFilter]
        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCDependency("SeqGrupoOferta")]
        [SMCDependency("SeqItemHierarquiaOferta")]
        [SMCDependency("SeqProcesso")]
        [SMCConditionalReadonly("SeqGrupoOferta", "", RuleName = "R1")]
        [SMCConditionalReadonly("SeqItemHierarquiaOferta", "", RuleName = "R2")]
        [SMCConditionalRule("R1 && R2")]
        [SMCRequired]
        public GPILookupViewModel Oferta { get; set; }

        [SMCFilter]
        [SMCMinValue(0)]
        [SMCMask("999")]
        [SMCSize(SMCSize.Grid3_24)]
        public int? Opcao { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid5_24)]
        public long? SeqInscricao { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid8_24)]
        public string Candidato { get; set; }
        #endregion
    }
}