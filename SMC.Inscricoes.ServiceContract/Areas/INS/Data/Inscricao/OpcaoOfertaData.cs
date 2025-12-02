using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec uma opção de oferta de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class OpcaoOfertaData : ISMCMappable
    {
        [DataMember]
        public long SeqOferta { get; set; }
        [DataMember]
        public short NumeroOpcao { get; set; }

        [DataMember]
        public string DescricaoOferta { get; set; }

        [DataMember]
        public string DescricaoOfertaOriginal { get; set; }

        [DataMember]
        public string Justificativa { get; set; }
        [DataMember]
        public string SituacaoOferta { get; set; }
        [DataMember]
        public string NumeroOpcaoFormatado { get; set; }
        public long SeqProcesso { get; set; }
        public List<long> InscricaoOferta { get; set; }
        public long SeqInscricao { get; set; }
        public long SeqInscricaoOferta { get; set; }
        public long SeqInscrito { get; set; }
        public string NomeInscrito { get; set; }
    }
}