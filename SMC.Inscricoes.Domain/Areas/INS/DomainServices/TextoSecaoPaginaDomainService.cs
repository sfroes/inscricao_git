using SMC.Framework.Domain;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class TextoSecaoPaginaDomainService : InscricaoContextDomain<TextoSecaoPagina>
    {
        public ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaIdiomaDomainService>(); }
        }

        /// <summary>
        /// Salva a alteração no texto da seção de uma página em um idioma
        /// </summary>        
        public void SalvarTextoSecao(TextoSecaoPagina textoSecao)
        {
            //valida o campo de texto do input para garantir que não envie lixo para a base de dados
            if (!string.IsNullOrEmpty(textoSecao.Texto))
            {
                textoSecao.Texto = textoSecao.Texto == "&lt;p&gt;&lt;br&gt;&lt;/p&gt;\t\t" ? null : textoSecao.Texto;
            }

            //Aplicar a RN_INS_114
            var texto = this.SearchByKey(
                new SMCSeqSpecification<TextoSecaoPagina>(textoSecao.Seq));

            var Situacao = ConfiguracaoEtapaPaginaIdiomaDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapaPaginaIdioma>(texto.SeqConfiguracaoEtapaPaginaIdioma),
                x => x.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa);
            if (Situacao == SituacaoEtapa.Liberada && texto.Texto != textoSecao.Texto)
            {
                throw new AlteracaoPaginaEtapaLiberadaException();
            }
            texto.Texto = textoSecao.Texto;
            this.UpdateEntity(texto);
        }
    }
}
