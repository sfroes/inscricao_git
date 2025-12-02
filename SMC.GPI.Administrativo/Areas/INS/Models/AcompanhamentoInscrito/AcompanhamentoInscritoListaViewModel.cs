using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AcompanhamentoInscritoListaViewModel : SMCViewModelBase, ISMCMappable
    {

        #region [Dados Inscrito]

        [SMCSize(SMCSize.Grid3_24)]
        public long SeqInscrito { get; set; }

        [SMCHidden]
        public long SeqSituacao { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        public string NomeInscrito { get; set; }

        [SMCCpf]
        [SMCSize(SMCSize.Grid4_24)]
        public string Cpf { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        public string NumeroPassaporte { get; set; }
        public string CpfouPassaporte { get; set; }
        #endregion

        #region [Dados Processo]

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long SeqTipoProcesso { get; set; }

        public string TipoProcesso { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        public string DescricaoProcesso { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        public string SemestreAno { get; set; }

        #endregion

        #region [Dados Inscrição]

        public int? SemestreReferencia { get; set; }

        public List<object> Inscricoes { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        public long SeqInscricao { get; set; }
 
        public long SeqInscricaoHistoricoSituacao { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        public string SituacaoInscricao { get; set; }

        [SMCHidden]
        public string BackUrl { get; set; }
        #endregion

        #region [Dados Oferta]

        public List<OpcaoOfertaViewModel> OpcoesOferta { get; set; }



        #endregion

    }
}