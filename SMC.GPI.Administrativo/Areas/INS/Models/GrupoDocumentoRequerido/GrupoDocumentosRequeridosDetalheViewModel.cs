using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
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
    public class GrupoDocumentosRequeridosDetalheViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCSelect("DocumentosRequeridos")]
        [SMCRequired]
        [SMCSize(SMCSize.Grid10_24)]
        public long? SeqDocumentoRequerido { get; set; }

        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqGrupoDocumentoRequerido { get; set; }
        
    }
}