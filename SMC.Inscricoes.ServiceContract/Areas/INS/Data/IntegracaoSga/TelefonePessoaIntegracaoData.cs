using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TelefonePessoaIntegracaoData : ISMCMappable
    {
        [DataMember]
        public TipoTelefone TipoTelefone { get; set; }

        [DataMember]
        public int CodigoPais { get; set; }

        [DataMember]
        public int CodigoArea { get; set; }

        [DataMember]
        public string Numero { get; set; }
    }
}
