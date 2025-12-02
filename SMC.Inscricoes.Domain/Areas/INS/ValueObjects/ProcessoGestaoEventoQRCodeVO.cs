using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ProcessoGestaoEventoQRCodeVO : ISMCMappable
    {
        public long SeqProcesso { get; set; }
        public string DescricaoProcesso { get; set; }
        public string TokenHistoricoSituacao { get; set; }
        public List<ProcessoGrupoOfertaQRCodeVO> GrupoOfertas { get; set; }
        public List<ProcessoHierarquiaQRcodeVO> Hierarquias { get; set; }
        public List<ProcessoOfertaQRCodeVO> Ofertas { get; set; }
    }
}
