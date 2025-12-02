using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class FiltroCheckinVO : ISMCMappable
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string TokenHistoricoSituacao { get; set; }
        public List<long> SeqsOferta { get; set; }
    }
}
