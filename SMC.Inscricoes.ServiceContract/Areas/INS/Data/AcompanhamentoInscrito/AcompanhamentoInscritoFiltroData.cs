using SMC.Framework.Mapper;
using SMC.Framework.Model;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class AcompanhamentoInscritoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        #region Processo
        public long? SeqUnidadeResponsavel { get; set; }

        public long? SeqTipoProcesso { get; set; }
        public long? SeqProcesso { get; set; }

        public long? SemestreReferencia { get; set; }

        public int? AnoReferencia { get; set; }

        public string DescricaoProcesso { get; set; }
        #endregion

        #region Inscrito
        public long? SeqInscrito { get; set; }
        public string NomeInscrito { get; set; }
        public string Cpf { get; set; }
        public string NumeroPassaporte { get; set; }
        #endregion
        public long? Seq { get; set; }
        public long? SeqInscricao { get; set; }

        [SMCMapProperty("Oferta.Seq")]
        public long? SeqOferta { get; set; }
    }
}