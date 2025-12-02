using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class PosicaoConsolidadaPorGrupoOfertaFiltroViewModel : SMCPagerViewModel
    {
        #region Cabeçalho
        public string TipoProcesso { get; set; }        

        public string Descricao { get; set; }

        public int CandidatosConfirmados { get; set; }

        public int CandidatosDesistentes { get; set; }

        public int CandidatosReprovados { get; set; }

        public int CandidatosSelecionados { get; set; }

        public int CandidatosExcedentes { get; set; }

        public int Convocados { get; set; }

        public int ConvocadosDesistentes { get; set; }

        public int ConvocadosConfirmados { get; set; }
        #endregion

        #region DataSources        
        public List<SMCDatasourceItem> GrupoOfertas { get; set; }
        #endregion

        #region Filtro
        [SMCFilterKey]
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCFilter]
        [SMCSelect("GrupoOfertas")]
        [SMCSize(SMCSize.Grid5_24)]
        [SMCSingleFill("oferta")]
        public long? SeqGrupoOferta { get; set; }

        [SMCFilter]
        [LookupHierarquiaOferta]
        [SMCSize(SMCSize.Grid7_24)]
        [SMCDependency("SeqProcesso")]
        [SMCSingleFill("oferta")]
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
        public GPILookupViewModel SeqOferta { get; set; }
        #endregion
    }
}