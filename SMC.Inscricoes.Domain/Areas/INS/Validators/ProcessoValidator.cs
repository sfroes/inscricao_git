using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Validation;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Validators
{
    public class ProcessoValidator : SMCValidator<Processo>
    {
        /// <summary>
        /// Realiza a validação de inscrito
        /// </summary>
        /// <param name="item">Inscrito a ser validado</param>
        /// <param name="validationResults">Resultado da validação</param>
        protected override void DoValidate(Processo item, SMCValidationResults validationResults)
        {
            base.DoValidate(item, validationResults);
                        
            if (item.EnderecosEletronicos != null && 
                item.EnderecosEletronicos.Count(x=>x.TipoEnderecoEletronico==TipoEnderecoEletronico.Website)>1)
            {
                this.AddPropertyError(x => x.EnderecosEletronicos, 
                    Resources.MessagesResource.ResourceManager.GetString(
                    "WebsiteProcessoDuplicado", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
            var possuiMaisDeUmIgual = item.ConfiguracoesFormulario.GroupBy(x => x.SeqFormularioSgf).Any(y => y.Count() > 1);
            if (possuiMaisDeUmIgual)
            {
                this.AddPropertyError(x => x.ConfiguracoesFormulario,
                    Resources.MessagesResource.ResourceManager.GetString(
                    "FormularioEventoDuplicado", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if (item.Idiomas.Count(x => x.Padrao) == 0) 
            {
                this.AddPropertyError(x => x.Idiomas,
                    Resources.MessagesResource.ResourceManager.GetString(
                    "IdiomaPadraoNaoDefinido", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
            else if (item.Idiomas.Count(x => x.Padrao) > 1) 
            {
                this.AddPropertyError(x => x.Idiomas,
                    Resources.MessagesResource.ResourceManager.GetString(
                    "MaisDeUmIdiomaPadrao", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
            else if (item.EnderecosEletronicos != null && item.EnderecosEletronicos.Count() > 0
               && item.EnderecosEletronicos.Any(x => x.TipoEnderecoEletronico == TipoEnderecoEletronico.Email) &&
                (!SMCValidationHelper.ValidateEmail(
                    item.EnderecosEletronicos.FirstOrDefault(
                        x => x.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).Descricao)))
            {
                this.AddPropertyError(x => x.EnderecosEletronicos,
                     Resources.MessagesResource.ResourceManager.GetString(
                    "EmailInvalido", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
            
        }

    }
}
