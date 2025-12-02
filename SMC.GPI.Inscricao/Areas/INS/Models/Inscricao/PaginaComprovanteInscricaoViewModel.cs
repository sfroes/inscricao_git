using SMC.Formularios.UI.Mvc.Model;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaComprovanteInscricaoViewModel : PaginaViewModel
    {
        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_COMPROVANTE_INSCRICAO;
            }
        }

        public PaginaComprovanteInscricaoViewModel()
        {
            Ofertas = new List<InscricaoOfertaListaViewModel>();
            CodigosAutorizacao = new List<InscricaoCodigoAutorizacaoViewModel>();
            Documentos = new SMCPagerModel<InscricaoDocumentoListaViewModel>();
            Formularios = new List<InscricaoDadoFormularioListaViewModel>();
        }

        public DadosInscritoViewModel DadosInscrito { get; set; }

        [SMCMapForceFromTo]
        public List<InscricaoOfertaListaViewModel> Ofertas { get; set; }

        public List<InscricaoCodigoAutorizacaoViewModel> CodigosAutorizacao { get; set; }

        public SMCPagerModel<InscricaoDocumentoListaViewModel> Documentos { get; set; }

        public List<InscricaoDadoFormularioListaViewModel> Formularios { get; set; }

        public short? NumeroOpcoesDesejadas { get; set; }
        public Guid? UidInscricaoOferta { get; set; }
        public string Qrcode { get; set; }


        #region Campos Exibe

        public string TextoInstrucoes
        {
            get
            {
                if (_textoInstrucoes == null && this.Secoes.Any(x => x.Token.Equals(TOKENS.SECAO_INSTRUCOES)))
                {
                    var protoloco = (ITemplateSecaoPaginaTexto)this.Secoes.First(x => x.Token.Equals(TOKENS.SECAO_INSTRUCOES));
                    _textoInstrucoes = HttpUtility.HtmlDecode(protoloco.Texto);
                }
                return _textoInstrucoes;
            }
        }
        private string _textoInstrucoes;

        public string TextoControleSecretaria
        {
            get
            {
                if (_textoControleSecretaria == null && this.Secoes.Any(x => x.Token.Equals(TOKENS.SECAO_CONTROLE_SECRETARIA)))
                {
                    var protoloco = (ITemplateSecaoPaginaTexto)this.Secoes.First(x => x.Token.Equals(TOKENS.SECAO_CONTROLE_SECRETARIA));
                    _textoControleSecretaria = HttpUtility.HtmlDecode(protoloco.Texto);
                }
                return _textoControleSecretaria;
            }
        }
        private string _textoControleSecretaria;

        public string TextoFoto
        {
            get
            {
                if (_textoFoto == null && this.Secoes.Any(x => x.Token.Equals(TOKENS.SECAO_FOTO)))
                {
                    var protoloco = (ITemplateSecaoPaginaTexto)this.Secoes.First(x => x.Token.Equals(TOKENS.SECAO_FOTO));
                    _textoFoto = HttpUtility.HtmlDecode(protoloco.Texto);
                }
                return _textoFoto;
            }
        }
        private string _textoFoto;

        public bool ExibeCodigosAutorizacao
        {
            get
            {
                return (this.FluxoPaginas.Any(f => f.Token.Equals(TOKENS.PAGINA_CODIGO_AUTORIZACAO) && (f as FluxoPaginaViewModel).ExibeComprovanteInscricao));
            }
        }

        public bool ExibeDocumentacao
        {
            get
            {
                return (this.FluxoPaginas.Any(f => f.Token.Equals(TOKENS.PAGINA_UPLOAD_DOCUMENTOS) && (f as FluxoPaginaViewModel).ExibeComprovanteInscricao));
            }
        }

        public bool ExibeComprovante
        {
            get
            {
                if (this.Secoes.Any(x => x.Token.Equals(TOKENS.SECAO_PROTOCOLO_ENTREGA)))
                {
                    var protoloco = (ITemplateSecaoPaginaTexto)this.Secoes.First(x => x.Token.Equals(TOKENS.SECAO_PROTOCOLO_ENTREGA));
                    return !string.IsNullOrEmpty(protoloco.Texto);
                }
                else
                    return false;
            }
        }

        public bool ExibeRodape
        {
            get
            {
                if (this.Secoes.Any(x => x.Token.Equals(TOKENS.SECAO_RODAPE)))
                {
                    var protoloco = (ITemplateSecaoPaginaTexto)this.Secoes.First(x => x.Token.Equals(TOKENS.SECAO_RODAPE));
                    return !string.IsNullOrEmpty(protoloco.Texto);
                }
                else
                    return false;
            }
        }

        public string DescricaoTermoEntregaDocumentacao { get; set; }

        public bool ExibeTermoPrincipalResponsabilidadeEntrega { get; set; }

        public bool ExibeDadosPessoais { get; set; }

        #endregion Campos Exibe

    }
}