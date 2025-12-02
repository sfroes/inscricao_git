using SMC.Framework.Fake;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Fake
{
    public class TipoHierarquiaOfertaStrategy : SMCFakeStrategyBase
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
            return !type.IsAbstract && type == typeof(TipoHierarquiaOfertaViewModel);
        }

        /// <summary>
        /// Define em que situação a estratégia deve ser aplicada
        /// </summary>
        /// <param name="methodInfo">Informações do método</param>
        /// <returns>TRUE se a estratégia deve ser aplicada. Caso contrário, retorna FALSE.</returns>
        protected override bool CheckMethod(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(TipoHierarquiaOfertaViewModel);
        }

        /// <summary>
        /// Retorna uma lista de áreas de conhecimento fixa, já que é impossível gerar a estrutura da árvore de
        /// áreas de conhecimento aleatoriamente.
        /// </summary>
        /// <param name="type">Tipo da classe</param>
        /// <returns>Lista de áreas de conhecimento</returns>
        protected override object CreateType(Type type)
        {
            return CreateTree();
        }

        protected override object CreateMethod(System.Reflection.MethodInfo methodInfo)
        {
            return CreateTree();
        }

        private static object CreateTree()
        {
            var lista = new List<NoArvoreTipoHierarquiaOfertaViewModel>()
            {
                new NoArvoreTipoHierarquiaOfertaViewModel ( ) { Seq = 1, SeqPai = 0, SeqTipoHierarquiaOferta = 10, Descricao = "Programa" },
                new NoArvoreTipoHierarquiaOfertaViewModel ( ) { Seq = 2, SeqPai = 1, SeqTipoHierarquiaOferta = 10, Descricao = "Curso" },
                new NoArvoreTipoHierarquiaOfertaViewModel ( ) { Seq = 3, SeqPai = 2, SeqTipoHierarquiaOferta = 10, Descricao = "Linha de Pesquisa",HabilitaCadastroOferta = true }
            };

            return new TipoHierarquiaOfertaViewModel()
            {
                Seq = SMCFakeHelper.Random<long>(1,99999),
                Descricao = SMCFakeHelper.RandomListElement(FakeConfig.DESCRICAO_TIPO_HIERARQUIA_OFERTA)                
            };
        }
    }
}
