using SMC.Framework.Fake;
using System;
using System.Collections.Generic;
using System.Reflection;
using SMC.Framework.Model;
using SMC.GPI.Inscricao.Models;

namespace SMC.GPI.Inscricao.Fake
{
    public class ProcessosListaStrategy : SMCFakeStrategyBase
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
            return type == typeof(SMCPagerModel<ProcessoAbertoListaViewModel>);
        }  

        protected override bool CheckMethod(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(SMCPagerModel<ProcessoAbertoListaViewModel>);
        }           

        protected override object CreateMethod(MethodInfo methodInfo)
        {
            return Gerar();
        }

        protected override object CreateType(Type type)
        {
            return Gerar();
        }

        private SMCPagerModel<ProcessoAbertoListaViewModel> Gerar()
        {
            List<ProcessoAbertoListaViewModel> lista = new List<ProcessoAbertoListaViewModel>();
            var total = new Random().Next(2,10);
            for (int i = 0; i < total; i++)
            {
                var proc = new ProcessoAbertoListaViewModel
                {
                    DescricaoProcesso = SMCFakeHelper.RandomListElement(TITULO_PROCESSO),
                    DescricaoComplementarProcesso = SMCFakeHelper.RandomListElement(DESCRICAO_COMPLEMENTAR_PROCESSO),
                    UrlInformacaoComplementar = SMCFakeHelper.RandomListElement(URL_COMPLEMENTAR),
                    DataInicioEtapa = new DateTime(2015, 06, 15, 8, 0, 0),
                    DataFimEtapa = new DateTime(2015, 07, 31, 20, 0, 0),
                    Grupos = new List<GrupoOfertaProcessoListaVewModel>()
                };
                var j = new Random().Next(1,5);
                for (int a = 0; a < j; a++)
			    {
                    var item = new GrupoOfertaProcessoListaVewModel
                    {
                        NomeGrupo = SMCFakeHelper.RandomListElement(GRUPOS_OFERTA_PROCESSO),                        
                    };
                    proc.Grupos.Add(item);
			    }
                lista.Add(proc);
            }


            return new SMCPagerModel<ProcessoAbertoListaViewModel>(lista, new SMCPageSetting 
            {
                PageSize = 3,
                Total = lista.Count
            });
        }

        public static string[] TITULO_PROCESSO = { "Admissão de novos alunos - Colégio Santa Maria/2016",
                                                     "Concurso Correrios/2015", "Processo Seletivo IEC - 1/2015",
                                                 "Processo Seletivo Doutorado Ciências Sociais - PUC Minas/2014"};

        public static string[] GRUPOS_OFERTA_PROCESSO = { "Master", "PDP", "MBA", "Curta Duração", "Aperfeiçoamento" };

        public static string[] DESCRICAO_OFERTA_INSCRICAO = { "Mestrado em Ciência",
                                                     "Doutorado em letras",
                                                 "Mestrado em história da arte"};

        public static string[] DESCRICAO_COMPLEMENTAR_PROCESSO = { "Processo para admissão de novos calouros para os colégios Santa Maria para o ano de 2016. Podem se inscrever todas as Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna." };

        public static string[] URL_COMPLEMENTAR = { "www.pucminas.br", string.Empty, string.Empty };
    }
}