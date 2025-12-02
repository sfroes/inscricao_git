using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ProcessoCampoInscritoVO : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqProcesso { get; set; }
        public CampoInscrito CampoInscrito { get; set; }
    }
}

