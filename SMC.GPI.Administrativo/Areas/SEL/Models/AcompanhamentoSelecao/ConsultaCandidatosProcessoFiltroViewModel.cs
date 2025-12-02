using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.INS;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class ConsultaCandidatosProcessoFiltroViewModel : SMCPagerViewModel
    {
        #region Cabeçalho

        public string TipoProcesso { get; set; }

        public string Descricao { get; set; }

        #endregion Cabeçalho

        #region DataSources

        [SMCDataSource]
        public List<SMCDatasourceItem> SituacoesProcesso { get; set; }

        [SMCDataSource]
        public List<SMCDatasourceItem> GrupoOfertas { get; set; }

        #endregion DataSources

        #region Filtro

        [SMCFilterKey]
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCFilter]
        [SMCSelect(nameof(SituacoesProcesso))]
        [SMCSize(SMCSize.Grid5_24)]
        public long? SeqTipoProcessoSituacao { get; set; }

        [SMCFilter]
        [SMCSelect]
        [SMCDependency(nameof(SeqTipoProcessoSituacao), nameof(AcompanhamentoProcessoController.BuscarMotivos), "AcompanhamentoProcesso", "INS", true)]
        [SMCSize(SMCSize.Grid6_24)]
        public long? SeqMotivo { get; set; }

        [SMCFilter]
        [SMCSelect(nameof(GrupoOfertas))]
        [SMCSize(SMCSize.Grid5_24)]
        [SMCSingleFill("oferta")]
        public long? SeqGrupoOferta { get; set; }

        [SMCFilter]
        [LookupHierarquiaOferta]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCDependency(nameof(SeqProcesso))]
        [SMCSingleFill("oferta")]
        public LookupHierarquiaOfertaViewModel SeqItemHierarquiaOferta { get; set; }

        [SMCFilter]
        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCDependency(nameof(SeqGrupoOferta))]
        [SMCDependency(nameof(SeqItemHierarquiaOferta))]
        [SMCDependency(nameof(SeqProcesso))]
        [SMCConditionalReadonly(nameof(SeqGrupoOferta), "", RuleName = "R1")]
        [SMCConditionalReadonly(nameof(SeqItemHierarquiaOferta), "", RuleName = "R2")]
        [SMCConditionalRule("R1 && R2")]
        public GPILookupViewModel SeqOferta { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMinValue(1)]
        [SMCMask("999")]
        [SMCSortable(true, false, "NumeroOpcao")]
        public long? Opcao { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid5_24)]
        public long? SeqInscricao { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCSortable(true, true, "Inscricao.Inscrito.Nome")]
        public string Candidato { get; set; }

        
        #endregion Filtro
    }
}