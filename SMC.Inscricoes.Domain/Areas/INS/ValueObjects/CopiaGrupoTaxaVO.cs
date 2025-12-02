using SMC.Framework.Mapper;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class CopiaGrupoTaxaVO : ISMCMappable
    {
        public long SeqProcessoOrigem { get; set; }

        public long SeqProcessoCopia { get; set; }

        public string Descricao { get; set; }

        public short NumeroMinimoItens { get; set; }

        public short? NumeroMaximoItens { get; set; }

        public List<GrupoTaxaItemVO> Itens { get; set; }

        public List<TaxaVO> TaxasProcessoCopia { get; set; }

    }
}
