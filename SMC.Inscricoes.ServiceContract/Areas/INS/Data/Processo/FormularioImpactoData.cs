using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class FormularioImpactoData : ISMCMappable
    {
        [DataMember]
        public long SeqFormulario { get; set; }
        [DataMember]
        public DateTime? DataFimResposta { get; set; }
        [DataMember]
        public long SeqDadoFormulario {get; set; }
        [DataMember]
        public long SeqInscricao {get; set; }
        [DataMember]
        public long SeqVisaoSGF {get; set; }
        [DataMember]
        public long SeqProcesso { get; set; }
        [DataMember]
        public bool ExibirFormularioImpacto { get; set; }
        [DataMember]
        public string MensagemInformativa { get; set; }
        public string DescricaoMensagemInformativa { get; set; }
        public string MensagemFormularioImpactoIndisponivel { get; set; }

    }
}
