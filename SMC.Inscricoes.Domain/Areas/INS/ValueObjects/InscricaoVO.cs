using System;
using System.Collections.Generic;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo (usado em InscricoesProcessoVO)
    /// </summary>
    public class InscricaoVO : ISMCMappable
    {

        public InscricaoVO()
        {
            DescricaoOfertas = new List<string>();
        }

        public long SeqInscrito { get; set; }

        public long SeqInscricao { get; set; }
        public long SeqProcesso { get; set; }
        public long SeqTipoDocumento { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqGrupoOferta { get; set; }

        public SMCLanguage IdiomaInscricao { get; set; }

        public DateTime DataInscricao { get; set; }

        public string DescricaoSituacaoAtual { get; set; }        

        public string TokenSituacaoAtual { get; set; }

        public List<string> DescricaoOfertas { get; set; }

        public string DescricaoGrupoOferta { get; set; }

        public string DescricaoLabelOferta { get; set; }

        public bool ConfiguracaoEtapaVigente { get; set; }

        public bool GrupoPossuiOfertaVigente { get; set; }

        public bool ProcessoCancelado { get; set; }

        public Guid UidProcesso { get; set; }

        public bool PermissaoInscricaoForaPrazo { get; set; }

        public SituacaoEtapa SituacaoEtapa { get; set; }

        public string PaginaAtualInscricao { get; set; }

        public bool ExisteDocumentoObrigatorioIndeferidoOuPendente { get; set; }

        public bool ExisteGrupoDocumentoIndeferidoOuPendente { get; set; }

        public bool PermiteNovaEntregaDocumentacao { get; set; }

        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }

        public DateTime? DataEncerramentoProcesso { get; set; }

        public bool ProcessoEncerrado { get; set; }

        public bool HabilitaCheckin { get; set; }

        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }

        public bool DocumentacaoEntregue { get; set; }

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
