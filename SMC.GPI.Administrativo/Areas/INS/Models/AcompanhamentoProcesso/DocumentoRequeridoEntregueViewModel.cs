using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class DocumentoRequeridoEntregueViewModel : SMCViewModelBase, ISMCMappable
    {

        public bool Opcional { get; set; }

        [SMCHidden]
        [SMCKey]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqInscricao { get; set; }

        [SMCHidden]
        public long SeqDocumentoRequerido { get { return Seq; } }

        [SMCSize(SMCSize.Grid22_24)]
        public string DescricaoTipoDocumento { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        //[SMCSelect(nameof(SolicitacoesEntregaDocumento))]
        [SMCSelect(AjaxLoadAction = "BuscarSituacoesEntregaDocumento",
            AjaxLoadArea = "INS",
            AjaxLoadController = "AcompanhamentoProcesso",
            AjaxLoadProperties = new string[] { nameof(SeqDocumentoRequerido), nameof(SeqInscricao), nameof(SituacaoEntregaDocumentos) }, 
            IgnoredEnumItems = new object[] { SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel, SituacaoEntregaDocumento.AguardandoEntrega, SituacaoEntregaDocumento.AguardandoValidacao, SituacaoEntregaDocumento.Deferido, SituacaoEntregaDocumento.Indeferido, SituacaoEntregaDocumento.Pendente } )]

        public SituacaoEntregaDocumento SituacaoEntregaDocumentos { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect]
        [SMCRequired]
        public VersaoDocumento VersaoDocumento { get; set; }

        public VersaoDocumento VersaoDocumentoExigido { get; set; }

        [SMCHidden]
        public bool PermiteVarios { get; set; }

        [SMCHidden]
        public long SeqTipoDocumento { get; set; }

        [SMCHidden]
        public Sexo Sexo { get; set; }

        [SMCDependency(nameof(Seq))]
        [SMCDependency(nameof(SeqInscricao))]
        [SMCDetail(SMCDetailType.Modal)]
        public SMCMasterDetailList<InscricaoDocumentoViewModel> InscricaoDocumentos { get; set; }

        [SMCHidden]
        public bool PermiteEntregaPosterior { get; set; }

        [SMCHidden]
        public bool Obrigatorio { get; set; }

        [SMCHidden]
        public bool UploadObrigatorio { get; set; }

        [SMCHidden]
        public bool PermiteUploadArquivo { get; set; }

        [SMCHidden]
        public bool ValidacaoOutroSetor { get; set; }


    }
}