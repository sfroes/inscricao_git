using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.INS.Models;
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
    public class ResultadoPosicaoConsolidadaVO : ISMCMappable
    {
        public long Seq { get; set; }
        public string Descricao { get; set; }
     

        public List<PosicaoConsolidadaGrupoOfertaVo> GruposOfertas { get; set; }


        public List<ResultadoInscricaoPosicaoConsolidadaVO> Inscricoes { get; set; }
    }

    public class PosicaoConsolidadaGrupoOfertaVo
    {
        public long SeqGrupoOferta { get; set; }

        public string NomeGrupo { get; set; }

        public List<OfertaVO> Ofertas { get; set; }

    }
}
