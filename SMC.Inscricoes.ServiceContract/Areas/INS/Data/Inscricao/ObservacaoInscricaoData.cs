using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class ObservacaoInscricaoData : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqInscrito { get; set; }
        public long SeqProcesso { get; set; }
        public string Observacao { get; set; }

        public string Nome { get; set; }
    }
}
