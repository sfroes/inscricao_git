using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class ConvocadoData : ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public long SeqInscricaoOferta { get; set; }

        public string Nome { get; set; }        

        public string Situacao { get; set; }

        public string TokenSituacao { get; set; }

        public string TokenEtapa { get; set; }
    }
}
