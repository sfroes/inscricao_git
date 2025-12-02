using SMC.DadosMestres.Common.Constants;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{

    public class AcompanhamentoInscritoListaData : ISMCMappable
    {
       
        #region [Dados Inscrito]
        public long SeqInscrito { get; set; }
        public string NomeInscrito { get; set; }
        public string Cpf { get; set; }
        public string NumeroPassaporte { get; set; }

        public string CpfouPassaporte { get; set; }
        public long SeqSituacao { get; set; }
        #endregion

        #region [Dados Processo]
        public long SeqProcesso { get; set; }
        public long SeqTipoProcesso { get; set; }
        public string TipoProcesso { get; set; }
        public string DescricaoProcesso { get; set; }
        public string SemestreAno { get; set; }
        public int Semestre { get; set; }
        public int Ano { get; set; }
        #endregion

        #region[Dados Inscrição]

        public long SeqInscricao { get; set; }
        public long SeqInscricaoHistoricoSituacao { get; set; }
        public string SituacaoInscricao { get; set; }
        #endregion

        #region [Dados Oferta]

        public List<OpcaoOfertaData> OpcoesOferta { get; set; }
        #endregion

    }
}
