using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class TipoProcessoConsistenciaData : ISMCMappable
    {
        public long? SeqInscricao { get; set; }

        public long? SeqTipoProcesso { get; set; }

        public long? SeqProcesso { get; set; }

        public TipoConsistencia TipoConsistencia { get; set; }
    }
}
