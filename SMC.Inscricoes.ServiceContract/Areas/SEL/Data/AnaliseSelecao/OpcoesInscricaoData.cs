using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class OpcoesInscricaoData : ISMCMappable
    {
        public string Opcao { get; set; }
        
        public string Oferta { get; set; }
        
        public string Situacao { get; set; }

        public long SeqOferta { get; set; }
    }
}
