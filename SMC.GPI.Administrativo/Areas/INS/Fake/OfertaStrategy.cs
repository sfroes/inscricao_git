using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Fake;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Models;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Reflection;

namespace SMC.GPI.Administrativo.Areas.INS.Fake
{
    public class OfertaStrategy : SMCFakeStrategyBase
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
            return !type.IsAbstract && type == typeof(OfertaViewModel);
        }

        /// <summary>
        /// Define em que situação a estratégia deve ser aplicada
        /// </summary>
        /// <param name="methodInfo">Informações do método</param>
        /// <returns>TRUE se a estratégia deve ser aplicada. Caso contrário, retorna FALSE.</returns>
        protected override bool CheckMethod(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(OfertaViewModel);
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
            return new OfertaViewModel()
            {
                Seq = SMCFakeHelper.Random<int>(1, 99999),
                Nome = SMCFakeHelper.RandomListElement<string>(FakeConfig.OFERTA),
                SeqItemHierarquiaOferta = SMCFakeHelper.Random<long>(1, 5),
                SeqGrupoOferta = SMCFakeHelper.Random<long>(1, 3),
                SeqPai = SMCFakeHelper.Random<long>(1, 3),
                DataInicio = SMCFakeHelper.Data(),
                DataFim = SMCFakeHelper.Data(),
                NumeroVagas = SMCFakeHelper.Random<int>(1, 100),
                CodigosAutorizacao = new SMCMasterDetailList<CodigoAutorizacaoDetalheViewModel>()
                {
                    new CodigoAutorizacaoDetalheViewModel()
                    {
                        Seq = SMCFakeHelper.Random<int>(1,99999),
                        SeqCodigoAutorizacao = SMCFakeHelper.Random<long>(1,3)
                    },
                    new CodigoAutorizacaoDetalheViewModel()
                    {
                        Seq = SMCFakeHelper.Random<int>(1,99999),
                        SeqCodigoAutorizacao = SMCFakeHelper.Random<long>(1,3)
                    },
                    new CodigoAutorizacaoDetalheViewModel()
                    {
                        Seq = SMCFakeHelper.Random<int>(1,99999),
                        SeqCodigoAutorizacao = SMCFakeHelper.Random<long>(1,3)
                    },
                },
                NomeResponsavel = SMCFakeHelper.NomeCompleto(),                
                EnderecosEletronicos = new SMCMasterDetailList<EnderecoEletronicoViewModel>()
                {
                    new EnderecoEletronicoViewModel()
                    {
                        TipoEnderecoEletronico = TipoEnderecoEletronico.Email,
                        SeqEnderecoEletronico = SMCFakeHelper.Random<int>(1,99999),
                        Descricao = "email@mail.com.br"
                    },
                    new EnderecoEletronicoViewModel()
                    {
                        TipoEnderecoEletronico = TipoEnderecoEletronico.Facebook,
                        SeqEnderecoEletronico = SMCFakeHelper.Random<int>(1,99999),
                        Descricao = "www.facebook.com/endereco"
                    },
                },
                DataCancelamento = SMCFakeHelper.Data(),
                MotivoCancelamento = SMCFakeHelper.RandomListElement<string>(FakeConfig.MOTIVO_CANCELAMENTO_OFERTA),
                Taxas = new SMCMasterDetailList<TaxaOfertaViewModel>() { new TaxaOfertaViewModel() { SeqTaxa = 1 } }
            };
        }
    }
}