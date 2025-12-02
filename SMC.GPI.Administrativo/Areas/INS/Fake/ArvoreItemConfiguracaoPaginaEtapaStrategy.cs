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
    public class ArvoreItemConfiguracaoPaginaEtapaStrategy : SMCFakeStrategyBase
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
            return !type.IsAbstract && type == typeof(List<ArvoreItemConfiguracaoPaginaEtapaViewModel>);
        }

        /// <summary>
        /// Define em que situação a estratégia deve ser aplicada
        /// </summary>
        /// <param name="methodInfo">Informações do método</param>
        /// <returns>TRUE se a estratégia deve ser aplicada. Caso contrário, retorna FALSE.</returns>
        protected override bool CheckMethod(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(List<ArvoreItemConfiguracaoPaginaEtapaViewModel>);
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
            return new List<ArvoreItemConfiguracaoPaginaEtapaViewModel>()
            {
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 1, SeqPai = 0, Descricao = "Instruções Iniciais", Tipo = TipoItemPaginaEtapa.Pagina, PaginaPermiteExibicaoOutrasPaginas = true, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 2, SeqPai = 1, Descricao = "Português", Tipo = TipoItemPaginaEtapa.Idioma, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 3, SeqPai = 2, Descricao = "Instruções", Tipo = TipoItemPaginaEtapa.Secao, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 4, SeqPai = 2, Descricao = "Arquivos", Tipo = TipoItemPaginaEtapa.Arquivo, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 5, SeqPai = 0, Descricao = "Confirmação de Dados", Tipo = TipoItemPaginaEtapa.Pagina, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 6, SeqPai = 5, Descricao = "Português", Tipo = TipoItemPaginaEtapa.Idioma, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 7, SeqPai = 6, Descricao = "Instruções", Tipo = TipoItemPaginaEtapa.Secao, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 8, SeqPai = 0, Descricao = "Seleção de Oferta", Tipo = TipoItemPaginaEtapa.Pagina, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999),PaginaPermiteExibicaoOutrasPaginas=true,PaginaExibeFormulario=true,PaginaObrigatoria=true,PaginaPermiteDuplicar=true},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 9, SeqPai = 8, Descricao = "Português", Tipo = TipoItemPaginaEtapa.Idioma, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},
                new ArvoreItemConfiguracaoPaginaEtapaViewModel() {Seq = 10, SeqPai = 9, Descricao = "Instruções", Tipo = TipoItemPaginaEtapa.Secao, SeqEtapaProcesso = SMCFakeHelper.Random<long>(1,99999), SeqProcesso = SMCFakeHelper.Random<long>(1,99999)},

                
            };
        }
    }
}