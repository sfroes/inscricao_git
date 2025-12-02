using SMC.Framework.Mapper;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.ViewEventoSae
{
    public class ViewEventoSaeData : ISMCMappable
    {
        public int Codigo { get; set; }

        public int? Ano { get; set; }

        public int CodigoUnidadePromotora { get; set; }

        public string Nome { get; set; }
    }
}
