using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelTipoFormularioViewModel : SMCViewModelBase, ISMCMappable
    {

        public UnidadeResponsavelTipoFormularioViewModel()
        {
            Ativo = true;
        }

        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqUnidadeResponsavel { get; set; }

        [SMCRequired]
        [SMCReadOnly(SMCViewMode.Edit)]
        [SMCSize(SMCSize.Grid18_24)]
        [SMCSelect("TiposFormulario")]
        public long? SeqTipoFormulario { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRadioButtonList]
        [SMCMapForceFromTo]
        public bool? Ativo { get; set; }
        
        public List<SMCDatasourceItem> TiposFormulario { get; set; }

        [SMCHidden]
        public int CodigoUnidade { get; set; }
    }
}