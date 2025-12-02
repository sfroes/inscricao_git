using SMC.Framework;
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
    /// <summary>
    /// Value Objec uma opção de oferta de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class LancamentoNotaData : ISMCMappable
    {   
        
        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public string NomeInscrito { get; set; }

        [DataMember]
        public decimal? NotaGeral { get; set; }

        [DataMember]
        public int? NumeroClassificacao { get; set; }        
    }
}
