using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class FluxoPaginaMenuViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqPaginaIdioma { get; set; }
        public bool ExibeConfirmacaoInscricao { get; set; }
        public bool ExibeComprovanteInscricao { get; set; }
        public string Alerta { get; set; }
        public long SeqConfiguracaoEtapaPagina { get; set; }
        public int Ordem { get; set; }
        public string Token { get; set; }
        public string Titulo { get; set; }
        public long SeqFormularioSGF { get; set; }
        public long SeqVisaoSGF { get; set; }
    }
}
