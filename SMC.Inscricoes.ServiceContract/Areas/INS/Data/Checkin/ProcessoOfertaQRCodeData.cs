using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class ProcessoOfertaQRCodeData : ISMCMappable
    {
        public long SeqProcesso { get; set; }
        public long SeqGrupoOferta { get; set; }
        public long? SeqNivelSuperior { get; set; }
        public long SeqOferta { get; set; }
        public string DescricaoCompleta { get; set; }
        public string DescricaoSimples { get; set; }
        public DateTime DataHoraInicioEvento { get; set; }
        public DateTime DataHoraFimEvento { get; set; }
    }
}
