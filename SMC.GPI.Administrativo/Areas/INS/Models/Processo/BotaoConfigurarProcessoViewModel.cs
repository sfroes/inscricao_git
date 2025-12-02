using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class BotaoConfigurarProcessoViewModel : SMCViewModelBase
    {
        public SMCEncryptedLong SeqProcesso { get; set; }
        public ProcessoActionsEnum Action { get; set; }
    }

    public enum ProcessoActionsEnum : short
    {
        Nenhum,
        Alterar,
        HierarquiaOferta,
        AssociacaoEtapa,
        GrupoOferta,
        GrupoTaxa,
        TaxaOfertaLote,
        ConfigurarNotificacao,
        InscricaoForaPrazo
    }
}