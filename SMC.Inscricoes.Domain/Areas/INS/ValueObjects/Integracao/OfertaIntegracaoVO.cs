using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class OfertaIntegracaoVO : ISMCMappable
    {
        public long SeqOferta { get; set; }
        
        public long SeqInscricaoOferta { get; set; }
    }
}
