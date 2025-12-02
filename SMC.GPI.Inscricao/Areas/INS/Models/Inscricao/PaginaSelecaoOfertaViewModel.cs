using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.Linq;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaSelecaoOfertaViewModel : PaginaViewModel
    {
        #region DataSources
        public List<SMCDatasourceItem> OpcoesParaConvocacao
        {
            get
            {
                var opcoes = new List<SMCDatasourceItem>();
                for (int i = 1; i <= NumeroMaximoConvocacaoPorInscricao; i++)
                {
                    opcoes.Add(new SMCDatasourceItem(i, i.ToString()));
                }
                return opcoes;
            }
        }
        #endregion

        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_SELECAO_OFERTA;
            }
        }

        [SMCHidden]
        public short? NumeroMaximoOfertaPorInscricao { get; set; }

        [SMCHidden]
        public bool ExigeJustificativaOferta { get; set; }

        public short? NumeroMaximoConvocacaoPorInscricao { get; set; }

        [SMCRequired]
        [SMCSelect(nameof(OpcoesParaConvocacao), AutoSelectSingleItem = true)]
        [SMCSize(SMCSize.Grid24_24)]
        public short? NumeroOpcoesDesejadas { get; set; }

        public SMCMasterDetailList<InscricaoOfertaViewModel> Ofertas { get; set; }

        public List<InscricaoTaxaViewModel> Taxas { get; set; }

        [SMCIgnoreProp]
        public bool TituloPago => Taxas?.SMCAny(a => a.ValorTitulo.HasValue) ?? false;

        [SMCCurrency]
        [SMCMath(SMCMathOperations.Sum, 2, SMCSize.Grid24_24, "ValorTotalTaxa", FindByDataName = true)]
        public decimal TotalGeral
        {
            get
            {
                return this.Taxas == null ? 0 : this.Taxas.Sum(x => x.NumeroItens.GetValueOrDefault() * x.ValorEventoTaxa);
            }
        }

        [SMCHidden]
        public bool PossuiBoletoPago { get; set; }

        [SMCHidden]
        public bool CobrancaPorOferta { get; set; }

        [SMCHidden]
        public bool TipoCobrancaPorQuantidadeOferta { get; set; }

        [SMCHidden]
        public bool BolsaExAluno { get; set; }

        #region Apoio

        public string AlertaOferta
        {
            get
            {
                if (this.FluxoPaginas != null && this.FluxoPaginas.Any(f => f.Token.Equals(this.Token)))
                {
                    return (this.FluxoPaginas.Where(f => f.Token.Equals(this.Token)).First() as FluxoPaginaViewModel)?.Alerta;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public bool ExibirArvoreFechada { get; set; }

        [SMCHidden]
        public List<GrupoTaxaData> GruposTaxa { get; set; }

        [SMCHidden]
        public bool ProcessoPossuiTaxa { get; set; }

        [SMCHidden]
        public bool PermiteAlterarBoleto { get; set; }

        #endregion 
    }
}