using SMC.Inscricoes.Common;
using SMC.Framework.Fake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Model;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.GPI.Inscricao.Models;
using SMC.Localidades.UI.Mvc.Models;
using SMC.Formularios.UI.Mvc.Models;

namespace SMC.GPI.Inscricao.Areas.INS.Fake
{
    public class DadoFormularioStrategy : SMCFakeStrategyBase
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
            return type == typeof(DadoFormularioViewModel)
                 || (type.IsInterface && type == typeof(IDadoFormulario));
        }

        protected override object CreateType(Type type)
        {
            return Gerar();
        }

        private DadoFormularioViewModel Gerar()
        {
            var dadoForm = new DadoFormularioViewModel();
            dadoForm.DadosCampos = new List<DadoCampoViewModel>();
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2264,
                TokenElemento = "PORTADOR_DE_NECESSIDADES_ESPECIAIS",
                Valor = "true"
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2267,
                TokenElemento = "QUAL_NECESSIDADE_ESPECIAL",
                Valor = "Deficiência auditiva"
            });

            //GUID
            var IdCorrelacao = new Guid();
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2247,
                TokenElemento = "DOCUMENTACAO",
                Valor = IdCorrelacao.ToString()
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2248,
                TokenElemento = "DOCUMENTACAO_TITULO_DE_ELEITOR",
                Valor = "4562487537",
                IdCorrelacao = null
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2249,
                TokenElemento = "DOCUMENTACAO_ZONA",
                Valor = "56",
                IdCorrelacao = null
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2250,
                TokenElemento = "DOCUMENTACAO_SESSAO",
                Valor = "23",
                IdCorrelacao = null
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2253,
                TokenElemento = "DOCUMENTACAO_DOCUMENTO_MILITAR",
                Valor = "45465455565",
                IdCorrelacao = null
            });

            //GUID
            IdCorrelacao = new Guid();
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2247,
                TokenElemento = "DOCUMENTACAO",
                Valor = IdCorrelacao.ToString()
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2248,
                TokenElemento = "DOCUMENTACAO_TITULO_DE_ELEITOR",
                Valor = "4562487537",
                IdCorrelacao = null
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2249,
                TokenElemento = "DOCUMENTACAO_ZONA",
                Valor = "56",
                IdCorrelacao = null
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2250,
                TokenElemento = "DOCUMENTACAO_SESSAO",
                Valor = "23",
                IdCorrelacao = null
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2253,
                TokenElemento = "DOCUMENTACAO_DOCUMENTO_MILITAR",
                Valor = "45465455565",
                IdCorrelacao = null
            });

            //GUID
            IdCorrelacao = new Guid();
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2268,
                TokenElemento = "FORMACAO_UNIVERSITARIA_CURSOS_DE_GRADUACAO",
                Valor = IdCorrelacao.ToString(),
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2256,
                TokenElemento = "CURSOS_DE_GRADUACAO_CURSO",
                Valor = "Ciência da Computação",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2257,
                TokenElemento = "CURSOS_DE_GRADUACAO_UNIVERSIDADE",
                Valor = "Pontíficia Universidade Católica de Minas Gerais",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2258,
                TokenElemento = "CURSOS_DE_GRADUACAO_DATA_DE_INGRESSO",
                Valor = "19/01/2006",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2259,
                TokenElemento = "CURSOS_DE_GRADUACAO_DATA_DE_CONCLUSAO",
                Valor = "20/12/2011",
                IdCorrelacao = IdCorrelacao
            });

            IdCorrelacao = new Guid();
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2268,
                TokenElemento = "FORMACAO_UNIVERSITARIA_CURSOS_DE_GRADUACAO",
                Valor = IdCorrelacao.ToString(),
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2256,
                TokenElemento = "CURSOS_DE_GRADUACAO_CURSO",
                Valor = "Ciência da Computação",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2257,
                TokenElemento = "CURSOS_DE_GRADUACAO_UNIVERSIDADE",
                Valor = "Pontíficia Universidade Católica de Minas Gerais",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2258,
                TokenElemento = "CURSOS_DE_GRADUACAO_DATA_DE_INGRESSO",
                Valor = "19/01/2006",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2259,
                TokenElemento = "CURSOS_DE_GRADUACAO_DATA_DE_CONCLUSAO",
                Valor = "20/12/2011",
                IdCorrelacao = IdCorrelacao
            });

            //GUID
            IdCorrelacao = new Guid();
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2269,
                TokenElemento = "CURSOS_DE_POSGRADUACAO",
                Valor = IdCorrelacao.ToString(),
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2270,
                TokenElemento = "CURSOS_DE_POSGRADUACAO_AREA",
                Valor = "Tecnologia da Informação",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2271,
                TokenElemento = "CURSOS_DE_POSGRADUACAO_INSTITUICAO",
                Valor = "IEC - Puc Minas",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2272,
                TokenElemento = "CURSOS_DE_POSGRADUACAO_DATA_DE_INGRESSO",
                Valor = "03/03/2012",
                IdCorrelacao = IdCorrelacao
            });

            //GUID
            IdCorrelacao = new Guid();
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2275,
                TokenElemento = "ATIVIDADES_PROFISSIONAIS",
                Valor = IdCorrelacao.ToString(),
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2276,
                TokenElemento = "ATIVIDADES_PROFISSIONAIS_FUNCAO",
                Valor = "Analista de Sistemas",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2277,
                TokenElemento = "ATIVIDADES_PROFISSIONAIS_NOME_DA_INSTITUICAO",
                Valor = "Fake sistemas",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2278,
                TokenElemento = "ATIVIDADES_PROFISSIONAIS_HORARIO_DE_TRABALHO",
                Valor = "8:00 as 17:00",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2279,
                TokenElemento = "ATIVIDADES_PROFISSIONAIS_MUNICIPIO",
                Valor = "Belo Horizonte",
                IdCorrelacao = IdCorrelacao
            });
            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2280,
                TokenElemento = "ATIVIDADES_PROFISSIONAIS_POSSUI_VINCULO_EMPREGATICIO",
                Valor = "Sim",
                IdCorrelacao = IdCorrelacao
            });

            dadoForm.DadosCampos.Add(new DadoCampoViewModel
            {
                SeqElemento = 2281,
                TokenElemento = "CONHECIMENTO_DO_PROGRAMA",
                Valor = "Divulgação em ônibus"
            });
            return dadoForm;
        }
    }
}