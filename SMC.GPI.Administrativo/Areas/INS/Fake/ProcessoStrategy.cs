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
    public class ProcessoStrategy : SMCFakeStrategyBase
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
            return !type.IsAbstract && type == typeof(ProcessoViewModel);
        }

        /// <summary>
        /// Define em que situação a estratégia deve ser aplicada
        /// </summary>
        /// <param name="methodInfo">Informações do método</param>
        /// <returns>TRUE se a estratégia deve ser aplicada. Caso contrário, retorna FALSE.</returns>
        protected override bool CheckMethod(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(ProcessoViewModel);
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
            return new ProcessoViewModel()
            {
                Seq = SMCFakeHelper.Random<int>(1, 99999),
                Descricao = SMCFakeHelper.RandomListElement<string>(FakeConfig.DESCRICAO_PROCESSO),
                MaximoInscricoesPorInscrito = SMCFakeHelper.Random<short>(1, 10),
                Idiomas = new SMCMasterDetailList<IdiomaProcessoViewModel>()
                {
                    new IdiomaProcessoViewModel()
                    {
                        Seq = SMCFakeHelper.Random<int>(1,99999),
                        SeqProcesso = SMCFakeHelper.Random<int>(1,99999),
                        Idioma = Framework.SMCLanguage.Portuguese,
                        DescricaoComplementar = "As inscrições para o Processo de Ingresso 2016...",
                        LabelOferta = "Oferta",
                        LabelCodigoAutorizacao = "Código de Autorização",
                        LabelGrupoOferta = "Grupo de Oferta"
                    },
                    new IdiomaProcessoViewModel()
                    {
                        Seq = SMCFakeHelper.Random<int>(1,99999),
                        SeqProcesso = SMCFakeHelper.Random<int>(1,99999),
                        Idioma = Framework.SMCLanguage.English,
                        DescricaoComplementar = "As inscrições para o Processo de Ingresso 2016...",
                        LabelOferta = "Oferta",
                        LabelCodigoAutorizacao = "Código de Autorização",
                        LabelGrupoOferta = "Grupo de Oferta"
                    },
                },
                SemestreReferencia = SMCFakeHelper.Random<int>(1, 2),
                AnoReferencia = SMCFakeHelper.Random<int>(2015, 2016),
                SeqCliente = SMCFakeHelper.Random<int>(1, 4),
                SeqTemplateProcessoSGF = SMCFakeHelper.Random<int>(1, 4),
                SeqTipoHierarquiaOferta = SMCFakeHelper.Random<int>(1, 4),
                SeqTipoProcesso = SMCFakeHelper.Random<int>(1, 4),
                SeqUnidadeResponsavel = SMCFakeHelper.Random<int>(1, 4),
                NomeContato = SMCFakeHelper.NomeCompleto(),
                DataCancelamento = SMCFakeHelper.Data(),
                DataEncerramento = SMCFakeHelper.Data(),                
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
                }
            };
        }
    }
}