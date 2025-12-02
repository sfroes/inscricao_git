using SMC.Framework;
using SMC.Framework.Mapper;
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
    public class ResultadoHistoricoSituacaoPosicaoConsolidadaVO : ISMCMappable
    {
        public long? SeqMotivoSituacaoSGF { get; set; }
        public bool Atual { get; set; }
        public bool AtualEtapa { get; set; }
        public string Token { get; set; }
    }
}
