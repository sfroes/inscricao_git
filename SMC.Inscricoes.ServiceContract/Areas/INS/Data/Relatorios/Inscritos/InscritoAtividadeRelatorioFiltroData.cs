using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract]
    public class InscritoAtividadeRelatorioFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public List<long> SeqsOfertas { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

    }
}



