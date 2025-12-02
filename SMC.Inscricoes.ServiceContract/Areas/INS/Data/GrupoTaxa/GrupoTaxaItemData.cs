using SMC.DadosMestres.Common.Constants;
using SMC.Framework.Mapper;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class GrupoTaxaItemData: ISMCMappable 
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqGrupoTaxa { get; set; }

        [DataMember]
        public long SeqTaxa { get; set; }

        [DataMember]
        public TaxaData Taxa { get; set; }

    }
}
