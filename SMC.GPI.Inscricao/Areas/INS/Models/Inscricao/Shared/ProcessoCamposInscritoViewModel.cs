using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Inscricao.Models;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.Localidades.UI.Mvc.DataAnnotation;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class ProcessoCamposInscritoViewModel : SMCViewModelBase, ISMCMappable
    {
        public bool ExibirNome { get; set; }
        public bool ExibirNomeSocial { get; set; }
        public bool ExibirDataNascimento { get; set; }
        public bool ExibirSexo { get; set; }
        public bool ExibirEstadoCivil { get; set; }
        public bool ExibirNacionalidade { get; set; }
        public bool ExibirPaisOrigem { get; set; }
        public bool ExibirNaturalidade { get; set; }
        public bool ExibirCPF { get; set; }
        public bool ExibirPassaporte { get; set; }
        public bool ExibirNumeroIdentidade { get; set; }
        public bool ExibirOrgaoEmissorIdentidade { get; set; }
        public bool ExibirUfIdentidade { get; set; }
        public bool ExibirFiliacao { get; set; }
        public bool ExibirEndereco { get; set; }
        public bool ExibirTelefone { get; set; }
        public bool ExibirEmail { get; set; }
        public bool ExibirOutrosEnderecosEletronicos { get; set; }

    }
}