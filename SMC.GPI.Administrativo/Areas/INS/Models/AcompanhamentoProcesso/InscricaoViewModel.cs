using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class InscricaoViewModel : SMCViewModelBase, ISMCMappable
	{

		public InscricaoViewModel()
		{
            CodigosAutorizacao = new List<string>();
			Documentos = new SMCPagerModel<DocumentoInscricaoViewModel>();
            Titulos = new SMCPagerModel<TituloInscricaoViewModel>();
            DadosInscrito = new DadosInscritoViewModel();
            Formularios = new List<InscricaoDadoFormularioListaViewModel>();
		}

        [SMCHidden]
        public long SeqInscricao { get; set; }
        
        public long? SeqArquivoComprovante { get; set; }

        public long SeqInscrito { get; set; }
		
        public long  SeqProcesso { get; set; }
		
		public string DescricaoGrupoOferta { get; set; }

        public short? NumeroOpcoesDesejadas { get; set; }

        public string Observacao { get; set; }

        public string SituacaoInscrito { get; set; }

        public string TokenSituacaoInscrito { get; set; }

        [SMCHidden]
        public bool OfertaVigente { get; set; }

        [SMCHidden]
        public bool CandidatoComBoletoPago { get; set; }

        [SMCHidden]
        public string Origem { get; set; }

        public string BackURL { get; set; }

        public bool RecebeuBolsa { get; set; }

        public bool BolsaExAluno { get; set; }

        [SMCMapForceFromTo]
        public DadosInscritoViewModel DadosInscrito { get; set; }

        public List<OfertaInscricaoViewModel> Ofertas { get; set; }

        [SMCMapForceFromTo]
		public List<string> CodigosAutorizacao { get; set; }

        [SMCMapForceFromTo]
        public SMCPagerModel<DocumentoInscricaoViewModel> Documentos { get; set; }

        [SMCMapForceFromTo]
        public SMCPagerModel<TituloInscricaoViewModel> Titulos { get; set; }

        [SMCMapForceFromTo]
        public List<InscricaoDadoFormularioListaViewModel> Formularios { get; set; }
		 
    } 
}