using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ProrrogacaoEtapaVO : ISMCMappable
    {
        public long SeqEtapaProcesso { get; set; }

        public long SeqProcesso { get; set; }

        public string Descricao { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime DataFimAntiga { get; set; }               

        public List<ProrrogacaoConfiguracaoVO> Configuracoes { get; set; }       
        
        public string MensagemInformativa { get; set; } 
    }
}
