using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class IngressoVO : ISMCMappable
    {
        public long SeqProcesso { get; set; }
        public long SeqInscricao { get; set; }
        public string DescricaoProcesso { get; set; }
        public string TokenResource { get; set; }
        public string TituloInscricoes { get; set; }
        public string TokenSituacaoAtual { get; set; }
        public Guid UidProcesso { get; set; }
        public string NomeInscrito { get; set; }
        public long? SeqArquivoComprovante { get; set; }
        public List<IngressoOfertaVO> Ofertas { get; set; }

        public bool? ExibirPeriodoAtividadeOferta { get; set; }

        public TimeSpan? QuantidadeHorasAberturaCheckin { get; set; }

        public string TokenTipoProcesso { get; set; }

        public string CssUrl { get; set; }

        public string CssFisico { get; set; }
    }
}
