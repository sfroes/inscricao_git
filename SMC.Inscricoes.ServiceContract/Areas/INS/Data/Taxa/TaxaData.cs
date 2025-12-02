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
    /// <summary>
    /// Dados do processo resumidos
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TaxaData : ISMCMappable
    {        
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public long SeqTipoTaxa { get; set; }

        [DataMember]
        public bool? SelecaoInscricao { get; set; }    
        
        [DataMember]   
        public bool? CobrarPorOferta { get; set; }

        [DataMember]
        public TipoCobranca TipoCobranca { get; set; }

        [DataMember]
        [SMCMapProperty("TipoTaxa.Descricao")]
        public string Descricao { get; set; }

        [DataMember]
        public string DescricaoComplementar { get; set; }
    }
}
