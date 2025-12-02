using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.SEL.ValueObjects
{
    public class InscricaoOfertaHistoricoSituacaoVO : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqSituacao { get; set; }

        public long SeqInscricao { get; set; }

        public string TokenSituacao { get; set; }

        public bool Atual { get; set; }

        public string Justificativa { get; set; }

        public long? SeqMotivoSituacao { get; set; }
    }
}
