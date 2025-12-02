using SMC.Framework.Fake;
using System;
using System.Collections.Generic;
using System.Reflection;
using SMC.Framework.Model;
using SMC.GPI.Inscricao.Models;

namespace SMC.GPI.Inscricao.Fake
{
    public class HomeProcessoInscritoStrategy : SMCFakeStrategyBase
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
			return type == typeof(ProcessoHomeViewModel);
        }  

        protected override bool CheckMethod(MethodInfo methodInfo)
        {
			return methodInfo.ReturnType == typeof(ProcessoHomeViewModel);
        }           

        protected override object CreateMethod(MethodInfo methodInfo)
        {
            return Gerar();
        }

        protected override object CreateType(Type type)
        {
            return Gerar();
        }

		private ProcessoHomeViewModel Gerar()
        {
			var pagina = new ProcessoHomeViewModel();

			pagina.SeqProcesso = 1;
			pagina.DescricaoProcesso = SMCFakeHelper.RandomListElement(TITULO_PROCESSO);
			pagina.DescricaoComplementarProcesso = SMCFakeHelper.RandomListElement(DESCRICAO_COMPLEMENTAR_PROCESSO);
			pagina.UrlInformacaoComplementar =  SMCFakeHelper.RandomListElement(URL_COMPLEMENTAR);
            /*
			List<InscricaoInscritoListaViewModel> listaInscricaoDoInscrito = new List<InscricaoInscritoListaViewModel>();
			var total1 = new Random().Next(2, 10);
			for(int i = 0; i < total1; i++)
			{
				var proc = new InscricaoInscritoListaViewModel
				{
					DescricaoOferta = SMCFakeHelper.RandomListElement(DESCRICAO_INSCRICAO),
					SituacaoInscricao = SMCFakeHelper.RandomListElement(SITUACAO_INSCRICAO),
					SeqProcesso = 1
				};
				listaInscricaoDoInscrito.Add(proc);
			}
             * */
            /*
			pagina.InscricoesDoInscrito = new SMCPagerModel<InscricaoInscritoListaViewModel>(listaInscricaoDoInscrito, new SMCPageSetting 
            {
                PageSize = 3,
				Finalizadas = listaInscricaoDoInscrito.Count
            });
            */
            /*
			List<OfertasProcessoListaViewModel> listaOfertasProcesso = new List<OfertasProcessoListaViewModel>();
			var total2 = new Random().Next(2, 10);
			for(int i = 0; i < total2; i++)
			{
				var proc = new OfertasProcessoListaViewModel
				{
					DescricaoGrupoOferta = SMCFakeHelper.RandomListElement(GRUPOS_OFERTA_PROCESSO),
					DataFimConfiguracaoEtapa = new DateTime(2015, 06, 15, 8, 0, 0),
					DataInicioConfiguracaoEtapa = new DateTime(2015, 06, 15, 8, 0, 0), 
					SeqProcesso = 1
				};
				listaOfertasProcesso.Add(proc);
			}*/
            /*
			pagina.InscricoesEmAbertoDoProcesso = new SMCPagerModel<OfertasProcessoListaViewModel>(listaOfertasProcesso, new SMCPageSetting 
            {
                PageSize = 3,
				Finalizadas = listaOfertasProcesso.Count
            }); 
            */
			return pagina;
        }

        public static string[] TITULO_PROCESSO = { "Admissão de novos alunos - Colégio Santa Maria/2016",
                                                     "Concurso Correrios/2015", "Processo Seletivo IEC - 1/2015",
                                                 "Processo Seletivo Doutorado Ciências Sociais - PUC Minas/2014"};

        public static string[] GRUPOS_OFERTA_PROCESSO = { "Master", "PDP", "MBA", "Curta Duração", "Aperfeiçoamento" };

		public static string[] SITUACAO_INSCRICAO = { "Finalizada", "Iniciada" }; 

        public static string[] DESCRICAO_OFERTA_INSCRICAO = { "Mestrado em Ciência",
                                                     "Doutorado em letras",
                                                 "Mestrado em história da arte"};

        public static string[] DESCRICAO_COMPLEMENTAR_PROCESSO = { "Processo para admissão de novos calouros para os colégios Santa Maria para o ano de 2016. Podem se inscrever todas as Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna." };

        public static string[] URL_COMPLEMENTAR = { "www.pucminas.br", string.Empty, string.Empty }; 

		public static string[] DESCRICAO_INSCRICAO = { "Praça da Liberdade / Presencial / Noite / MBA Executivo em Estratégia e Negócios", "Praça da Liberdade / Presencial / Noite / Gestão de Negócios da Moda", "Coração Eucarístico / Mestrado em Informática / Presencial / Cristiane Neri Nobre", "Curta Duração", "Aperfeiçoamento" };
		
    }
}