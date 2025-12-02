using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common;
using System.Collections.Generic;
using System.Linq;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaConfirmarInscricaoViewModel : PaginaViewModel
    {
        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_CONFIRMACAO_INSCRICAO;
            }
        }

        public PaginaConfirmarInscricaoViewModel()
        {
            Ofertas = new List<InscricaoOfertaListaViewModel>();
            CodigosAutorizacao = new List<InscricaoCodigoAutorizacaoViewModel>();
            Documentos = new SMCPagerModel<InscricaoDocumentoListaViewModel>();
            Formularios = new List<InscricaoDadoFormularioListaViewModel>();
        }

        public string DescricaoProcessoVisivel => DescricaoProcesso;

        [SMCMapForceFromTo]
        public List<InscricaoOfertaListaViewModel> Ofertas { get; set; }

        [SMCMapForceFromTo]
        public SMCPagerModel<InscricaoTaxaViewModel> Taxas { get; set; }

        public List<InscricaoCodigoAutorizacaoViewModel> CodigosAutorizacao { get; set; }

        public SMCPagerModel<InscricaoDocumentoListaViewModel> Documentos { get; set; }

        public List<InscricaoDadoFormularioListaViewModel> Formularios { get; set; }

        public short? NumeroOpcoesDesejadas { get; set; }

        public short? NumeroMaximoOfertaPorInscricao { get; set; }

        public string OrientacaoAceiteConversaoArquivosPDF { get; set; }

        public string TermoAceiteConversaoArquivosPDF { get; set; }

        public bool AceiteConversaoPDF { get; set; }

        [SMCHidden]
        public bool? ExibeTermoConsentimentoLGPD { get; set; }

        public int Idade { get; set; }

        public bool ConsentimentoLGPD { get; set; }

        public string TermoLGPD { get; set; }

        #region Campos Exibe

        public bool ExibeCodigosAutorizacao
        {
            get
            {
                return (this.FluxoPaginas.Any(f => f.Token.Equals(TOKENS.PAGINA_CODIGO_AUTORIZACAO) && (f as FluxoPaginaViewModel).ExibeConfirmacaoInscricao));
            }
        }

        public bool ExibeDocumentacao
        {
            get
            {
                return (this.FluxoPaginas.Any(f => f.Token.Equals(TOKENS.PAGINA_UPLOAD_DOCUMENTOS) && (f as FluxoPaginaViewModel).ExibeConfirmacaoInscricao));
            }
        }

        public override string ChaveTextoBotaoProximo
        {
            get
            {
                return "Botao_Confirmar_Inscricao";
            }
        }

        public string DescricaoTermoEntregaDocumentacao { get; set; }

        public bool ExibeTermoPrincipalResponsabilidadeEntrega { get; set; }

        public bool ExibeTermoOrientacaoPDF
        {
            get {
                return !string.IsNullOrEmpty(OrientacaoAceiteConversaoArquivosPDF) && !string.IsNullOrEmpty(TermoAceiteConversaoArquivosPDF) && Documentos.Any(a => a.ConvertidoParaPDF);
            }
        }

        #endregion Campos Exibe

    }
}