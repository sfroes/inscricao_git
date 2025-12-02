using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Service.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ClienteData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Sigla { get; set; }

        [DataMember]
        public string NomeFantasia { get; set; }

        [DataMember]
        public string RazaoSocial { get; set; }

        [DataMember]
        public string Cnpj { get; set; }

        [DataMember]
        public List<EnderecoData> Enderecos { get; set; }

        [DataMember]
        [SMCMapForceFromTo]
        public List<TelefoneData> Telefones { get; set; }

        [DataMember]
        public List<EnderecoEletronicoData> EnderecosEletronicos { get; set; }
    }
}
