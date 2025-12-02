using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class IncluirTaxaEmLoteData : ISMCMappable
    {        

        public List<SMCDatasourceItem> Ofertas { get; set; }

        public List<OfertaPeriodoTaxaData> Taxas { get; set; }

    }
}
