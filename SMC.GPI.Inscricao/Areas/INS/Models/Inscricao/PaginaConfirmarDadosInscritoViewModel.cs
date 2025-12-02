using System;
using System.Collections.Generic;
using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Framework.Model;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Localidades.UI.Mvc.Models;
using SMC.Inscricoes.Common;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaConfirmarDadosInscritoViewModel : PaginaViewModel
    {
        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_CONFIRMACAO_DADOS_INSCRITO;
            }
        }

        public override string ChaveTextoBotaoProximo
        {
            get
            {
                return "Botao_Navegacao_ConfirmarProsseguir";
            }
        }

        public PaginaConfirmarDadosInscritoViewModel()
        {
            DadosInscrito = new DadosInscritoViewModel();
        }

        public DadosInscritoViewModel DadosInscrito { get; set; }
    }
}



