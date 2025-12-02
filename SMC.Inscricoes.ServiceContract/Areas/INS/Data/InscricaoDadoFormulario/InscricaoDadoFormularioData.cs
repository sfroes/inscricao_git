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
    public class InscricaoDadoFormularioData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long SeqFormulario { get; set; }

        [DataMember]
        public long SeqVisao { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        [DataMember]
        public bool Editable { get; set; }

        [DataMember]
        public List<InscricaoDadoFormularioCampoData> DadosCampos { get; set; }
        
        [DataMember]
        public string TituloFormulario { get; set; }
    }
}
