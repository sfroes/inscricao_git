using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.SEL.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    public class ResultadoInscricaoPosicaoConsolidadaVO : ISMCMappable
    {
        public long SeqInscricao { get; set; }
        public bool TituloPago { get; set; }
        public bool DocumentacaoEntregue { get; set; }
        public long SeqGrupoOferta { get; set; }
        public List<ResultadoHistoricoSituacaoPosicaoConsolidadaVO> HistoricosSituacao { get; set; }
        public List<ResultadoInscricaoOfertaOfertaPosicaoConsolidadaVO> Ofertas { get; set; }
    }
}
