using SMC.Framework.Fake;
using System;
using System.Collections.Generic;
using System.Reflection;
using SMC.Framework.Model;
using SMC.GPI.Inscricao.Models;

namespace SMC.GPI.Inscricao.Fake
{
    public class InscricoesListaStrategy : SMCFakeStrategyBase
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
            return type == typeof(SMCPagerModel<InscricaoListaViewModel>);
        }  

        protected override bool CheckMethod(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(SMCPagerModel<InscricaoListaViewModel>);
        }

        protected override object CreateMethod(MethodInfo methodInfo)
        {
            return Gerar();
        }

        protected override object CreateType(Type type)
        {
            return Gerar();
        }

        private SMCPagerModel<InscricaoListaViewModel> Gerar()
        {
            List<InscricaoListaViewModel> lista = new List<InscricaoListaViewModel>();
            var total = new Random().Next(2,10);
            for (int i = 0; i < total; i++)
            {
                var inscr = new InscricaoListaViewModel
                {
                    DescricaoProcesso = SMCFakeHelper.RandomListElement(TITULO_PROCESSO),
                };
                var j = new Random().Next(1,5);
                for (int a = 0; a < j; a++)
			    {
                    var item = new InscricaoProcessoItemViewModel
                    {
                        DescricaoOfertas = new List<string>(),
                        DescricaoSituacaoAtual = SMCFakeHelper.RandomListElement(SITUACOES)
                    };
                    item.DescricaoOfertas.Add(SMCFakeHelper.RandomListElement(DESCRICAO_OFERTA_INSCRICAO));
                    inscr.Inscricoes.Add(item);
			    }
                lista.Add(inscr);
            }


            return new SMCPagerModel<InscricaoListaViewModel>(lista,new SMCPageSetting 
            {
                PageSize = 3,
                Total = lista.Count
            });
        }

        public static string[] TITULO_PROCESSO = { "Admissão de novos alunos - Colégio Santa Maria/2015",
                                                     "Concurso Correrios/2015", "Processo Seletivo IEC - 1/2015",
                                                 "Processo Seletivo Doutorado Ciências Sociais - PUC Minas/2014"};

        public static string[] GRUPOS_OFERTA_PROCESSO = { "Master", "PDP", "MBA", "Curta Duração", "Aperfeiçoamento" };

        public static string[] DESCRICAO_OFERTA_INSCRICAO = { "Mestrado em Ciência",
                                                     "Doutorado em letras",
                                                 "Mestrado em história da arte"};

        public static string[] SITUACOES = { "Iniciada", "Finalizada", "Confimada", "Deferida", "Indeferida" };

    }
}