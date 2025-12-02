using SMC.Framework.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class HistoricoSituacaoViewModel : SMCViewModelBase
    {
        public HistoricoSituacaoFiltroViewModel FiltroCabecalho { get; set; }

        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqSituacao { get; set; }

        [SMCHidden]
        public long SeqInscricao { get; set; }

        [SMCSize(Framework.SMCSize.Grid8_24)]
        [SMCReadOnly]
        public string Situacao { get; set; }

        [SMCSize(Framework.SMCSize.Grid5_24)]
        [SMCReadOnly]
        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        public DateTime Data { get; set; }

        [SMCSize(Framework.SMCSize.Grid6_24)]
        [SMCReadOnly]
        public string Responsavel { get; set; }

        [SMCSize(Framework.SMCSize.Grid5_24)]
        [SMCSelect("Motivos")]
        public long? SeqMotivoSGF { get; set; }

        public List<SMCDatasourceItem> Motivos { get; set; }

        [SMCMultiline(Rows = 8)]        
        [SMCSize(Framework.SMCSize.Grid24_24)]
        public string Justificativa { get; set; }

        [SMCHidden]
        public string BackURL { get; set; }

    }
}