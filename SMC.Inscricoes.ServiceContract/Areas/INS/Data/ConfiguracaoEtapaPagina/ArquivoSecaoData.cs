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
    public class ArquivoSecaoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqSecaoPaginaSGF { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        [DataMember]
        public int Ordem { get; set; }

        [DataMember]
        public string NomeLink { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public long SeqArquivo { get; set; }

        [DataMember]
        public SMCUploadFile Arquivo { get; set; }


    }
}
