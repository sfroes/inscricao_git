using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.RES;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TituloInscricaoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        [SMCKey]
        public long SeqInscricaoBoletoTitulo { get; set; }

        public SMCEncryptedLong id
        {
            get
            {
                return new SMCEncryptedLong(SeqTitulo);
            }
        }

        [SMCLink("BoletoTitulo", "AcompanhamentoProcesso", SMCLinkTarget.NewWindow, "id")]
        public int SeqTitulo { get; set; }

        public TipoBoleto TipoBoleto { get; set; }

        public string Valor { get; set; }

        public string Situacao
        {
            get
            {
                return DataCancelamento.HasValue ?
                    SMC.GPI.Administrativo.Areas.INS.Views.AcompanhamentoProcesso.App_LocalResources.UIResource
                .ResourceManager.GetString("Texto_Cancelado", CultureInfo.CurrentCulture) :
                    SMC.GPI.Administrativo.Areas.INS.Views.AcompanhamentoProcesso.App_LocalResources.UIResource
                .ResourceManager.GetString("Texto_Ativo", CultureInfo.CurrentCulture);
            }
        }

        public List<TaxaTituloInscricaoViewModel> Taxas { get; set; }

        public DateTime? DataCancelamento { get; set; }

        public DateTime DataVencimento { get; set; }

        public DateTime DataGeracao { get; set; }

        public DateTime? DataPagamento { get; set; }
    }
}