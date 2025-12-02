using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class NoArvoreTipoHierarquiaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        public long Seq { get; set; }

        public long SeqTipoHierarquiaOferta { get; set; }

        public long SeqPai { get; set; }

        [SMCDescription]
        public string Descricao { get; set; }

        public bool HabilitaCadastroOferta { get; set; }
    }
}