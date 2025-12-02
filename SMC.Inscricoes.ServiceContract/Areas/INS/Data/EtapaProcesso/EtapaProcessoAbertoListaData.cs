using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class EtapaProcessoAbertoListaData : ISMCMappable
    {
        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public string DescricaoProcesso { get; set; }

        [DataMember]
        public string DescricaoComplementarProcesso { get; set; }

        [DataMember]
        public List<SMCLanguage> IdiomasDisponiveis { get; set; }

        [DataMember]
        public string UrlInformacaoComplementar { get; set; }

        [DataMember]
        public DateTime DataInicioEtapa { get; set; }

        [DataMember]
        public DateTime DataFimEtapa { get; set; }

        [DataMember]
        public List<GrupoOfertaEmAbertoData> Grupos { get; set; }

        [DataMember]
        public Guid UidProcesso { get; set; }

        #region Botões dinamicos
        public string BotaoInscrever { get; set; }
        public string BotaoInscreverTootip { get; set; }
        public bool HabilitarBotaoInscrever { get; set; }
        public string MensagemBotaoInscrever { get; set; }
        #endregion
    }
}
