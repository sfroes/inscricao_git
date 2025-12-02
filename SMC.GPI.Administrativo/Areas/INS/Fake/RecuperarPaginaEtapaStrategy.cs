using SMC.Framework.Fake;
using SMC.Framework.Model;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using SMC.Inscricoes.Common.Enums;

namespace SMC.GPI.Administrativo.Areas.INS.Fake
{
    public class RecuperarPaginaEtapaStrategy : SMCFakeStrategyBase
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
            return !type.IsAbstract && type == typeof(RecuperarPaginaEtapaViewModel);
        }

        /// <summary>
        /// Define em que situação a estratégia deve ser aplicada
        /// </summary>
        /// <param name="methodInfo">Informações do método</param>
        /// <returns>TRUE se a estratégia deve ser aplicada. Caso contrário, retorna FALSE.</returns>
        protected override bool CheckMethod(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(RecuperarPaginaEtapaViewModel);
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
            return new RecuperarPaginaEtapaViewModel
            {
                SeqConfiguracaoEtapa = 1,        
                PaginasDisponiveis = new List<SMCDatasourceItem>()
                {
                    new SMCDatasourceItem(){Seq=1,Descricao="Código de Autorização"},
                    new SMCDatasourceItem(){Seq=2,Descricao="Instruções Iniciais"}
                },
               
            };
        }
    }
}