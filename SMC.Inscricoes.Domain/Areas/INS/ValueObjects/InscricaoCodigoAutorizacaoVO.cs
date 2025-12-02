using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoCodigoAutorizacaoVO : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqInscricao { get; set; }

        public long SeqCodigoAutorizacao { get; set; }

        public string Codigo { get; set; }
    }
}
