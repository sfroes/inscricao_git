using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class PermissaoInscricaoForaPrazoVO : ISMCMappable
    {
        public long SeqProcesso { get; set; }
        
        public long Seq { get; set; }

        public long SeqInscritoEdicao { get; set; }

        public List<PermissaoInscricaoForaPrazoInscritoVO> Inscritos { get; set; }
        
        public DateTime DataInicio { get; set; }
        
        public DateTime DataFim { get; set; }

        public DateTime? DataVencimento { get; set; }
    }

    public class PermissaoInscricaoForaPrazoInscritoVO
    {
        public long? Seq { get; set; }

        public string Nome { get; set; }        

        public long SeqInscrito { get; set; }
    }
}
