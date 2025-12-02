using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class ClienteListaViewModel : SMCViewModelBase
    {

        [SMCSortable]
        public long Seq { get; set; }

        [SMCSortable]
        public string Nome { get; set; }
        
        public string Sigla { get; set; }

    }
}