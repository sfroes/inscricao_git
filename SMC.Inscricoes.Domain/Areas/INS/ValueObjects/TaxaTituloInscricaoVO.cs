using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class TaxaTituloInscricaoVO : ISMCMappable
    {
        public string Descricao { get; set; }

        public int NumeroItens { get; set; }
    }
}
