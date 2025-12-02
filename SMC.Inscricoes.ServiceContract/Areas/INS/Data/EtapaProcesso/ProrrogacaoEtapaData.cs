using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class ProrrogacaoEtapaData : ISMCMappable
    {
        public long SeqEtapaProcesso { get; set; }

        public long SeqProcesso { get; set; }

        public string Descricao { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime DataFimAntiga { get; set; }               

        public List<ProrrogacaoConfiguracaoData> Configuracoes { get; set; }        
    }
}
