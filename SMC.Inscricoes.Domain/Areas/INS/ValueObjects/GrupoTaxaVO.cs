using SMC.Framework.Mapper;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class GrupoTaxaVO : ISMCMappable
    {

        public long Seq { get; set; }

        public long SeqProcesso { get; set; }

        public string Descricao { get; set; }

        public short NumeroMinimoItens { get; set; }

        public short? NumeroMaximoItens { get; set; }

        public List<GrupoTaxaItemVO> Itens { get; set; }

    }
}
