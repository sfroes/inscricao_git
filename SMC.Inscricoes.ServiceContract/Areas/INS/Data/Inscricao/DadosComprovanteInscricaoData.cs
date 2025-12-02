using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class DadosComprovanteInscricaoData : ISMCMappable
    {
        [DataMember]
        public long SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        public long SeqInscrito { get; set; }

        [DataMember]
        public SMCLanguage Idioma { get; set; }

        [DataMember]
        public long SeqGrupoOferta { get; set; }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public byte[] DadosComprovante { get; set; }
    }
}
