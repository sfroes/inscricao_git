using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class EnderecoEletronicoPessoaIntegracaoData : ISMCMappable
    {
        [DataMember]
        public TipoEnderecoEletronico TipoEnderecoEletronico { get; set; }

        [DataMember]
        public string Descricao { get; set; }
    }
}
