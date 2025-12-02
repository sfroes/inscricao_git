using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ArvoreItemHierarquiaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        public long Seq { get; set; }

        public long SeqTipoItemHierarquiaOferta { get; set; }

        public long SeqPai { get; set; }

        public long SeqProcesso { get; set; }

        public string Descricao { get; set; }

        [SMCDescription]
        public string DescricaoFormatada
        {
            get
            {
                if (Cancelada)
                    return $"{Descricao} (Cancelada)";
                else if (Desativada)
                    return $"{Descricao} (Desativada)";
                return Descricao;
            }
        }

        public bool EOferta { get; set; }

        public bool PermiteCadastroOfertaFilha { get; set; }

        public bool PermiteCadastroItemFilho { get; set; }

        public bool PermiteCadastroNetos { get; set; }

        public bool Cancelada { get; set; }

        public bool Desativada { get; set; }
    }
}