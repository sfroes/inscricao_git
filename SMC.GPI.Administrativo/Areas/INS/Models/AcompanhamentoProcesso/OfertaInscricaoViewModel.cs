using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class OfertaInscricaoViewModel : SMCViewModelBase, ISMCMappable
    {
        public short NumeroOpcao { get; set; }

        public string DescricaoOferta { get; set; }

        public string DescricaoOfertaOriginal { get; set; }

        public string Justificativa { get; set; }

        public override string ToString()
        {
            if (DescricaoOferta == DescricaoOfertaOriginal)
                return $"{NumeroOpcao}ª opção: {DescricaoOferta}";

            return $"NumeroOpcaoª opção:<br />Oferta atual: {DescricaoOferta}<br />Oferta original: {DescricaoOfertaOriginal}";
        }
    }
}