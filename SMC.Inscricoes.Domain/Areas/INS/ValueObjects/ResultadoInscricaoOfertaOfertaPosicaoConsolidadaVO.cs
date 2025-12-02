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
    public class ResultadoInscricaoOfertaOfertaPosicaoConsolidadaVO : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqOferta { get; set; }
        public long? SeqGrupoOferta { get; set; }
        public string DescricaoOferta { get; set; }
        public string DescricaoGrupoOferta { get; set; }

        public string HierarquiaCompleta { get; set; }
    }
}
