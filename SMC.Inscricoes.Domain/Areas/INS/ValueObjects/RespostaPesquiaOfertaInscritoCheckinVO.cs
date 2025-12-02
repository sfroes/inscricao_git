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
    public class RespostaPesquiaOfertaInscritoCheckinVO : ISMCMappable
    {
        public string Nome { get; set; }
        public string GuidInscricao { get; set; }
        public bool CheckinEfetuado { get; set; }
    }
}
