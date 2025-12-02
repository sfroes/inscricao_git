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
    public class FluxoPaginaData : ISMCMappable
    {
        [DataMember]
        public long SeqConfiguracaoEtapaPagina { get; set; }

        [DataMember]
        public int Ordem { get; set; }

        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public long? SeqFormularioSGF { get; set; }

        [DataMember]
        public long? SeqVisaoSGF { get; set; }

        [DataMember]
        public long? SeqPaginaIdioma { get; set; }

        [DataMember]
        public bool ExibeConfirmacaoInscricao { get; set; }

        [DataMember]
        public bool ExibeComprovanteInscricao { get; set; }

        [DataMember]
        public string Alerta { get; set; }

    }
}
