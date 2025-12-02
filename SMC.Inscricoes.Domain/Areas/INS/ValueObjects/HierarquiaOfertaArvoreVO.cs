using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class HierarquiaOfertaArvoreVO
    {
        public string Descricao { get; set; }

        public long Seq { get; set; }

        public long? SeqPai { get; set; }

        public bool IsLeaf { get; set; }

        public string Token { get; set; }
    }
}
