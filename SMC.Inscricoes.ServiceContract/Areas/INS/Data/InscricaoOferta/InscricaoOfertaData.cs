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
    public class InscricaoOfertaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long SeqOferta { get; set; }

        [DataMember]
        public short NumeroOpcao { get; set; }

        [DataMember]
        public string DescricaoCompleta { get; set; }

        [DataMember]
        public string JustificativaInscricao { get; set; }

        [DataMember]
        public Guid? UidInscricaoOferta { get; set; }
        [DataMember]
        public Guid UidProcesso { get; set; }
        [DataMember]
        public long SeqInscrito { get; set; }
        public string NomeInscrito { get; set; }
        public long? SeqUsuario { get; set; }

        public bool Ativo { get; set; }

        public bool OfertaImpedida { get; set; }

    }
}
