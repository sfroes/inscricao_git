using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TaxaPeriodoOfertaData : ISMCMappable
    {
        public string TipoTaxa { get; set; }        

        public string Periodo { get; set; }

        public short? NumeroMinimo { get; set; }

        public short? NumeroMaximo { get; set; }

        public long SeqEventoTaxa { get; set; }

        public string EventoTaxa { get; set; }

        public decimal? Valor { get; set; }

        public long SeqParametroCrei { get; set; }

        public string VencimentoTitulo { get; set; }
    }
}
