using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class GrupoTaxaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]        
        public string Descricao { get; set; }

        [DataMember]
        public short NumeroMinimoItens { get; set; }

        [DataMember]
        public short? NumeroMaximoItens { get; set; }

        [DataMember]
        public IList<GrupoTaxaItemData> Itens { get; set; }


        public List<SMCDatasourceItem> Taxas { get; set; }
    }
}
