using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.GPI.Inscricao.Areas.INS.Controllers;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaUploadDocumentosViewModel : PaginaViewModel
    {
        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_UPLOAD_DOCUMENTOS;
            }
        }

        public PaginaUploadDocumentosViewModel()
        {
            DocumentosOpcionais = new List<InscricaoDocumentoOpcionalViewModel>();
            DocumentosObrigatorios = new List<InscricaoDocumentoObrigatorioViewModel>();
            DocumentosGrupo = new List<InscricaoDocumentoGrupoViewModel>();
            DocumentosAdicionais = new SMCMasterDetailList<InscricaoDocumentoAdicionalViewModel>();
        }

        public List<InscricaoDocumentoOpcionalViewModel> DocumentosOpcionais { get; set; }

        public List<InscricaoDocumentoObrigatorioViewModel> DocumentosObrigatorios { get; set; }

        public List<InscricaoDocumentoGrupoViewModel> DocumentosGrupo { get; set; }

        public List<SMCDatasourceItem> DocumentosOpcionaisUpload { get; set; }

        [SMCDetail(SMCDetailType.Tabular)]
        public SMCMasterDetailList<InscricaoDocumentoAdicionalViewModel> DocumentosAdicionais { get; set; }

        public List<SMCDatasourceItem> DocumentosAdicionaisUpload { get; set; }
               
        public string DescricaoTermoEntregaDocumentacao { get; set; }

        public bool ExibeTermoPrincipalResponsabilidadeEntrega { get; set; }

        [SMCHidden]
        public bool ExibirMensagemInformativaConversaoPDF { get; set; }
    }
}