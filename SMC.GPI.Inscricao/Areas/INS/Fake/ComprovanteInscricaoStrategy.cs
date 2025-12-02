using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Formularios.UI.Mvc.Model;
using SMC.Framework.Fake;
using SMC.Framework.Model;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Inscricao.Areas.INS.Fake
{
    public class ComprovanteInscricaoStrategy : SMCFakeStrategyBase
    {
        public static string[] OFERTAS_CONCATENADAS_CONFIRMACAO_DADOS_INSCRICAO = { "1ª Opção => Coração Eucarístico => Mestrado em Informática => Presencial => Manhã => MBA Executivo em Estratégia e Negócios.",
                                                                                    "2ª Opção => Praça da Liberdade => Mestrado em Informática => Semipresencial => Manhã => MBA Executivo em Finanças.",
                                                                                    "3ª Opção => Praça da Liberdade => Mestrado em Informática => Presencial => Tarde => MBA Executivo em Administração."};

        /// <summary>
        /// Define a prioridade da estratégia de fake
        /// </summary>
        public override int Priority
        {
            get { return 99; }
        }

        protected override bool CheckType(Type type)
        {
            return type == typeof(PaginaComprovanteInscricaoViewModel);
        }

        protected override object CreateType(Type type)
        {
            return Gerar();
        }

        private PaginaComprovanteInscricaoViewModel Gerar()
        {
            var pagina = new PaginaComprovanteInscricaoViewModel();

            pagina.SeqInscricao = SMCFakeHelper.Random<long>(1, 999999999);

            pagina.Titulo = "Comprovante";

            pagina.CodigosAutorizacao = new List<InscricaoCodigoAutorizacaoViewModel>()
            {
                new InscricaoCodigoAutorizacaoViewModel(){ Codigo = SMCFakeHelper.RandomListElement(new string[]{"Fiat 2014"})},
            };

            var listaOfertas = new List<InscricaoOfertaListaViewModel>()
            {
                new InscricaoOfertaListaViewModel()
                {
                    NumeroOpcao = 1,
                    DescricaoCompleta = "Mestrado ABC"
                },
                new InscricaoOfertaListaViewModel()
                {
                    NumeroOpcao = 1,
                    DescricaoCompleta = "Mestrado BCD"
                },
            };
            pagina.Ofertas = listaOfertas;

            pagina.DadosInscrito.Nome = SMCFakeHelper.NomeCompleto();
            pagina.DadosInscrito.NomeSocial = SMCFakeHelper.NomeCompleto();
            pagina.DadosInscrito.DataNascimento = SMCFakeHelper.Data();
            pagina.DadosInscrito.Sexo = SMCFakeHelper.Random<Sexo>(1, 2);
            pagina.DadosInscrito.EstadoCivil = SMCFakeHelper.Random<EstadoCivil>();
            pagina.DadosInscrito.Cpf = SMCFakeHelper.CPF();
            pagina.DadosInscrito.NumeroIdentidade = SMCFakeHelper.RandomListElement(FakeConfig.NUMERO_RG);
            pagina.DadosInscrito.OrgaoEmissorIdentidade = SMCFakeHelper.RandomListElement(FakeConfig.ORGAO_EMISSOR_RG);
            pagina.DadosInscrito.UfIdentidade = SMCFakeHelper.UFSigla();
            pagina.DadosInscrito.NumeroPassaporte = SMCFakeHelper.RandomListElement(FakeConfig.NUMERO_PASSAPORTE);
            pagina.DadosInscrito.Nacionalidade = TipoNacionalidade.Brasileira;
            pagina.DadosInscrito.PaisNacionalidade = SMCFakeHelper.Pais();
            pagina.DadosInscrito.UfNaturalidade = SMCFakeHelper.UFSigla();
            pagina.DadosInscrito.CidadeNaturalidade = SMCFakeHelper.Cidade();
            pagina.DadosInscrito.NomePai = SMCFakeHelper.NomeCompleto();
            pagina.DadosInscrito.NomeMae = SMCFakeHelper.NomeCompleto();

            pagina.DadosInscrito.Email = SMCFakeHelper.RandomListElement(FakeConfig.ENDERECOS_ELETRONICOS);

            pagina.DadosInscrito.EnderecosEletronicos = new List<EnderecoEletronicoViewModel>()
            {
                new EnderecoEletronicoViewModel()
                {
                    Seq = SMCFakeHelper.Random<long>(1, 999999999),
                    TipoEnderecoEletronico = SMCFakeHelper.Random<short>(2, 4),
                    Descricao = SMCFakeHelper.RandomListElement(FakeConfig.ENDERECOS_ELETRONICOS)
                },
                new EnderecoEletronicoViewModel()
                {
                    Seq = SMCFakeHelper.Random<long>(1, 999999999),
                    TipoEnderecoEletronico = SMCFakeHelper.Random<short>(2, 4),
                    Descricao = SMCFakeHelper.RandomListElement(FakeConfig.ENDERECOS_ELETRONICOS)
                }
            };

            var documentos = new List<InscricaoDocumentoListaViewModel>()
            {
                new InscricaoDocumentoListaViewModel()
                {
                    Seq=SMCFakeHelper.Random<long>(1, 999999999),
                    DescricaoTipoDocumento = SMCFakeHelper.RandomListElement(FakeConfig.TIPO_DOCUMENTO),
                    DescricaoArquivoAnexado = SMCFakeHelper.RandomListElement(FakeConfig.DESCRICAO_DOCUMENTO),
                    NomeArquivoAnexado = SMCFakeHelper.RandomListElement(FakeConfig.ARQUIVO_DOCUMENTO)
                },
                new InscricaoDocumentoListaViewModel()
                {
                    Seq=SMCFakeHelper.Random<long>(1, 999999999),
                    DescricaoTipoDocumento = SMCFakeHelper.RandomListElement(FakeConfig.TIPO_DOCUMENTO),
                    DescricaoArquivoAnexado = SMCFakeHelper.RandomListElement(FakeConfig.DESCRICAO_DOCUMENTO),
                    NomeArquivoAnexado = SMCFakeHelper.RandomListElement(FakeConfig.ARQUIVO_DOCUMENTO)
                }
            };

            pagina.Documentos = new SMCPagerModel<InscricaoDocumentoListaViewModel>(documentos);

            //Instruções
            pagina.Secoes.Add(new TemplateSecaoPaginaTextoViewModel()
            {
                Token = TOKENS.SECAO_INSTRUCOES,
                Texto = "<p><b>INSTRUÇÕES</b></p><span>Esta página é o seu Comprovante de Solicitação de Inscrição. Imprima duas cópias e envie uma junto com sua documentação para o IEC.</span>"
            });

            //Controle Secretaria
            pagina.Secoes.Add(new TemplateSecaoPaginaTextoViewModel()
            {
                Token = TOKENS.SECAO_CONTROLE_SECRETARIA,
                Texto = "<p>Nº de controle interno: ___________________________</p><p>Boleto:  (  ) Pago     (  ) Não pago</p><p>Atendente: _______________________</p><p>Data: _____/_____/________</p><p>Baixa: _______________________</p><p>Relatório: _________________________</p>"
            });

            // Foto
            pagina.Secoes.Add(new TemplateSecaoPaginaTextoViewModel()
            {
                Token = TOKENS.SECAO_FOTO,
                Texto = "<p>foto 3 x 4</p>"
            });

            //Rodapé
            pagina.Secoes.Add(new TemplateSecaoPaginaTextoViewModel()
            {
                Token = "RODAPE",
                Texto = "<p>Declaro ter conhecimento dos procedimentos que deverei executar para efetivar a minha inscrição.</p><p>Declaro, ainda, serem verdadeiras todas as informações fornecidas nesta ficha e em meu currículo.</p><p>____________________________________ _____/_____/________.</p><p>Assinatura</p>"
            });

            // protocolo de entrega
            pagina.Secoes.Add(new TemplateSecaoPaginaTextoViewModel()
            {
                Token = "PROTOCOLO_ENTREGA",
                Texto = ("<span>Observações:</span> " +
                        "<p>Apresentar um documento com foto, juntamente com este protocolo, nos dias das provas.</p>" +
                        "<p>Os locais das provas serão divulgados na secretaria e no endereço eletrônico www.pucminas.br/mestrado_educacao </p>")
            });

            return pagina;
        }
    }
}