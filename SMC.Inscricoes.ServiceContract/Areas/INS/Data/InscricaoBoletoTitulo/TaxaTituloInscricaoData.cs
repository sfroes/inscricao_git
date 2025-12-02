using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TaxaTituloInscricaoData : ISMCMappable
    {
        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public int NumeroItens { get; set; }
    }
}
