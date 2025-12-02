using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS.Enums;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TipoDocumentoListaViewModel : SMCViewModelBase
    {        
        [SMCHidden]
        public long Seq { get; set; }
        
        public string DescricaoTipoDocumento { get; set; }

        public TipoEmissao? TipoEmissao { get; set; }

        //public bool PermiteVariosArquivos { get; set; }

    }
}
