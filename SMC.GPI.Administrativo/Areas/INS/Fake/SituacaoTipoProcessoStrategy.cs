using SMC.Framework.Fake;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Fake
{
    public class SituacaoTipoProcessoStrategy : SMCFakeStrategyBase
    {
        /// <summary>
        /// Define a prioridade da estratégia de fake.
        /// </summary>
        public override int Priority
        {
            get { return 99; }
        }

        /// <summary>
        /// Define em que situação a estratégia deve ser aplicada
        /// </summary>
        /// <param name="type">Tipo da classe</param>
        /// <returns>TRUE se a estratégia deve ser aplicada. Caso contrário, retorna FALSE.</returns>
        protected override bool CheckType(Type type)
        {
            return !type.IsAbstract && type == typeof(List<TipoProcessoSituacaoViewModel>);
        }

        /// <summary>
        /// Define em que situação a estratégia deve ser aplicada
        /// </summary>
        /// <param name="methodInfo">Informações do método</param>
        /// <returns>TRUE se a estratégia deve ser aplicada. Caso contrário, retorna FALSE.</returns>
        protected override bool CheckMethod(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(List<TipoProcessoSituacaoViewModel>);
        }

        /// <summary>
        /// Retorna uma lista de áreas de conhecimento fixa, já que é impossível gerar a estrutura da árvore de
        /// áreas de conhecimento aleatoriamente.
        /// </summary>
        /// <param name="type">Tipo da classe</param>
        /// <returns>Lista de áreas de conhecimento</returns>
        protected override object CreateType(Type type)
        {
            return CreateType();
        }

        protected override object CreateMethod(System.Reflection.MethodInfo methodInfo)
        {
            return CreateType();
        }

        private static object CreateType()
        {
            var lista = new List<TipoProcessoSituacaoViewModel>();

            for (var i = 0; i <= 6; i++)
            {

                lista.Add(
                    new TipoProcessoSituacaoViewModel()
                    {
                        DescricaoSGF = FakeConfig.DESCRICAO_SITUACAO_TIPO_PROCESSO[i],
                        Descricao = FakeConfig.DESCRICAO_SITUACAO_TIPO_PROCESSO[i]
                    });
            };

            return lista;
        }
    }
}
