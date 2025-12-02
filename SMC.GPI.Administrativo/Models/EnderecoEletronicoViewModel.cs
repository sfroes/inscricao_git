using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.RES;

namespace SMC.GPI.Administrativo.Models
{
    public class EnderecoEletronicoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long? SeqEnderecoEletronico { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCRequired]
        [SMCSelect]
        public TipoEnderecoEletronico TipoEnderecoEletronico { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCRequired]
        [SMCMaxLength(100)]
        public string Descricao { get; set; }
    }
}