using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo (usado em InscricoesProcessoVO)
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricaoListaInscritoData : ISMCMappable
    {
        public InscricaoListaInscritoData()
        {
            DescricaoOfertas = new List<string>();
        }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        public long SeqGrupoOferta { get; set; }

        [DataMember]
        public DateTime DataInscricao { get; set; }

        [DataMember]
        public string DescricaoSituacaoAtual { get; set; }

        [DataMember]
        public string TokenSituacaoAtual { get; set; }

        [SMCMapForceFromTo]
        [DataMember]
        public List<string> DescricaoOfertas { get; set; }

        [DataMember]
        public SMCLanguage IdiomaInscricao { get; set; }

        [DataMember]
        public string DescricaoGrupoOferta { get; set; }

        [DataMember]
        public string DescricaoLabelOferta { get; set; }

        [DataMember]
        public bool ConfiguracaoEtapaVigente { get; set; }

        [DataMember]
        public bool GrupoPossuiOfertaVigente { get; set; }

        [DataMember]
        public bool ProcessoCancelado { get; set; }

        [DataMember]
        public Guid UidProcesso { get; set; }

        [DataMember]
        public bool PermissaoInscricaoForaPrazo { get; set; }

        [DataMember]
        public SituacaoEtapa SituacaoEtapa { get; set; }

        [DataMember]
        public string PaginaAtualInscricao { get; set; }

        [DataMember]
        public bool ExisteDocumentoObrigatorioIndeferidoOuPendente { get; set; }

        [DataMember]
        public bool ExisteGrupoDocumentoIndeferidoOuPendente { get; set; }

        [DataMember]
        public bool PermiteNovaEntregaDocumentacao { get; set; }

        [DataMember]
        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }

        [DataMember]
        public DateTime? DataEncerramentoProcesso { get; set; }

        [DataMember]
        public bool ProcessoEncerrado { get; set; }
        
        [DataMember]
        public bool HabilitaCheckin { get; set; }

        [DataMember]
        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }

        [DataMember]
        public bool DocumentacaoEntregue { get; set; }

        [DataMember]
        public bool GestaoEventos { get; set; }

        public long SeqTipoDocumento { get; set; }

        public long SeqProcesso { get; set; }


        #region Botões dinamicos

        public string BotaoContiuar { get; set; }
        public string BotaoContiuarTootip { get; set; }
        public bool HabilitarBotaoContinuar { get; set; }
        public string MensagemBotaoContinuar { get; set; }
        public string TokenResource { get; set; }

        #endregion

    }
}
