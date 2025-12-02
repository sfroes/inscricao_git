using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Idiomas de um determinado processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class IdiomasPaginasProcessoData : ISMCMappable
    {
        public long SeqConfiguracaoEtapa { get; set; }

        public SMCLanguage IdiomaPadrao { get; set; }

        public List<SMCLanguage> IdiomasDisponiveis { get; set; }

        public List<SMCLanguage> IdiomasEmUso { get; set; }
    }
}
