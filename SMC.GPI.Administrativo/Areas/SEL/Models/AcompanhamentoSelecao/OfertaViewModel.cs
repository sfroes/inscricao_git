using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class OfertaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        [SMCDependency(nameof(SeqOferta), "BloquearJustificativaAlteracaoOferta", "AcompanhamentoSelecao", false, includedProperties: new string[] { nameof(SeqOfertaOriginal) })]
        public bool BloquearJustificativa
        {
            get
            {
                return SeqOferta?.Seq == SeqOfertaOriginal;
            }
        }

        [SMCHidden]
        public long? SeqInscricaoOferta { get; set; }

        [SMCHidden]
        public long? SeqInscricao { get; set; }

        [SMCHidden]
        public long? SeqOfertaOriginal { get; set; }

        [SMCHidden]
        public long? SeqGrupoOferta { get; set; }

        [SMCHidden]
        public long? SeqItemHierarquiaOferta { get; set; }

        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCRequired]
        [SMCDependency(nameof(SeqGrupoOferta))]
        public GPILookupViewModel SeqOferta { get; set; }

        /// <summary>
        /// Justificativa da alteração: justificativa da alteração da oferta.
        /// </summary>
        [SMCConditionalRequired(nameof(BloquearJustificativa), SMCConditionalOperation.Equals, false)]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCMaxLength(500)]
        [SMCMultiline]
        [SMCConditionalReadonly(nameof(BloquearJustificativa), SMCConditionalOperation.Equals, true)]
        public string JustificativaAlteracaoOferta { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }
    }
}