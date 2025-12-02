using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Service.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TipoDocumentoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public long? Seq { get; set; }

        public string Descricao { get; set; }

        public TipoEmissao? TipoEmissao { get; set; }

        public bool? PermiteVariosArquivos { get; set; }


    }
}
