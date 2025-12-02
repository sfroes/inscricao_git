using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class DadosLiberacaoInscricaoVO : ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public string TokenSituacaoInscricao { get; set; }

        public List<string> TokenSituacoesOfertas { get; set; }

        public SituacaoEtapa SituacaoEtapa { get; set; }

        public DateTime? DataInicioEtapa { get; set; }

        public DateTime? DataFimEtapa { get; set; }

        public DateTime? InscricaoForaPrazoDataInicio { get; set; }

        public DateTime? InscricaoForaPrazoDataFim { get; set; }

        public ArquivoAnexado ArquivoComprovante { get; set; }

        public bool? EnviarNotificacao { get; set; }

        public long? SeqConfiguracaoEtapaPagina { get; set; }
    }
}
