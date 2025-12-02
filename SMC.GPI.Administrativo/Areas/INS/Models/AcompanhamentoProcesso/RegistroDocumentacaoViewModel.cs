using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class RegistroDocumentacaoViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCKey]
        [SMCReadOnly()]
        [SMCSize(SMCSize.Grid2_24)]
        public long SeqInscricao { get; set; }

        public long SeqProcesso { get; set; }

        public string DescricaoGrupoOferta { get; set; }

        public List<OfertaInscricaoViewModel> DescricaoOfertas { get; set; }

        public string NomeInscrito { get; set; }

        public string DescricaoEtapaAtual { get; set; }

        /// <summary>
        /// Indica se a inscrição possui uma situação Finalizada no seu historico
        /// </summary>
        public bool Finalizada { get; set; }

        #region Documentacao Entregue

        public SumarioDocumentosEntreguesViewModel DocumentosEntregues { get; set; }

        #endregion

        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }


        public string SituacaoInscrito { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }

        [SMCHidden]
        public string Origem { get; set; }
    }
}