using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Models
{
    public class ProcessoAbertoListaViewModel : SMCViewModelBase
    {
        public long SeqProcesso { get; set; }

        public string DescricaoProcesso { get; set; }

        public string DescricaoComplementarProcesso { get; set; }

        public List<SMCLanguage> IdiomasDisponiveis { get; set; }

        public string UrlInformacaoComplementar { get; set; }

        public DateTime DataInicioEtapa { get; set; }

        public DateTime DataFimEtapa { get; set; }

        public List<GrupoOfertaProcessoListaVewModel> Grupos { get; set; }

        public Guid UidProcesso { get; set; }

    }
}