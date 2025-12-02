using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Inscricao
{
    public class JustificativaSituacaoInscricaoData : ISMCMappable
    {        
        public string NomeInscrito { get; set; }

        public string Motivo { get; set; }

        public string Justificativa { get; set; }
    }
}
