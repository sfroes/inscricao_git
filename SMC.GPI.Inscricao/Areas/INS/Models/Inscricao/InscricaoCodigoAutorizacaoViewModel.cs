using System;
using System.Collections.Generic;
using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Framework.Model;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Localidades.UI.Mvc.Models;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoCodigoAutorizacaoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        public string Codigo { get; set; }
    }

}



