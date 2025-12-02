using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Security;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Dynamic;
using SMC.Inscricoes.Common.Areas.SEL.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class HistoricoSituacaoItemViewModel : SMCViewModelBase
    {
        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqInscricaoOferta { get; set; } 
        
        public string Situacao { get; set; }
        
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime Data { get; set; }
        
        public string Responsavel { get; set; }
        
        public string Motivo { get; set; }

        public string Justificativa { get; set; }       
    }
}