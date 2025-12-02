using SMC.Formularios.UI.Mvc.Model;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Security;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public abstract class PaginaViewModel : TemplatePaginaViewModel, IIdioma
    {
        public PaginaViewModel() : base()
        {
            DescricaoOfertas = new List<string>();
        }

        [SMCHidden]
        public long SeqGrupoOferta { get; set; }

        [SMCHidden]
        public long SeqInscricao { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public SMCLanguage Idioma { get; set; }

        public string DescricaoGrupoOferta { get; set; }

        [SMCMapForceFromTo]
        public List<string> DescricaoOfertas { get; set; }

        //public string TokenSituacaoAtual { get; set; }

        public bool InscricaoIniciada
        {
            get
            {
                if (this.TokenSituacaoAtual != null)
                    return this.TokenSituacaoAtual.Equals(TOKENS.SITUACAO_INSCRICAO_INICIADA);
                return true;
            }
        }

        public string ImagemCabecalho { get; set; }

        public string LabelCodigoAutorizacao { get; set; }

        public string LabelGrupoOferta { get; set; }

        public string LabelOferta { get; set; }

        [SMCHidden]
        public string DescricaoSituacaoAtual { get; set; }

        [SMCHidden]
        public string SeqGrupoOfertaEncrypted
        {
            get
            {
                return SMCDESCrypto.EncryptNumberForURL(SeqGrupoOferta);
            }
            set
            {
                SeqGrupoOferta = SMCDESCrypto.DecryptNumberForURL(value);
            }
        }

        [SMCHidden]
        public string SeqInscricaoEncrypted
        {
            get
            {
                return SMCDESCrypto.EncryptNumberForURL(SeqInscricao);
            }
            set
            {
                SeqInscricao = SMCDESCrypto.DecryptNumberForURL(value);
            }
        }

        public string TermoAceiteConversaoArquivosPDF { get; set; }

        public string OrientacaoAceiteConversaoArquivosPDF { get; set; }

        public bool ExibeTermoOrientacaoPDF
        {
            get
            {
                return !string.IsNullOrEmpty(OrientacaoAceiteConversaoArquivosPDF) && !string.IsNullOrEmpty(TermoAceiteConversaoArquivosPDF);
            }
        }

        public string TokenResource { get; set; }

        public Guid UidProcesso { get; set; }

        public bool AptoBolsa { get; set; }
        public bool GestaoEventos { get; set; }

        public bool HabilitaCheckin { get; set; }
        public string UrlCss { get; set; }
        public string TokenPaginaAnteriorEncrypted
        {
            get
            {
                return SMCDESCrypto.EncryptForURL(TokenPaginaAnterior);
            }
        }
        public string SeqConfiguracaoEtapaPaginaAnteriorEncrypted
        {
            get
            {
                return SMCDESCrypto.EncryptNumberForURL(SeqConfiguracaoEtapaPaginaAnterior);
            }
        }

    }
}