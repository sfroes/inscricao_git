using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin
{
    public class ProcessoGestaoEventoQRCodeData : ISMCMappable
    {
        public long SeqProcesso { get; set; }
        public string DescricaoProcesso { get; set; }
        public bool Atendente { get; set; }
        public string TokenHistoricoSituacao { get; set; }
        public List<ProcessoGrupoOfertaQRCodeData> GrupoOfertas { get; set; }
        public List<ProcessoHierarquiaQRcodeData> Hierarquias { get; set; }
        public List<ProcessoOfertaQRCodeData> Ofertas { get; set; }
    }
}
