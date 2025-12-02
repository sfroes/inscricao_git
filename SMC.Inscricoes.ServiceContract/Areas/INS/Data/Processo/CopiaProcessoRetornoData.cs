using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class CopiaProcessoRetornoData : ISMCMappable
    {
        [DataMember]
        public long SeqProcessoOrigem { get; set; }

        [DataMember]
        public Dictionary<long, long?> ProcessosGpi { get; set; }

        [DataMember]
        public Dictionary<long, long?> ItensOfertasHierarquiasOfertas { get; set; }

    }
}