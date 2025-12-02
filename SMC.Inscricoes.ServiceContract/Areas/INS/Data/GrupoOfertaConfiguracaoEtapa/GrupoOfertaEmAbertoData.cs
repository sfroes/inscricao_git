using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class GrupoOfertaEmAbertoData : ISMCMappable
    {
        [DataMember]
        public long SeqGrupo { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        public string NomeGrupo { get; set; }

        [DataMember]
        public DateTime DataInicioConfiguracaoEtapa { get; set; }

        [DataMember]
        public DateTime DataFimConfiguracaoEtapa { get; set; }

        [DataMember]
        public SMCLanguage IdiomaAtual { get; set; }

        #region Botões dinamicos
        public string BotaoInscrever { get; set; }
        public string BotaoInscreverTootip { get; set; }
        public bool HabilitarBotaoInscrever { get; set; }
        public string MensagemBotaoInscrever { get; set; }
        #endregion
    }
}

