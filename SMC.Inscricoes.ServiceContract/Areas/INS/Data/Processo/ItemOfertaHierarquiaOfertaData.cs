using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ItemOfertaHierarquiaOfertaData : ISMCMappable
    {
        [DataMember]
        public long SeqHierarquiaOfertaOrigem { get; set; }
        
        [DataMember]
        public long? SeqHierarquiaOfertaGPI { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public string TokenTipoItemHierarquiaOferta { get; set; }
    }
}