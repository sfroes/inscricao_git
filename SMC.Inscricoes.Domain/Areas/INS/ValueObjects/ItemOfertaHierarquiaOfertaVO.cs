using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ItemOfertaHierarquiaOfertaVO : ISMCMappable
    {
        public long SeqHierarquiaOfertaOrigem { get; set; }

        public long? SeqHierarquiaOfertaGPI { get; set; }

        public string Descricao { get; set; }

        public string TokenTipoItemHierarquiaOferta { get; set; }
    }
}