using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricaoTaxaOfertaData : ISMCMappable
    {
        [DataMember]
        public long? SeqInscricaoBoleto { get; set; }

        [DataMember]
        public long SeqTaxa { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public string DescricaoComplementar { get; set; }

        [DataMember]
        public short? NumeroItens { get; set; }

        [DataMember]
        public short? NumeroMinimo { get; set; }

        [DataMember]
        public short? NumeroMaximo { get; set; }

        [DataMember]
        public int SeqEventoTaxa { get; set; }

        [DataMember]
        public decimal ValorEventoTaxa { get; set; }

        [DataMember]
        public decimal? ValorTitulo { get; set; }

        [DataMember]
        public bool? CobrarPorOferta { get; set; }

        [DataMember]
        public  TipoCobranca TipoCobranca { get; set; }

        [DataMember]
        public long SeqOferta { get; set; }
        [DataMember]
        public bool PossuiGrupoTaxas { get; set; }

        public GrupoTaxaData GrupoTaxa { get; set; }
    }
}