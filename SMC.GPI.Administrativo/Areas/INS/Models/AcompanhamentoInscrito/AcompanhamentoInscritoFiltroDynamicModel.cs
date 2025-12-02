using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Dynamic;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AcompanhamentoInscritoFiltroDynamicModel : SMCDynamicFilterViewModel
    {
        #region [Itens ocultos]
        [SMCHidden]
        [SMCDataSource]
        [SMCServiceReference(typeof(ITipoProcessoService), nameof(ITipoProcessoService.BuscarTiposProcessoKeyValue))]
        public List<SMCSelectListItem> TiposProcessos { get; set; }


        [SMCHidden]
        [SMCDataSource]
        [SMCServiceReference(typeof(IProcessoService), nameof(IProcessoService.BuscarProcessoSelect))]
        public List<SMCSelectListItem> Processos { get; set; }

        [SMCHidden]
        [SMCDataSource]
        [SMCServiceReference(typeof(IUnidadeResponsavelService), nameof(IUnidadeResponsavelService.BuscarUnidadesResponsaveisKeyValue))]
        public List<SMCSelectListItem> Unidades { get; set; }


        [SMCHidden]
        public long? Seq { get; set; }

        [SMCHidden]
        public long? SeqGrupoOferta { get; set; }

        #endregion

        #region Processo

        [SMCFilter]
        [SMCSelect("Unidades", AutoSelectSingleItem = true)]
        [SMCSize(SMCSize.Grid8_24)]
        public long? SeqUnidadeResponsavel { get; set; }

        [SMCFilter]
        [SMCSelect(nameof(TiposProcessos))]
        [SMCDependency(nameof(SeqUnidadeResponsavel), "VerificaUnidadeResponsavel", "AcompanhamentoInscrito", false, Remote = true)]
        [SMCSize(SMCSize.Grid8_24)]
        public long? SeqTipoProcesso { get; set; }

        [SMCFilter]
        [SMCSelect(nameof(Processos), UseCustomSelect = true)]
        [SMCDependency(nameof(SeqUnidadeResponsavel), "FiltroProcessos", "AcompanhamentoInscrito", false, Remote = true)]
        [SMCDependency(nameof(SeqTipoProcesso), "FiltroProcessos", "AcompanhamentoInscrito", false, Remote = true)]
        [SMCSize(SMCSize.Grid8_24)]
        public long? SeqProcesso { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinValue(1)]
        [SMCMaxValue(2)]
        public long? SemestreReferencia { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMask("0000")]
        public int? AnoReferencia { get; set; }

        #endregion


        #region [Oferta]

        [SMCFilter]
        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid16_24)]
        [SMCDependency(nameof(SeqGrupoOferta))]
        [SMCDependency(nameof(SeqProcesso))]
        [SMCConditionalReadonly(nameof(SeqProcesso), "", RuleName = "R1")]
        [SMCConditionalRule("R1")]
        public GPILookupViewModel Oferta { get; set; }

        #endregion

        #region Inscrito

        [SMCFilter]
        [SMCSize(SMCSize.Grid2_24)]
        [SMCMinValue(1)]
        public long? SeqInscrito { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(100)]
        public string NomeInscrito { get; set; }

        [SMCCpf]
        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        public string Cpf { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxLength(30)]
        public string NumeroPassaporte { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMinValue(1)]
        public long? SeqInscricao { get; set; }

        #endregion



    }
}