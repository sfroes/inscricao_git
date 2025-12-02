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
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoIdiomaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public SMCLanguage Idioma { get; set; }

        [DataMember]
        public bool Padrao { get; set; }

        [DataMember]
        public string DescricaoComplementar { get; set; }

        [DataMember]
        public string LabelCodigoAutorizacao { get; set; }

        [DataMember]
        public string LabelGrupoOferta { get; set; }

        [DataMember]
        public string LabelOferta { get; set; }

    }
}
