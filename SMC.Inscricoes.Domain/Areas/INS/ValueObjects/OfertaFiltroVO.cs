using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class OfertaFiltroVO : ISMCMappable
    {
        public long? SeqInscricaoOferta { get; set; }

        public long? SeqOferta { get; set; }

        public long? SeqInscricao { get; set; }
    }
}