using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class DetalheAlteracaoSituacaoData : ISMCMappable
    {
        public long SeqInscricao { get; set; }
        
        public string NomeInscrito { get; set; }
    }
}
