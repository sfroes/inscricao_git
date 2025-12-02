using SMC.Framework.Mapper;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class GrupoTaxaListaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]        
        public string Descricao { get; set; }

        [DataMember]
        public short NumeroMinimoItens { get; set; }

        [DataMember]
        public short? NumeroMaximoItens { get; set; }
        
        [DataMember]
        public List<string> Itens { get; set; }

    }
}
