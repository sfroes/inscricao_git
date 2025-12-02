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
    public class DadosInscritoViewModel : SMCViewModelBase, ISMCMappable
    {
        public DadosInscritoViewModel()
        {
            Telefones = new List<TelefoneViewModel>();
            EnderecosEletronicos = new List<EnderecoEletronicoViewModel>();
        }

        public string Nome { get; set; }

        private string _nomeSocial;
        [SMCMapForceFromTo]
        public string NomeSocial 
        {
            get { return string.IsNullOrEmpty(_nomeSocial) ? "---" : _nomeSocial; }
            set { _nomeSocial = value; }
        }

        public DateTime DataNascimento { get; set; }

        public Sexo Sexo { get; set; }

        public EstadoCivil EstadoCivil { get; set; }

        public string DescricaoEstadoCivil 
        { 
            get { return EstadoCivil == EstadoCivil.Nenhum ? "---" : EstadoCivil.SMCGetDescription(); }
            set { DescricaoEstadoCivil = value; }
        }

        /*
        private string _estadoCivil;
        [SMCMapForceFromTo]
        public string EstadoCivil 
        {
            get { return string.IsNullOrEmpty(_estadoCivil) ? "---" : _estadoCivil; }
            set { _estadoCivil = value; }
        }
        */
        [SMCCpf(true)]
        public string Cpf { get; set; }

        private string _numeroPassaporte;
        [SMCMapForceFromTo]
        public string NumeroPassaporte 
        {
            get { return string.IsNullOrEmpty(_numeroPassaporte) ? "---" : _numeroPassaporte; }
            set { _numeroPassaporte = value; } 
        }

        private string _numeroIdentidade;
        [SMCMapForceFromTo]
        public string NumeroIdentidade 
        {
            get { return string.IsNullOrEmpty(_numeroIdentidade) ? "---" : _numeroIdentidade; }
            set { _numeroIdentidade = value; }
        }

        private string _orgaoEmissorIdentidade;
        [SMCMapForceFromTo]
        public string OrgaoEmissorIdentidade 
        {
            get { return string.IsNullOrEmpty(_orgaoEmissorIdentidade) ? "---" : _orgaoEmissorIdentidade; }
            set { _orgaoEmissorIdentidade = value; } 
        }

        private string _ufIdentidade;
        [SMCMapForceFromTo]
        public string UfIdentidade 
        {
            get { return string.IsNullOrEmpty(_ufIdentidade) ? "---" : _ufIdentidade; }
            set { _ufIdentidade = value; }
        }

        public TipoNacionalidade Nacionalidade { get; set; }

        public string PaisNacionalidade { get; set; }

        public string UfNaturalidade { get; set; }

        public string CidadeNaturalidade { get; set; }

        public string DescricaoNaturalidadeEstrangeira { get; set; }

        private string _nomeMae;
        [SMCMapForceFromTo]
        public string NomeMae 
        {
            get { return string.IsNullOrEmpty(_nomeMae) ? "---" : _nomeMae; }
            set { _nomeMae = value; } 
        }

        private string _nomePai;
        [SMCMapForceFromTo]
        public string NomePai 
        {
            get { return string.IsNullOrEmpty(_nomePai) ? "---" : _nomePai; }
            set { _nomePai = value; } 
        }

        public string Email { get; set; }

        private AddressList _endereco;
        [SMCMapForceFromTo]
        public AddressList Enderecos 
        {
            get { return this._endereco; }

            set
            {
                this._endereco = value;
                foreach (var endereco in this._endereco)
                {
                    endereco.Complemento = !string.IsNullOrEmpty(endereco.Complemento) ? endereco.Complemento : "---";
                    this.DescPaisSelecionado = endereco.DescPaisSelecionado;
                }
            }
        }

        public string DescPaisSelecionado { get; set; }

        [SMCMapForceFromTo]
        public List<TelefoneViewModel> Telefones { get; set; }
        
        [SMCMapForceFromTo]
        public List<EnderecoEletronicoViewModel> EnderecosEletronicos{ get; set; }

        public ProcessoCamposInscritoViewModel CamposInscritoVisiveis { get; set; }

    }
}