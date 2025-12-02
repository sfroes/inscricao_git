using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoOfertaPorSituacaoVO : ISMCMappable
    {
        public long[] SeqOfertas { get; set; }

        public string[] Tokens { get; set; }
    }
}
