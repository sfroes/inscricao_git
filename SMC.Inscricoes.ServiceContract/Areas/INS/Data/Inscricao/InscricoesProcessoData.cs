using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Object usado para listagem de inscrições num determinado processo  na tela inicial do GPI
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricoesProcessoData : ISMCMappable
    {

        public InscricoesProcessoData()
        {
            Inscricoes = new List<InscricaoListaInscritoData>();
        }

        [SMCMapForceFromTo]
        [DataMember]
        public List<InscricaoListaInscritoData> Inscricoes { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public string DescricaoProcesso { get; set; }

        [DataMember]
        public Guid UidProcesso { get; set; }

        [DataMember]
        public string SituacaoProcesso { get; set; }

        [DataMember]
        public bool GestaoEventos { get; set; }

        #region Botões dinamicos
        public string BotaoContiuar { get; set; }
        public string BotaoContiuarTootip { get; set; }
        public bool HabilitarBotaoContinuar { get; set; }
        public string MensagemBotaoContinuar { get; set; }
        public string TokenResource { get; set; }
        #endregion
    }
}
