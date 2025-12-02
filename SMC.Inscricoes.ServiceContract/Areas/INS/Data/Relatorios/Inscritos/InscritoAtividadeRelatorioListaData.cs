using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class InscritoAtividadeRelatorioListaData : ISMCMappable
    {
        public long SeqOferta { get; set; }
        public string DescricaoOferta { get; set; }
        public long SeqProcesso { get; set; }
        public string DescricaoProcesso { get; set; }
        public string NumeroInscricao { get; set; }
        public string NomeInscrito { get; set; }
    }
}
