using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class InscricaoHistoricoSituacaoData : ISMCMappable
    {
        public long Seq { get; set; }        
        
        public long SeqSituacao { get; set; }
        
        public string Situacao { get; set; }

        public DateTime Data { get; set; }

        public string Responsavel { get; set; }

        [SMCMapProperty("SeqMotivoSGF")]
        public long? SeqMotivo { get; set; }

        public string Motivo { get; set; }

        public string Justificativa { get; set; }

        public bool Atual { get; set; }

        public string TokenSituacao { get; set; }

        public string TokenEtapa { get; set; }
    }
}
