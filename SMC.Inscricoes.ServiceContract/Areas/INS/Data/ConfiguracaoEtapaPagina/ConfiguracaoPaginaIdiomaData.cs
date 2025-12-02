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
    public class ConfiguracaoPaginaIdiomaData : ISMCMappable
    {
        [DataMember]
        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        [DataMember]
        public long SeqUnidadeResponsavel { get; set; }

        [DataMember]
        public long SeqPaginaEtapaSGF { get; set; }

        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public bool ExibeFormulario { get; set; }

        [DataMember]
        public long? SeqTipoFormulario { get; set; }

        [DataMember]
        public long? SeqFormulario { get; set; }

        [DataMember]
        public long? SeqVisao { get; set; }

        [DataMember]
        public long? SeqVisaoGestao { get; set; }

        [DataMember]
        public string PaginaToken { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }
    }
}
