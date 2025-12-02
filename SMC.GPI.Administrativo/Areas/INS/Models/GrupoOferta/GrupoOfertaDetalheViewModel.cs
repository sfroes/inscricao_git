using Newtonsoft.Json;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class GrupoOfertaDetalheViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCSelect("GruposOfertaSelect")]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRequired]
        public long SeqGrupoOferta { get; set; }

        [SMCDescription]
        [SMCHidden]
        public string Descricao { get; set; }
       
    }
}