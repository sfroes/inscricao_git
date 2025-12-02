using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class CheckinData : ISMCMappable
    {
        public long? SeqProcesso { get; set; }
        public long? SeqOferta{ get; set; }
        public long? SeqGrupoOferta { get; set; }
        public long? SeqHierarquia { get; set; }
        public string Guid { get; set; }
        public TipoCheckin TipoCheckin { get; set; }
        public string TokenHistoricoSituacao { get; set; }
        public List<long> SeqsOferta { get; set; }
        public List<long> SeqsInscricao { get; set; }

    }
}
