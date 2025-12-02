using SMC.Framework.Fake;
using SMC.GPI.Inscricao.Areas.INS.Fake;
using SMC.GPI.Inscricao.Fake;

namespace SMC.GPI.Inscricao
{
    public static class FakeConfig
    {
        public static void RegisterStrategies(SMCFakeStrategyConfiguration strategies)
        {

            #region Localidade
            /*
            strategies.AddForSelect(typeof(ILocalidadesControllerService), "BuscarPaisesSelect")
                     .RandomValues(COUNTRY);

            strategies.AddForSelect(typeof(ILocalidadesControllerService), "BuscarEstadosSelect")
                      .RandomValues(SMCFakeHelper.GenerateStringList(10, SMCFakeHelper.UF));

            strategies.AddForSelect(typeof(ILocalidadesControllerService), "BuscarCidadesSelect")
                      .RandomValues(SMCFakeHelper.GenerateStringList(10, SMCFakeHelper.Cidade));
                      */
            #endregion Selecao OFerta

            #region Tela Inicial

            strategies.Add(new ProcessosListaStrategy());
            strategies.Add(new InscricoesListaStrategy());

            #endregion

			#region Inscrito

			strategies.AddForProperty<string>()
			 .Where(prop => prop.DeclaringType.Name.Contains("InscritoViewModel") && prop.Name == "Titulo")
			 .RandomValues(TITULO_INSCRITO);

			#endregion

            strategies.AddForProperty<long>()
                      .Where(prop => prop.DeclaringType.Name.Equals("ConfiguradorFormularioViewModel") && prop.Name == "SeqFormulario")
                      .SetViewStrategy(@"Inscricao\FormularioInscricao")
                      .RandomValues(28);

            strategies.AddForProperty<long>()
                      .Where(prop => prop.DeclaringType.Name.Equals("ConfiguradorFormularioViewModel") && prop.Name == "SeqVisao")
                      .SetViewStrategy(@"Inscricao\FormularioInscricao")
                      .RandomValues(25);

            strategies.AddForProperty<long>()
                      .Where(prop => prop.DeclaringType.Name.Equals("ConfiguradorFormularioViewModel") && prop.Name == "SeqFormulario")
                      .SetViewStrategy(@"Inscricao\ConfirmarInscricao")
                      .RandomValues(28);

            strategies.AddForProperty<long>()
                      .Where(prop => prop.DeclaringType.Name.Equals("ConfiguradorFormularioViewModel") && prop.Name == "SeqVisao")
                      .SetViewStrategy(@"Inscricao\ConfirmarInscricao")
                      .RandomValues(25);

            strategies.AddForProperty<long>()
                    .Where(prop => prop.DeclaringType.Name.Equals("ConfiguradorFormularioViewModel") && prop.Name == "SeqFormulario")
                    .SetViewStrategy(@"Inscricao\ComprovanteInscricao")
                    .RandomValues(28);

            strategies.AddForProperty<long>()
                      .Where(prop => prop.DeclaringType.Name.Equals("ConfiguradorFormularioViewModel") && prop.Name == "SeqVisao")
                      .SetViewStrategy(@"Inscricao\ComprovanteInscricao")
                      .RandomValues(25);

            
            strategies.Add(new DadoFormularioStrategy());

		}

        #region Lista de Constantes

        public static string[] COUNTRY = { "Argentina", "Bolívia", "Brasil", "Chile", "Colômbia", "Equador", "Guiana Francesa", "Paraguai", "Peru", "Suriname", "Uruguai", "Venezuela" };

        public static string[] ENDERECOS_ELETRONICOS = { "fulano@inscrito.com.br", "beltrano@inscrito.com.br", "sicrano@inscrito.com.br" };

		public static string[] TITULO_INSCRITO = { "Novo Inscrito" };

		public static string[] TIPO_DOCUMENTO = { "Curriculum Vitae", "Comprovação de Currículo", "Carta de Justificativa", "Outros" };

        public static int[] CODIGO_AREA_TELEFONE = { 11, 31, 21, 61 };

        public static string[] ESTADO_CIVIL = { "Casado", "Solteiro", "União Estável" };

        public static string[] NUMERO_RG = { "MG-4.567.234", "MG-2.567.123" };

