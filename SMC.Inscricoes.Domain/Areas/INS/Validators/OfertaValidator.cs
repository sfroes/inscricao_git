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
    public class OfertaValidator : SMCValidator<Oferta>
    {
        /// <summary>
        /// Realiza a validação de inscrito
        /// </summary>
        /// <param name="item">Inscrito a ser validado</param>
        /// <param name="validationResults">Resultado da validação</param>
        protected override void DoValidate(Oferta item, SMCValidationResults validationResults)
        {
            base.DoValidate(item, validationResults);

            if (item.InscricaoSoComCodigo && !item.CodigosAutorizacao.Any())
            {
                this.AddPropertyError(x => x.CodigosAutorizacao,
                    Resources.MessagesResource.ResourceManager.GetString(
                    "InfomarPeloMenosUmCodigoOferta", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if (item.PermiteVariosCodigos && item.CodigosAutorizacao.Count <= 1)
            {
                this.AddPropertyError(x => x.CodigosAutorizacao,
                    Resources.MessagesResource.ResourceManager.GetString(
                    "InformarMaisDeUmCodigoOferta", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if (item.Taxas != null && item.Taxas.Any(x => x.NumeroMaximo < x.NumeroMinimo)) 
            {
                this.AddPropertyError(x => x.Taxas,
                    Resources.MessagesResource.ResourceManager.GetString(
                    "MaximoTaxasInferiorMinido", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if(item.EnderecosEletronicos!=null && item.EnderecosEletronicos.Count()>0
               && item.EnderecosEletronicos.Any(x=>x.TipoEnderecoEletronico==TipoEnderecoEletronico.Email) &&
                (!SMCValidationHelper.ValidateEmail(
                    item.EnderecosEletronicos.FirstOrDefault(
                        x=>x.TipoEnderecoEletronico==TipoEnderecoEletronico.Email).Descricao)))
            {
                this.AddPropertyError(x => x.EnderecosEletronicos,
                     Resources.MessagesResource.ResourceManager.GetString(
                    "EmailInvalido", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
                
        }

    }
}
