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
    public class CabecalhoProcessoEtapaViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long SeqEtapaProcesso { get; set; }


        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        public string DescricaoTipoProcesso { get; set; }

        public string DescricaoEtapa { get; set; }

        public string DescricaoProcesso { get; set; }

        public string DescricaoConfiguracaoEtapa { get; set; }

        public EtapaProcessoActionsEnum Action { get; set; }

        public bool ExibirBotaoAlterarAssociacaoEtapaProcesso { get; set; }

        public bool ExibirBotaoCadastroConfiguracaoEtapaProcesso { get; set; }

        public bool ExibirBotaoConfiguracaoEtapaProcesso { get; set; }
    }
}