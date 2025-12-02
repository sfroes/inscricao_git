using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.RES.Enums;
using SMC.Inscricoes.Service.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract]
    public class UnidadeResponsavelData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Sigla { get; set; }

        [DataMember]
        public TipoUnidadeResponsavel TipoUnidadeResponsavel { get; set; }

        [DataMember]
        public string NomeContato { get; set; }

        [DataMember]
        public string UsuarioAlteracao { get; set; }

        [DataMember]
        public int? CodigoUnidade { get; set; }

        [DataMember]
        public long? SeqUnidadeResponsavelNotificacao { get; set; }

        [DataMember]
        public long? SeqUnidadeResponsavelSgf { get; set; }

        [DataMember]
        public List<EnderecoData> Enderecos { get; set; }

        [DataMember]
        public List<EnderecoEletronicoData> EnderecosEletronicos { get; set; }

        [DataMember]
        [SMCMapForceFromTo]
        public List<TelefoneData> Telefones { get; set; }

        [DataMember]
        public List<UnidadeResponsavelCentroCustoData> CentrosCusto { get; set; }

        [DataMember]
        public string TokenSistemaOrigemGad { get; set; }

    }
}
