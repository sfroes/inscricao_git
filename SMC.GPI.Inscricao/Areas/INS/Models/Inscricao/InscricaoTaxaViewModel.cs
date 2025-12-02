using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoTaxaViewModel : SMCViewModelBase, ISMCMappable
    {
        public InscricaoTaxaViewModel()
        {
            NumeroItens = 0;
        }

        [SMCHidden]
        public long? SeqInscricaoBoleto { get; set; }

        [SMCHidden]
        public long SeqTaxa { get; set; }

        [SMCHidden]
        public string Descricao { get; set; }

        [SMCHidden]
        public string DescricaoComplementar { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        public string DescricaoDisplay
        {
            get { return Descricao; }
        }

        [SMCMapForceFromTo]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxValue(99)]
        [SMCMinValue(0)]
        public short? NumeroItens { get; set; }

        [SMCHidden]
        public short? NumeroMinimo { get; set; }

        [SMCHidden]
        public short? NumeroMaximo { get; set; }

        [SMCCurrency]
        [SMCDecimalDigits(2)]
        [SMCReadOnly]
        [SMCSize(SMCSize.Grid4_24)]
        public decimal ValorItem => ValorEventoTaxa;

        [SMCDecimalDigits(2)]
        public decimal ValorEventoTaxa { get; set; }

        [SMCHidden]
        public decimal? ValorTitulo { get; set; }

        [SMCDecimalDigits(2)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMath("{0} * {1}", 2, SMCSize.Grid6_24, "NumeroItens", "ValorItem")]
        public decimal ValorTotalTaxa
        {
            get
            {
                return this.ValorItem * NumeroItens.GetValueOrDefault();
            }
        }

        public bool? CobrarPorOferta { get; set; }

        public bool TituloPago { get; set; }

        [SMCHidden]
        public  TipoCobranca TipoCobranca { get; set; }

        [SMCHidden]
        public long SeqOferta { get; set; }
        
        [SMCHidden]
        public bool PossuiGrupoTaxas { get; set; }

        [SMCHidden]
        public GrupoTaxaData GrupoTaxa { get; set; }
    }
}