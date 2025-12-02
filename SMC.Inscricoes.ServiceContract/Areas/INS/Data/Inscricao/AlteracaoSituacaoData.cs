using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class AlteracaoSituacaoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]       
        public long SeqTipoProcessoSituacaoDestino { get; set; }

        [DataMember]
        public List<long> SeqInscricoes { get; set; }

        [DataMember]
        public long? SeqMotivoSGF { get; set; }

        [DataMember]
        public string Justificativa { get; set; }
    }
}
