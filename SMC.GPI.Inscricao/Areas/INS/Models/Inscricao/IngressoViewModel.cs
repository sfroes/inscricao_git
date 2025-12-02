using SMC.Formularios.UI.Mvc.Models;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Inscricao.Areas.INS.Views.Inscricao.App_LocalResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class IngressoViewModel : SMCViewModelBase
    {
        public long SeqProcesso { get; set; }
        public long SeqInscricao { get; set; }
        public string DescricaoProcesso { get; set; }
        public string TokenResource { get; set; }
        public string TituloInscricoes { get; set; }
        public string TokenSituacaoAtual { get; set; }
        public Guid UidProcesso { get; set; }
        public string NomeInscrito { get; set; }
        public List<IngressoOfertaViewModel> Ofertas { get; set; }
        public List<DadoCampoViewModel> DadosCampo { get; set; }
        public long SeqArquivoComprovante { get; set; }
        public bool? ExibirPeriodoAtividadeOferta { get; set; }

        public string MensagemCredenciamento
        {
            get
            {
                if (this.QuantidadeHorasAberturaCheckin.HasValue)
                {
                    TimeSpan timeSpan = this.QuantidadeHorasAberturaCheckin.Value;

                    string horas = timeSpan.Hours > 0
                        ? $"{timeSpan.Hours} hora{(timeSpan.Hours > 1 ? "s" : string.Empty)}"
                        : string.Empty;

                    string minutos = timeSpan.Minutes > 0
                        ? $"{timeSpan.Minutes} minuto{(timeSpan.Minutes > 1 ? "s" : string.Empty)}"
                        : string.Empty;

                    string valor = !string.IsNullOrEmpty(horas) && !string.IsNullOrEmpty(minutos)
                        ? $"{horas} e {minutos}"
                        : horas + minutos;

                    return !string.IsNullOrEmpty(valor) ? string.Format(UIResource.MSG_Ingressos, valor): string.Empty;
                }

                return string.Empty; // Ou alguma mensagem padrão se QuantidadeHorasAberturaCheckin não tiver valor
            }
        }


        public TimeSpan? QuantidadeHorasAberturaCheckin { get; set; }

        public string CssUrl { get; set; }

        public string CssFisico { get; set; }
    }
}