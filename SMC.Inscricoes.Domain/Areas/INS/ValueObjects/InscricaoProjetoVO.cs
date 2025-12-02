using SMC.DadosMestres.Common.Constants;
using SMC.Framework.Mapper;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoProjetoVO : ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public long SeqProjeto { get; set; }
    }
}