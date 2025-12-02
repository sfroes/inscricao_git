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
    public class PosicaoConsolidadaGrupoOfertaVO : PosicaoConsolidadaVO
    {

        public PosicaoConsolidadaGrupoOfertaVO()
        {
            PosicoesConsolidadasOfertas = new List<PosicaoConsolidadaOfertaVO>();
        }

        public long SeqProcesso { get; set; }

        public int OfertasNaoSelecionadas { get; set; }

        public List<PosicaoConsolidadaOfertaVO> PosicoesConsolidadasOfertas { get; set; }
    }
}
