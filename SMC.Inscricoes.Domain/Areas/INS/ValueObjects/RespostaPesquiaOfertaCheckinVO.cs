using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class RespostaPesquiaOfertaCheckinVO : ISMCMappable
    {
        public string DescricaoOferta { get; set; }
        public long SeqOferta { get; set; }
        public List<RespostaPesquiaOfertaInscritoCheckinVO> Inscritos { get; set; }       
    }
}
