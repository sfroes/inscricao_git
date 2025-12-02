using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoDadoFormularioDomainService : InscricaoContextDomain<InscricaoDadoFormulario>
    {
        #region DomainService

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private TipoProcessoDomainService TipoProcessoDomainService => Create<TipoProcessoDomainService>();

        #endregion DomainService

        #region Services

        private IElementoService ElementoService => Create<IElementoService>();

        #endregion Services

        /// <summary>
        /// Salva os dados do formulário
        /// </summary>
        /// <param name="inscricaoDadoFormulario">Dados do formulário para salvar</param>
        /// <returns></returns>
        public long SalvarInscricaoDadoFormulario(InscricaoDadoFormulario inscricaoDadoFormulario)
        {
            // Verifica as regras para avançar na inscrição
            var includesInscricao = IncludesInscricao.ConfiguracaoEtapa |
                                    IncludesInscricao.ConfiguracaoEtapa_EtapaProcesso;
            SMCSeqSpecification<Inscricao> spec = new SMCSeqSpecification<Inscricao>(inscricaoDadoFormulario.SeqInscricao);
            Inscricao inscricao = InscricaoDomainService.SearchByKey(spec, includesInscricao);
            InscricaoDomainService.VerificarRegrasAvancarInscricao(inscricao);

            // Verifica se o processo possui a regra de calculo de bolsa social.
            var tipoProcesso = InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(inscricaoDadoFormulario.SeqInscricao),
                                                        x => x.Processo.TipoProcesso);
            if (TipoProcessoDomainService.PossuiConsistencia(tipoProcesso, Common.Areas.INS.Enums.TipoConsistencia.CalculoBolsaSocial))
            {
                ExecutaRegraBolsaSocial(inscricaoDadoFormulario);
            }

            // Salva o dado formulário
            this.SaveEntity(inscricaoDadoFormulario);

            return inscricaoDadoFormulario.Seq;
        }

        /// <summary>
        /// Salva os dados do formulário
        /// </summary>
        /// <param name="inscricaoDadoFormulario">Dados do formulário para salvar</param>
        /// <returns>Sequencial do formulario de impacto</returns>
        public long SalvarInscricaoDadoFormularioImpacto(InscricaoDadoFormulario inscricaoDadoFormulario)
        {
            //formulario de impacto não leva em consideração Idioma
            inscricaoDadoFormulario.SeqConfiguracaoEtapaPaginaIdioma = null;

            // Salva o dado formulário
            this.SaveEntity(inscricaoDadoFormulario);

            return inscricaoDadoFormulario.Seq;
        }

        /// <summary>
        /// Busca os dados formulários para uma isncrião ordenados
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Lista de formularios de uma inscrição</returns>
        public List<InscricaoDadoFormularioVO> BuscarInscricaoDadosFormulario(long seqInscricao)
        {
            var spec = new InscricaoDadoFormularioFilterSpecification { SeqInscricao = seqInscricao };
            var inscricoesDadoFormulario = this.SearchProjectionBySpecification
                (spec, x => new InscricaoDadoFormularioVO
                {
                    Seq = x.Seq,
                    SeqFormulario = x.SeqFormulario,
                    SeqInscricao = x.SeqInscricao,
                    SeqVisao = x.SeqVisao,
                    Editable = x.ConfiguracaoEtapaPaginaIdioma.SeqVisaoGestaoSGF != null,

                    Ordem = x.Inscricao.ConfiguracaoEtapa.Paginas.Where(
                            p => p.Idiomas.Any(i => i.Idioma == x.Inscricao.Idioma
                            && i.SeqFormularioSGF == x.SeqFormulario && (i.SeqVisaoSGF == x.SeqVisao || i.SeqVisaoGestaoSGF == x.SeqVisao))).FirstOrDefault().Ordem,

                    TituloFormulario = x.Inscricao.ConfiguracaoEtapa.Paginas.Where(
                                p => p.Idiomas.Any(i => i.Idioma == x.Inscricao.Idioma
                                && i.SeqFormularioSGF == x.SeqFormulario && (i.SeqVisaoSGF == x.SeqVisao || i.SeqVisaoGestaoSGF == x.SeqVisao))).FirstOrDefault()
                                .Idiomas.Where(y => y.Idioma == x.Inscricao.Idioma && y.SeqFormularioSGF == x.SeqFormulario &&
                                    (y.SeqVisaoSGF == x.SeqVisao || y.SeqVisaoGestaoSGF == x.SeqVisao)).FirstOrDefault().Titulo,

                    SeqVisaoConfigucacao = x.ConfiguracaoEtapaPaginaIdioma.SeqVisaoSGF,
                    SeqVisaoGestaoConfiguracao = x.ConfiguracaoEtapaPaginaIdioma.SeqVisaoGestaoSGF
                }).OrderBy(o=> o?.Ordem);
               //

            var retorno = new List<InscricaoDadoFormularioVO>();
            // Agrupa os resultados pelo seq do formulário, para ver se existem mais de uma visão para o formulário.
            var grupos = inscricoesDadoFormulario.GroupBy(g => g.SeqFormulario);
            foreach (var grupo in grupos)
            {
                // Se houver mais de um formulário de mesmo tipo, procura pela visão de gestão
                if (grupo.Count() > 1)
                {
                    // Busca pela inscricao_dado_formulario que possua a visao de gestão.
                    var idf = grupo.FirstOrDefault(f => f.SeqVisao == f.SeqVisaoGestaoConfiguracao);
                    // Se nenhuma for encontrada, pega a primeira inscricao_dado_formulario e modifica a visão para a gestão para exibir corretamente os valores.
                    if (idf == null && idf.SeqVisaoGestaoConfiguracao.HasValue)
                    {
                        idf = grupo.First();
                        idf.SeqVisao = idf.SeqVisaoGestaoConfiguracao.Value;
                    }

                    retorno.Add(idf);
                }
                else
                {
                    // Se existir apenas um item no grupo, é a resposta do usuário. Modifica a visão para exibir a visão de gestão.
                    var idf = grupo.Single();
                    if (idf.SeqVisaoGestaoConfiguracao.HasValue)
                    {
                        idf.SeqVisao = idf.SeqVisaoGestaoConfiguracao.Value;
                    }
                    retorno.Add(idf);
                }
            }
            return retorno;
        }

        public void AlterarFormularioInscricao(InscricaoDadoFormulario formulario)
        {
            var dadoBanco = SearchByKey(new SMCSeqSpecification<InscricaoDadoFormulario>(formulario.Seq), x => x.ConfiguracaoEtapaPaginaIdioma);

            // Verifica se o formulário possui uma visão de gestão ou não.
            if (dadoBanco.ConfiguracaoEtapaPaginaIdioma.SeqVisaoGestaoSGF.HasValue && dadoBanco.SeqVisao != dadoBanco.ConfiguracaoEtapaPaginaIdioma.SeqVisaoGestaoSGF)
            {
                // Cria uma nova inscricao_dado_formulario com os valores das novas respostas
                dadoBanco.Seq = 0;
                dadoBanco.SeqVisao = dadoBanco.ConfiguracaoEtapaPaginaIdioma.SeqVisaoGestaoSGF.Value;
            }
            dadoBanco.DadosCampos = formulario.DadosCampos;

            // Verifica se o processo possui a regra de calculo de bolsa social.
            var tipoProcesso = SearchProjectionByKey(new SMCSeqSpecification<InscricaoDadoFormulario>(formulario.Seq),
                                                        x => x.Inscricao.Processo.TipoProcesso);
            if (TipoProcessoDomainService.PossuiConsistencia(tipoProcesso, Common.Areas.INS.Enums.TipoConsistencia.CalculoBolsaSocial))
            {
                ExecutaRegraBolsaSocial(dadoBanco);
            }

            SaveEntity(dadoBanco);
        }

        private void ExecutaRegraBolsaSocial(InscricaoDadoFormulario inscricaoDadoFormulario)
        {
            var qtdGrupoFamiliar = inscricaoDadoFormulario.DadosCampos.Where(f => f.UidCorrelacao.HasValue).GroupBy(g => g.UidCorrelacao).Count();
            var campoTotalRendaBruta = inscricaoDadoFormulario.DadosCampos.FirstOrDefault(f => f.Token == TOKENS_BOLSA_SOCIAL.TOTAL_RENDA_BRUTA);
            var campoRendaPerCapta = inscricaoDadoFormulario.DadosCampos.FirstOrDefault(f => f.Token == TOKENS_BOLSA_SOCIAL.RENDA_PER_CAPTA);
            var campoPercentualMaximo = inscricaoDadoFormulario.DadosCampos.FirstOrDefault(f => f.Token == TOKENS_BOLSA_SOCIAL.PERCENTUAL_MAXIMO_BOLSA);

            // Verifica se foram informados dados no mestre-detalhe de grupo familiar.
            if (qtdGrupoFamiliar > 0)
            {
                // TOTAL_RENDA_BRUTA
                var totalRendaBruta = inscricaoDadoFormulario.DadosCampos.Where(w => w.Token == TOKENS_BOLSA_SOCIAL.RENDA_BRUTA && !string.IsNullOrWhiteSpace(w.Valor))
                                                                         .Sum(s => Convert.ToDecimal(s.Valor));
                campoTotalRendaBruta.Valor = totalRendaBruta.ToString();

                // RENDA_PER_CAPTA
                var rendarPerCapta = totalRendaBruta / qtdGrupoFamiliar;
                campoRendaPerCapta.Valor = rendarPerCapta.ToString();

                // PERCENTUAL_MAXIMO_BOLSA
                var salarioMinimo = Convert.ToDecimal(inscricaoDadoFormulario.DadosCampos.First(w => w.Token == TOKENS_BOLSA_SOCIAL.SALARIO_MINIMO).Valor);
                decimal percentualMaximo;
                if (rendarPerCapta == 0 || ((salarioMinimo * 60) / rendarPerCapta) > 60)
                {
                    percentualMaximo = 60m;
                }
                else
                {
                    if (rendarPerCapta > (salarioMinimo * 1.5m))
                    {
                        percentualMaximo = ((salarioMinimo * 2) * 20) / rendarPerCapta;
                    }
                    else
                    {
                        percentualMaximo = (salarioMinimo * 60) / rendarPerCapta;
                    }
                }

                //Recupera o val_limite_percentual_desconto da oferta
                var limitePercentualDesconto = this.InscricaoDomainService.SearchProjectionByKey(inscricaoDadoFormulario.SeqInscricao, x => x.Ofertas.FirstOrDefault().Oferta.LimitePercentualDesconto);

                //Se o valor resultante do cálculo for menor que o informado no campo val_limite_percentual_desconto da tabela oferta,
                //relativo à oferta selecionada pelo candidato (inscricao_oferta), peencher o campo PERCENTUAL_MAXIMO_BOLSA com o valor calculado.
                //Caso contrário, preencher o campo PERCENTUAL_MAXICMO_BOLSA com o valor do campo val_limite_percentual_desconto.
                campoPercentualMaximo.Valor = percentualMaximo < limitePercentualDesconto.GetValueOrDefault() ? percentualMaximo.ToString("F") : limitePercentualDesconto.GetValueOrDefault().ToString("F");
            }
            else
            {
                campoTotalRendaBruta.Valor = "0";
                campoRendaPerCapta.Valor = "0";
                campoPercentualMaximo.Valor = "0";
            }
        }

        public List<InscricaoDadoFormularioCampoVO> BuscarCamposDadoFormularioPorSeqInscricao(long seqInscricao, List<string> tokensCampo)
        {
            
            var spec = new InscricaoDadoFormularioFilterSpecification { SeqInscricao = seqInscricao };

            var dadosCampos = this.SearchProjectionBySpecification
                (spec, s => s.DadosCampos)
                .SelectMany(s => s)
                .Where(w => tokensCampo.Contains(w.Token))
                .ToList();

            dadosCampos.ForEach(f => {

                string[] value = f.Valor.Split('|');

                if (value.Length > 1)
                {
                    f.Valor = value[1];
                }
            });
            return dadosCampos.TransformList<InscricaoDadoFormularioCampoVO>();
        }
    }
}