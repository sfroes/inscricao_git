using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ProcessoListaViewModel : SMCViewModelBase, ISMCMappable
    {
        // Aguardando ajustes no FW que permitam reordenar, quando existir uma ordenação na specification.
        //[SMCSortable]
        public long Seq { get; set; }

        //[SMCSortable]
        public string Descricao { get; set; }

        //[SMCSortable]
        public string DescricaoTipoProcesso { get; set; }

        //[SMCSortable]
        public int AnoReferencia { get; set; }

        //[SMCSortable]
        public int SemestreReferencia { get; set; }
    }
}