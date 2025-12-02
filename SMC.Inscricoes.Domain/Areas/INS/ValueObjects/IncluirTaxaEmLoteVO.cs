using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class IncluirTaxaEmLoteVO :  ISMCMappable
    {
        public List<SMCDatasourceItem> Ofertas { get; set; }

        public List<OfertaPeriodoTaxa> Taxas { get; set; }
    }
}
