using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.TipoDocumento
{
    public class TipoProcessoDocumentoData: ISMCMappable
    {
        [DataMember]
        public long SeqTipoDocumento { get; set; }

        [DataMember]
        public bool Ativo { get; set; }
    }
}
