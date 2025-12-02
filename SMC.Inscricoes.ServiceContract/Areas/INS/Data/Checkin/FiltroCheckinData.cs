using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class FiltroCheckinData : ISMCMappable
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string TokenHistoricoSituacao { get; set; }
        public List<long> SeqsOferta { get; set; }
    }
}
