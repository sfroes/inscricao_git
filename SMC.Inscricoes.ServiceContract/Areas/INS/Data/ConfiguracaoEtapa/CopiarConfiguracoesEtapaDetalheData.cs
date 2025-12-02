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
    public class CopiarConfiguracoesEtapaDetalheData : ISMCMappable
    {
        
        public long SeqConfiguracaoEtapa { get; set; }

        public string Descricao { get; set; }

        public string NumeroEdital { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime? DataLimiteDocumentacao { get; set; }

    }
}