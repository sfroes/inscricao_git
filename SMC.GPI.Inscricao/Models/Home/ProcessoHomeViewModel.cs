using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Models
{
	public class ProcessoHomeViewModel : SMCViewModelBase
	{

        public long SeqProcesso { get; set; }
        public string DescricaoProcesso { get; set; }
        public string DescricaoComplementarProcesso { get; set; }
        public List<SMCLanguage> IdiomasDisponiveis { get; set; }
        public string UrlInformacaoComplementar { get; set; }
        public SMCLanguage IdiomaAtual { get; set; }
        public bool ProcessoCancelado { get; set; }
        public SituacaoEtapa SituacaoEtapaInscricao { get; set; }
        public bool TodasOfertasProcessoInativas { get; set; }
        public string TokenResource { get; set; }
        public bool ProcessoEncerrado { get; set; }
        public bool ProcessoEmAndamento { get; set; }
        public string UidProcesso { get; set; }
        public string OrientacaoCadastroInscrito { get; set; }

        public Guid? UidInscricaoOferta { get; set; }
        public string TokenCssAlternativoSas { get; set; }
        public FormularioImpactoViewModel FormularioImpacto { get; set; }

        #region Métodos de apoio

        public string MensagemInformativa
        {
            get
            {                
                if (this.ProcessoCancelado)
                {
                    return SMC.GPI.Inscricao.Views.Home.App_LocalResources.UIResource.Mensagem_Informativa_Processo_Cancelado;
                }
                else if (this.ProcessoEncerrado)
                {
                    return SMC.GPI.Inscricao.Views.Home.App_LocalResources.UIResource.Mensagem_Informativa_Processo_Encerrado;
                }
                else if (this.SituacaoEtapaInscricao != SituacaoEtapa.Liberada)
                {
                    return string.Format(SMC.GPI.Inscricao.Views.Home.App_LocalResources.UIResource.Mensagem_Informativa_Inscricoes_Nao_Liberada, this.SituacaoEtapaInscricao.SMCGetDescription().ToLower());
                }
                else if (this.TodasOfertasProcessoInativas)
                {
                    return SMC.GPI.Inscricao.Views.Home.App_LocalResources.UIResource.ResourceManager.GetString("Mensagem_Nenhuma_Inscricao_Aberta_"+ TokenResource);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string TituloInscricoes { get; set; }

        public string UrlCss { get; set; }

        #endregion 

    }
}