        public static string[] NUMERO_PASSAPORTE = { "2.34534.123-8", "42.31212.132-18" };

        public static string[] ORGAO_EMISSOR_RG = { "SSP" };

        public static string[] NACIONALIDADE = { "Brasileira", "Chilena", "Argentina" };

        public static string[] DESCRICAO_DOCUMENTO = { "Currículo - Samila Souza", "Certificado do curso de aperfeiçoamento em Inovação em Negócios" };

        public static string[] ARQUIVO_DOCUMENTO = { "Currículo - Samila Souza.pdf", "Certificado.pdf" };
		 
//        public static string[] SEXO = { "Masculino", "Feminino" };

//        public static string[] TITULO_COMPROVANTE_INSCRICAO = { "Comprovante de Solicitação de Inscrição" };

//        public static string[] INSTRUCOES_COMPROVANTE_INSCRICAO = { "<p><b>INSTRUÇÕES</b></p><span>Esta página é o seu Comprovante de Solicitação de Inscrição. Imprima duas cópias e envie uma junto com sua documentação para o IEC.</span>" };

//        public static string[] CONTROLE_SECRETARIA_COMPROVANTE_INSCRICAO = { "<p>Nº de controle interno: ___________________________</p><p>Boleto:  (  ) Pago     (  ) Não pago</p><p>Atendente: _______________________</p><p>Data: _____/_____/________</p><p>Baixa: _______________________</p><p>Relatório: _________________________</p>" };

//        public static string[] RODAPE_COMPROVANTE_INSCRICAO = { "<p>Declaro ter conhecimento dos procedimentos que deverei executar para efetivar a minha inscrição.</p><p>Declaro, ainda, serem verdadeiras todas as informações fornecidas nesta ficha e em meu currículo.</p><p>____________________________________ _____/_____/________.</p><p>Assinatura</p>" };

//        public static string[] TITULO_DADOS_PESSOAIS_COMPROVANTE_INSCRICAO = { "Dados Pessoais" };

//        public static string[] TITULO_CONTATOS_COMPROVANTE_INSCRICAO = { "Contatos" };

//        public static string[] TITULO_GRUPO_OFERTAS_COMPROVANTE_INSCRICAO = { "Grupo de Ofertas", "Conjunto de Ofertas", "Ofertas" };

//        public static string[] TITULO_PROCESSO_COMPROVANTE_INSCRICAO = { "Processo" };

//        public static string[] TITULO_CODIGO_AUTORIZACAO_COMPROVANTE_INSCRICAO = { "Código(s) de Autorização", "Autorizações" };

//        public static string[] TITULO_OFERTAS_COMPROVANTE_INSCRICAO = { "Ofertas", "Ofertas Especiais" };

//        public static string[] TITULO_QUESTIONARIO_SOCIOECONOMICO_COMPROVANTE_INSCRICAO = { "Questionário Socioeconômico" };

//        public static string[] PROCESSO_COMPROVANTE_INSCRICAO = { "Inscrições para 2º semestre de 2014 - IEC Praça da Liberdade" };

//        public static string[] CODIGO_AUTORIZACAO_COMPROVANTE_INSCRICAO = { "Fiat 2014" };

/*        public static string[] TITULO_PROCESSO = { "Admissão de novos alunos - Colégio Santa Maria/2015",
                                                     "Concurso Correrios/2015", "Processo Seletivo IEC - 1/2015",
                                                 "Processo Seletivo Doutorado Ciências Sociais - PUC Minas/2014"};
*/
//        public static string[] GRUPOS_OFERTA_PROCESSO = { "Master","PDP","MBA","Curta Duração","Aperfeiçoamento"};

/*        public static string[] DESCRICAO_PROCESSO = { "Processo seletivo para mestrado e doutorado",
                                                     "Seleção de candidatos a mestrado e doutorado em ciências exatas",
                                                 "2ª seleção anual para alunos de mestrado e doutorado"};
*/
/*        public static string[] DESCRICAO_OFERTA_INSCRICAO = { "Mestrado em Ciência",
                                                     "Doutorado em letras",
                                                 "Mestrado em história da arte"};
*/

        #endregion


    }
}