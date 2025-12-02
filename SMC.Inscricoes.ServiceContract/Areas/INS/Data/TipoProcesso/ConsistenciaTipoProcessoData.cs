using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class ConsistenciaTipoProcessoData : ISMCMappable
    {
        public TipoConsistencia TipoConsistencia { get; set; }
    }
}
