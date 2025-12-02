using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Service.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TipoDocumentoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        [SMCMapProperty("Descricao")]
        public string DescricaoTipoDocumento { get; set; }
        
        [DataMember]
        public bool PermiteVariosArquivos { get; set; }

        [DataMember]
        public TipoEmissao? TipoEmissao { get; set; }

    }
}
