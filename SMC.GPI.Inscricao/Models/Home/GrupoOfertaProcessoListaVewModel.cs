using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Models
{
    public class GrupoOfertaProcessoListaVewModel : SMCViewModelBase
    {
        public long SeqGrupo { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public string NomeGrupo { get; set; }

        public DateTime DataInicioConfiguracaoEtapa { get; set; }

        public DateTime DataFimConfiguracaoEtapa { get; set; }

        public SMCLanguage IdiomaAtual { get; set; }

        #region Botões dinamicos
        public string BotaoInscrever { get; set; }
        public string BotaoInscreverTootip { get; set; }
        public bool HabilitarBotaoInscrever { get; set; }
        public string MensagemBotaoInscrever { get; set; }
        #endregion
    }
}