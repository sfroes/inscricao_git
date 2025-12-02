using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Validation;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.RES.Validators
{
    public class UnidadeResponsavelValidator : SMCValidator<UnidadeResponsavel>
    {
        /// <summary>
        /// Realiza a validação de inscrito
        /// </summary>
        /// <param name="item">Inscrito a ser validado</param>
        /// <param name="validationResults">Resultado da validação</param>
        protected override void DoValidate(UnidadeResponsavel item, SMCValidationResults validationResults)
        {
            base.DoValidate(item, validationResults);
                        
            if (item.EnderecosEletronicos != null && 
                item.EnderecosEletronicos.Count(x=>x.TipoEnderecoEletronico==TipoEnderecoEletronico.Email)<1)
            {
                this.AddPropertyError(x => x.EnderecosEletronicos, 
                    Resources.MessagesResource.ResourceManager.GetString(
                    "EmailObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            
        }

    }
}
