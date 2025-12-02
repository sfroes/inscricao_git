using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class PosicaoConsolidadaGrupoOfertaData : PosicaoConsolidadaData
    {

        public PosicaoConsolidadaGrupoOfertaData()
        {
            PosicoesConsolidadasOfertas = new List<PosicaoConsolidadaOfertaData>();
        }

        public long SeqProcesso { get; set; }

        [DataMember]
        public int OfertasNaoSelecionadas { get; set; }

        [DataMember]
        [SMCMapForceFromTo]
        public List<PosicaoConsolidadaOfertaData> PosicoesConsolidadasOfertas { get; set; }
    }
}
