using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.INS.Data
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo (usado em InscricoesProcessoVO)
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricaoListaData : ISMCMappable
    {
        public InscricaoListaData()
        {
            DescricaoOfertas = new List<string>();
        }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long Seqinscrito { get; set; }

        [DataMember]
        public string DescricaoSituacaoAtual { get; set; }

        [DataMember]
        public string TokenSituacaoAtual { get; set; }

        [SMCMapForceFromTo]
        [DataMember]
        public List<string> DescricaoOfertas { get; set; }

        [DataMember]
        public SMCLanguage IdiomaInscricao { get; set; }
    }
}
