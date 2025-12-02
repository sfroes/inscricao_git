using SMC.Framework.Fake;
using SMC.Framework.Model;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Fake
{
	public class ConsultaInscritoStrategy : SMCFakeStrategyBase
	{
		/// <summary>
		/// Define a prioridade da estratégia de fake
		/// </summary>
		public override int Priority
		{
			get { return 99; }
		}


		protected override bool CheckType(Type type)
		{
			return type == typeof(InscricaoViewModel);
		}

		protected override object CreateType(Type type)
		{
			//return Gerar();
            return null;
		}

        //FIX: CONSERTAR ISSO

        //private InscricaoViewModel Gerar()
        //{
        //    var pagina = new InscricaoViewModel()
        //    {
        //        SeqInscricao = 1,

        //        CabecalhoProcesso = new ProcessoCabecalhoViewModel()
        //        {
        //            DescricaoTipoProcesso = SMCFakeHelper.RandomListElement(FakeConfig.TIPO_PROCESSO),
        //            Descricao = SMCFakeHelper.RandomListElement(FakeConfig.DESCRICAO_PROCESSO),
        //            NomeCliente = SMCFakeHelper.NomeCompleto()
        //        },
        //        GrupoOferta = SMCFakeHelper.RandomListElement(FakeConfig.GRUPO_OFERTA),
        //        Ofertas = new List<OfertaInscricaoViewModel>()
        //        {
        //            new OfertaInscricaoViewModel()
        //            {
        //                NumeroOpcao = 1,
        //                DescricaoOferta = SMCFakeHelper.RandomListElement(FakeConfig.OFERTAS)
        //            },
        //            new OfertaInscricaoViewModel()
        //            {
        //                NumeroOpcao = 2,
        //                DescricaoOferta = SMCFakeHelper.RandomListElement(FakeConfig.OFERTAS)
        //            }
        //        },
        //        Nome = "Camila Campos Duarte Souza",
        //        NomeSocial = "--",
        //        DataNascimento = DateTime.Now,
        //        Sexo = DadosMestres.Common.PES.Sexo.Masculino,
        //        EstadoCivil = DadosMestres.Common.PES.EstadoCivil.Solteiro,
        //        Cpf = "123456",
        //        NumeroIdentidade = "123",
        //        OrgaoEmissorIdentidade = "SSP",
        //        UfIdentidade = "MG",
        //        NumeroPassaporte = "--",
        //        Nacionalidade = DadosMestres.Common.PES.TipoNacionalidade.Brasileira,
        //        PaisOrigem = "Brasil",
        //        UfNaturalidade = "Minas Gerais",
        //        CidadeNaturalidade = "Belo Horizonte",
        //        NomePai = "Carlos jose",
        //        NomeMae = "Maria carla da silva",
        //        Endereco = new EnderecoViewModel()
        //        {
        //            AceitaEnderecoEstrangeiro = false,
        //            Enderecos = new List<InformacoesEnderecoViewModel>() 
        //            { 
        //                new InformacoesEnderecoViewModel 
        //                { 
        //                    SeqEndereco = 1,
        //                    SeqCidade = 1,		
        //                    Bairro = "Floresta",
        //                    Cep = "35501219",
        //                    Cidade = "Belo Horizonte",
        //                    Estado = "MG",
        //                    Numero = "540",
        //                    Complemento = "-",
        //                    DescPaisSelecionado = "asdf",
        //                    Logradouro = "Rua XY",
        //                    SeqPais = 1,
        //                    SeqTipoEndereco = 1,
        //                    TiposEnderecos =  new List<SMCDatasourceItem>
        //                    {
										
        //                    }
        //                }
        //            },
        //            TiposEnderecos = new List<SMCDatasourceItem>()
        //            {

        //            }
        //        },
        //        CodigoAutorizacao = "VW 2015",
        //        OutrosEnderecosEletronicos = new List<EnderecoEletronicoViewModel>()
        //        {							
        //            new EnderecoEletronicoViewModel { SeqEnderecoEletronico = 1, Descricao = "face/face", TipoEnderecoEletronico = DadosMestres.Common.PES.TipoEnderecoEletronico.Facebook },
        //            new EnderecoEletronicoViewModel { SeqEnderecoEletronico = 2, Descricao = "testes@Twitter", TipoEnderecoEletronico = DadosMestres.Common.PES.TipoEnderecoEletronico.Twitter }
        //        },
        //        Telefones = new List<TelefoneViewModel>()
        //        {
        //            new TelefoneViewModel { CodigoArea = 31, Numero = "33456754", Seq = 1, CodigoPais = 55, TipoTelefone = 1}
        //        },
        //        Email = SMCFakeHelper.Email()
        //    };

        //    var documentos = new List<DocumentoInscricaoViewModel>() 
        //    {
        //        new DocumentoInscricaoViewModel()
        //        {
					
        //            Seq=SMCFakeHelper.Random<long>(1, 999999999),
        //            Tipo = SMCFakeHelper.RandomListElement(FakeConfig.TIPO_DOCUMENTO),
        //            Descricao = SMCFakeHelper.RandomListElement(FakeConfig.DESCRICAO_DOCUMENTO),
        //            Arquivo = SMCFakeHelper.RandomListElement(FakeConfig.ARQUIVO_DOCUMENTO),
        //            DataEntrega = DateTime.Now,
        //            FormaEntrega = (FormaEntregaDocumento)SMCFakeHelper.Random<short>(1,3),
        //            VersaoDocumento = (VersaoDocumento)SMCFakeHelper.Random<short>(1,3)
        //        },
        //        new DocumentoInscricaoViewModel()
        //        {
        //            Seq = SMCFakeHelper.Random<long>(1, 999999999),
        //            Tipo = SMCFakeHelper.RandomListElement(FakeConfig.TIPO_DOCUMENTO),
        //            Descricao = SMCFakeHelper.RandomListElement(FakeConfig.DESCRICAO_DOCUMENTO),
        //            Arquivo = SMCFakeHelper.RandomListElement(FakeConfig.ARQUIVO_DOCUMENTO),
        //            DataEntrega = DateTime.Now.AddDays(-8),
        //            FormaEntrega = (FormaEntregaDocumento)SMCFakeHelper.Random<short>(1,3),
        //            VersaoDocumento = (VersaoDocumento)SMCFakeHelper.Random<short>(1,3)
        //        }
        //    };

        //    pagina.Documentos = new SMCPagerModel<DocumentoInscricaoViewModel>(documentos);

        //    var titulos = new List<TituloInscricaoViewModel>()
        //    {
        //        new TituloInscricaoViewModel
        //        {
        //            Seq = SMCFakeHelper.Random<long>(1, 999999999),
        //            Situacao = "Baixado",
        //            Titulo = SMCFakeHelper.Random<long>(1, 999999999).ToString(),
        //            Valor = SMCFakeHelper.RandomListElement(FakeConfig.VALOR),
        //            DataImpressaoBoleto = DateTime.Now.AddDays(-5),
        //            DataPagamento = DateTime.Now.AddDays(-2),
        //            DataVencimentto = DateTime.Now
        //        }
				
        //    };

        //    pagina.Titulos = new SMCPagerModel<TituloInscricaoViewModel>(titulos);

        //    return pagina;
        //}
	}
}