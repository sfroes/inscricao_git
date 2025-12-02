using SMC.Framework.Mapper;
using SMC.Framework.Model;
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
    public class CabecalhoProcessoEtapaConfiguracaoData : ISMCMappable
    {        
        public long SeqProcesso { get; set; }
     
        public long SeqEtapaProcesso { get; set; }
        
        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqEtapaProcessoSGF { get; set; }

        public string DescricaoTipoProcesso { get; set; }

        public string DescricaoEtapaProcesso { get; set; }

        public string DescricaoConfiguracaoEtapa { get; set; }

        public string DescricaoProcesso { get; set; }
    }
}