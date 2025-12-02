using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using SMC.Framework.DataAnnotations;
using SMC.GPI.Inscricao.Views.Home.App_LocalResources;
using Castle.Core.Internal;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaInstrucoesFinaisViewModel : PaginaViewModel
    {
        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_INSTRUCOES_FINAIS;
            }
        }

        public PaginaInstrucoesFinaisViewModel()
        { }

        public bool ImprimeBoleto
        {
            get
            {
                return string.IsNullOrWhiteSpace(MensagemImpressaoBoleto);
            }
        }

        public string MensagemImpressaoBoleto { get; set; }

        public bool EmiteComprovante 
        {
            get
            {
                return string.IsNullOrWhiteSpace(MensagemEmissaoComprovante);
            }
        }

        public string MensagemEmissaoComprovante { get; set; }

        public SMCEncryptedLong id 
        {
            get { return new SMCEncryptedLong(SeqInscricao); }
        } 

        public bool PossuiTaxas { get; set; }

        [SMCLink("DownloadDocumento", "Inscricao", SMCLinkTarget.NewWindow, "SeqArquivoComprovante")]
        public long? SeqArquivoComprovante { get; set; }

        public long SeqConfiguracaoEtapaPaginaComprovante 
        { 
            get
            {
                return this.FluxoPaginas.Where(f => f.Token.Equals(TOKENS.PAGINA_COMPROVANTE_INSCRICAO))
                                        .Select(f => f.SeqConfiguracaoEtapaPagina).SingleOrDefault();
            }
        }

        public bool HabilitarBotaoCheckin { get; set; }


        public string MensagemInformativaCheckin { get; set; }
      
        public bool DocumentacaoPendente { get; set; }

    }
}