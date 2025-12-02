using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Models
{
    public class InscricaoListaViewModel : SMCViewModelBase
    {

        public InscricaoListaViewModel()
        {
            Inscricoes = new List<InscricaoProcessoItemViewModel>();
        }

        [SMCMapForceFromTo]
        public List<InscricaoProcessoItemViewModel> Inscricoes { get; set; }        

        public long SeqProcesso {get;set;}

        public string DescricaoProcesso { get; set; }

        public Guid UidProcesso { get; set; }

        public int OfertasInativas { get; set; }

        public int TotalOfertasProcesso { get; set; }

        #region Botões dinamicos
        public string BotaoContiuar { get; set; }

        public string BotaoContiuarTootip { get; set; }
        #endregion

    }
}