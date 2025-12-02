using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Enums;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ArvoreItemConfiguracaoPaginaEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        public long Seq { get; set; }

        public long SeqEtapaProcesso { get; set; }

        public long SeqProcesso { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqItem { get; set; }

        public long SeqPai { get; set; }
        
        [SMCDescription]
        public string Descricao { get; set; }

        public TipoItemPaginaEtapa Tipo { get; set; }

        public bool PaginaPermiteExibicaoOutrasPaginas { get; set; }

        public bool PaginaObrigatoria { get; set; }

        public bool PaginaExibeFormulario { get; set; }

        public bool PaginaPermiteDuplicar { get; set; }

        public string PaginaToken { get; set; }

        public bool ExibeContextMenu 
        {
            get 
            {
                if (this.Tipo != TipoItemPaginaEtapa.Pagina || PaginaToken.Equals(TOKENS.PAGINA_COMPROVANTE_INSCRICAO) ||
                   (this.Tipo == TipoItemPaginaEtapa.Pagina && (this.PaginaPermiteDuplicar || this.PaginaPermiteExibicaoOutrasPaginas || !PaginaObrigatoria || (this.PaginaExibeFormulario && this.PaginaPermiteDuplicar))))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}