using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.GPI.Inscricao.Models
{
    public class EnderecoEletronicoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; } 

        // Deixar com o tipo de dados short, pois as opções do select são filtradas. Se alterar para o Enum, o filtro não funciona.
		[SMCSelect("TiposEnderecoEletronico")]
		[SMCSize(SMCSize.Grid6_24)]
        //[SMCCssClass("col-30-mob col-40-xs col-40-ms", true)]
        [SMCRequired()]
        public short TipoEnderecoEletronico { get; set; } 

        [SMCSize(SMCSize.Grid8_24)]
        //[SMCCssClass("col-60-mob col-50-xs col-50-ms", true)]
        [SMCMaxLength(100)]
        [SMCRequired()]
        public string Descricao { get; set; }
    }
}