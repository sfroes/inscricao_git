using SMC.Framework.Model;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoDadoFormularioCampoDomainService : InscricaoContextDomain<InscricaoDadoFormularioCampo>
    {
        public SMCPagerData<InscricaoDadoFormularioCampoValorVO> BuscarValoresElemento(InscricaoDadoFormularioCampoFilterSpecification spec)
        {
            int total = 0;
            spec.MaxResults = 10;

            List<InscricaoDadoFormularioCampoValorVO> lista = this.SearchProjectionBySpecification(spec, x => new InscricaoDadoFormularioCampoValorVO { Valor = x.Valor }, out total, true).ToList();

            // Verifica se existe algum dado campo com valor múltiplo. Estes dados são separados por ||
            var listaMultiplo = lista.Where(x => x.Valor.Contains("||")).ToList();

            // Caso tenha algum, itera e deleta o elemento, adicionando os novos elementos na lista
            if (listaMultiplo != null)
            {
                foreach (var item in listaMultiplo)
                {
                    // Remove
                    lista.Remove(item);

                    // Adiciona os valores separadamente
                    lista.AddRange(item.Valor.Split(new string[] { "||" }, StringSplitOptions.None).Select(y => new InscricaoDadoFormularioCampoValorVO { Valor = y }));
                }

                // Pega distinct
                lista = lista.GroupBy(i => i.Valor, (key, group) => group.First()).ToList();
            }

            // Corrige as descrições
            lista.ForEach(item =>
            {
                item.Descricao = item.Valor?.Split('|').LastOrDefault();
                //corrigi por causa do javascript considerar ponto e virgual como fim de codigo
                item.Valor = item.Valor.Replace(";", "%%");
            });

            return new SMCPagerData<InscricaoDadoFormularioCampoValorVO>(lista, total);
        }
    }
}