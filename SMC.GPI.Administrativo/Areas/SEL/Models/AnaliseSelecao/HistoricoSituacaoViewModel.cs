using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class HistoricoSituacaoViewModel : SMCViewModelBase
    {
        #region Cabeçalho
        public long SeqProcesso { get; set; }

        public string TipoProcesso { get; set; }

        public string Descricao { get; set; }

        public long SeqInscricao { get; set; }

        public string Candidato { get; set; }

        public string GrupoOferta { get; set; }

        public string Opcao { get; set; }

        public string Oferta { get; set; }
        #endregion

        #region Datasources
        public List<SMCDatasourceItem> Motivos { get; set; }
        #endregion

        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqInscricaoOferta { get; set; }

        [SMCHidden]
        public long SeqSituacao { get; set; }

        [SMCReadOnly]
        [SMCSize(SMCSize.Grid5_24)]
        public string Situacao { get; set; }

        [SMCReadOnly]
        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        [SMCSize(SMCSize.Grid4_24)]
        public DateTime Data { get; set; }

        [SMCReadOnly]
        [SMCSize(SMCSize.Grid7_24)]
        public string Responsavel { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect("Motivos", NameDescriptionField = "Motivo")]
        public long? SeqMotivo { get; set; }

        public string Motivo { get; set; }

        [SMCHidden]
        [SMCDependency(nameof(SeqMotivo), "MotivoRequerJustificativa", "AnaliseSelecao", true)]
        public bool RequerJustificativa { get; set; }

        [SMCMultiline]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCConditionalRequired(nameof(RequerJustificativa), SMCConditionalOperation.Equals, true)]
        public string Justificativa { get; set; }

       [SMCHidden]
        public string BackUrl { get; set; }
    }
}