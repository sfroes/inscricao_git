using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class ConfiguracaoUsuarioData : ISMCMappable
    {
        public long? SeqProcesso { get; set; }
        public long? SeqGrupoOferta { get; set; }
        public long? SeqHierarquia { get; set; }
        public string TokenHistoricoSituacao { get; set; }
        public List<long> SeqsOferta { get; set; }
    }
}
