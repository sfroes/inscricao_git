using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SMC.GPI.Inscricao.Models
{
	public class FormularioImpactoViewModel : SMCViewModelBase
	{
        public long SeqFormulario { get; set; }
        public DateTime? DataFimResposta { get; set; }
        public long SeqDadoFormulario { get; set; }
        public long SeqInscricao { get; set; }
        public long SeqVisaoSGF { get; set; }
        public bool ExibirFormularioImpacto { get; set; }
        public string MensagemInformativa { get; set; }
        public string DescricaoMensagemInformativa { get; set; }
        public string MensagemFormularioImpactoIndisponivel { get; set; }
    }
}