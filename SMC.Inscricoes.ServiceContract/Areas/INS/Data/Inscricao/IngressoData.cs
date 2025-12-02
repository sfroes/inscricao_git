using SMC.DadosMestres.Common.Areas.GED.Enums;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.Common.Areas.SHA.Enums;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class IngressoData : ISMCMappable
    {
        public long SeqProcesso { get; set; }
        public long SeqInscricao { get; set; }
        public string TokenResource { get; set; }
        public string DescricaoProcesso { get; set; }
        public string TokenSituacaoAtual { get; set; }
        public string TituloInscricoes { get; set; }
        public string NomeInscrito { get; set; }
        public Guid UidProcesso { get; set; }
        public long SeqArquivoComprovante { get; set; }
        public List<IngressoOfertaData> Ofertas { get; set; }

        public bool? ExibirPeriodoAtividadeOferta { get; set; }

        public TimeSpan? QuantidadeHorasAberturaCheckin { get; set; }        

        public string CssUrl { get; set; }

        public string CssFisico { get; set; }
    }
}
