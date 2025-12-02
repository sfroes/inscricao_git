using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Inscricao.Areas.INS.Views.NovaEntregaDocumentacao.App_LocalResources;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class NovaEntregaDocumentacaoViewModel : SMCViewModelBase, ISMCMappable, IIdioma
    {

        [SMCHidden]
        public long SeqConfiguracaoEtapaPagina { get; set; }

        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCHidden]
        public long SeqGrupoOferta { get; set; }

        [SMCHidden]
        public long SeqInscricao { get; set; }

        [SMCHidden]
        public SMCLanguage Idioma { get; set; }

        [SMCHidden]
        public Guid UidProcesso { get; set; }

        [SMCDisplay]
        [SMCHideLabel]
        public string MensagemInformativa { get { return UIResource.MSG_Tamanho_Maximo_Arquivo; } }

        public List<NovaEntregaDocumentacaoDocumentoViewModel> Documentos { get; set; }
    }
}