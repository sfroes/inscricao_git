using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class GrupoOfertaConfiguracaoEtapaVO : ISMCMappable
    {
        public long SeqGrupo { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public string NomeGrupo { get; set; }

        public DateTime DataInicioConfiguracaoEtapa { get; set; }

        public DateTime DataFimConfiguracaoEtapa { get; set; }

        public SMCLanguage IdiomaAtual { get; set; }

        public List<InscricaoVO> Inscricoes { get; set; }

        #region Botões dinamicos

        public string BotaoInscrever { get; set; }
        public string BotaoInscreverTootip { get; set; }
        public bool HabilitarBotaoInscrever { get; set; }
        public string MensagemBotaoInscrever { get; set; }
             
        #endregion
    }
}
