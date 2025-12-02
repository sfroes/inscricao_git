using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TituloInscricaoData : ISMCMappable
    {
        [DataMember]
        public long SeqInscricaoBoletoTitulo { get; set; }

        [DataMember]
        public int SeqTitulo { get; set; }

        [DataMember]
        public TipoBoleto TipoBoleto { get; set; }

        [DataMember]
        public string Valor { get; set; }

        [DataMember]
        public List<TaxaTituloInscricaoData> Taxas { get; set; }

        [DataMember]
        public DateTime? DataCancelamento { get; set; }

        [DataMember]
        public DateTime DataVencimento { get; set; }

        [DataMember]
        public DateTime DataGeracao { get; set; }

        [DataMember]
        public DateTime? DataPagamento { get; set; }
    }
}
