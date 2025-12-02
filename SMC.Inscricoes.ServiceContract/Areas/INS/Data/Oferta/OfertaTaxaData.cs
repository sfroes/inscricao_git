using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]    
    public class OfertaTaxaData : ISMCMappable
    {
        public long SeqOferta { get; set; }

        public string Descricao { get; set; }

        public string Periodo { get; set; }

        public List<TaxaPeriodoOfertaData> Taxas { get; set; }
    }
}
