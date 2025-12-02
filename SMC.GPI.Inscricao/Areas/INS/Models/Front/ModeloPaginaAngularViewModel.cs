using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class ModeloPaginaAngularViewModel : SMCViewModelBase, ISMCMappable
    {
        public object AlertaOferta { get; set; }
        public bool AptoBolsa { get; set; }
        public bool BolsaExAluno { get; set; }
        public string ChaveTextoBotaoAnterior { get; set; }
        public string ChaveTextoBotaoProximo { get; set; }
        public bool CobrancaPorOferta { get; set; }
        public object DescricaoGrupoOferta { get; set; }
        public List<string> DescricaoOfertas { get; set; }
        public string DescricaoProcesso { get; set; }
        public string DescricaoSituacaoAtual { get; set; }
        public bool ExibeTermoOrientacaoPDF { get; set; }
        public bool ExigeJustificativaOferta { get; set; }
        public List<FluxoPaginaMenuViewModel> FluxoPaginas { get; set; }
        public bool GestaoEventos { get; set; }
        public bool HabilitaCheckin { get; set; }
        public string Idioma { get; set; }
        public object ImagemCabecalho { get; set; }
        public bool InscricaoIniciada { get; set; }
        public object LabelCodigoAutorizacao { get; set; }
        public string LabelGrupoOferta { get; set; }
        public string LabelOferta { get; set; }
        public object NavigationGroup { get; set; }
        public object NumeroMaximoConvocacaoPorInscricao { get; set; }
        public object NumeroMaximoOfertaPorInscricao { get; set; }
        public object NumeroOpcoesDesejadas { get; set; }
        public int NumeroPaginaAtual { get; set; }
        public int Ofertas { get; set; }
        public int OpcoesParaConvocacao { get; set; }
        public int Ordem { get; set; }
        public object OrientacaoAceiteConversaoArquivosPDF { get; set; }
        public bool PossuiBoletoPago { get; set; }
        public int Secoes { get; set; }
        public int SeqConfiguracaoEtapa { get; set; }
        public string SeqConfiguracaoEtapaEncrypted { get; set; }
        public int SeqConfiguracaoEtapaPagina { get; set; }
        public int SeqConfiguracaoEtapaPaginaAnterior { get; set; }
        public string SeqConfiguracaoEtapaPaginaEncrypted { get; set; }
        public int SeqConfiguracaoEtapaPaginaProxima { get; set; }
        public string SeqConfiguracaoEtapaPaginaProximaEncrypted { get; set; }
        public int SeqGrupoOferta { get; set; }
        public string SeqGrupoOfertaEncrypted { get; set; }
        public int SeqInscricao { get; set; }
        public string SeqInscricaoEncrypted { get; set; }
        public int SeqProcesso { get; set; }
        public int Taxas { get; set; }
        public object TermoAceiteConversaoArquivosPDF { get; set; }
        public string Titulo { get; set; }
        public bool TituloPago { get; set; }
        public string Token { get; set; }
        public string TokenPaginaAnterior { get; set; }
        public string TokenProximaPagina { get; set; }
        public string TokenProximaPaginaEncrypted { get; set; }
        public string TokenResource { get; set; }
        public string TokenSituacaoAtual { get; set; }
        public int TotalGeral { get; set; }
        public Guid UidProcesso { get; set; }
        public string UrlCss { get; set; }

        // Propriedades privadas (equivalentes às privadas no TypeScript)
        private int _seqConfiguracaoEtapaPaginaProxima = 0;
        private string _tokenProximaPagina = string.Empty;
    }
}