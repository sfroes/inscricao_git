using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelTipoFormularioListaViewModel : SMCViewModelBase, ISMCMappable
    {  
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqUnidadeResponsavel { get; set; }

        public string DescricaoTipoFormulario { get; set; }

        public bool Ativo { get; set; }
    }
}