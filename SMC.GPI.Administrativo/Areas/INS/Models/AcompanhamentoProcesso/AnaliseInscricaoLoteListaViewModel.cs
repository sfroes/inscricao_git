using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AnaliseInscricaoLoteListaViewModel : SMCViewModelBase, ISMCMappable
	{
        [SMCKey]
        [SMCSortable(true, false)]
        public long Seq { get; set; }

        [SMCSortable(true, false, "Inscrito.Nome")]
        public string NomeInscrito { get; set; }
        
        [SMCLink("ListarSituacaoInscricao", "AcompanhamentoProcesso", SMCLinkTarget.Modal, "Seq", ModalWindowTitleResourceKey = "SituacaoInscricao_Modal_Title")]
        public string DescricaoSituacaoAtual { get; set; }

        [SMCHidden]
        public string JustificativaSituacaoAtual { get; set; }

        [SMCSortable(true, false, "SeqGrupoOferta")]
        public string DescricaoGrupoOferta { get; set; }

        [SMCMapForceFromTo()]
        public List<OfertaInscricaoViewModel> OpcoesOferta { get; set; }
	}
}