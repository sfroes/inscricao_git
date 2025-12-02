using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Framework.Model;
using System.Web.Mvc;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupEtapaFiltroExemploViewModel : ISMCFilter<LookupEtapaFiltroViewModel>
    {
        public LookupEtapaFiltroViewModel Filter(SMCControllerBase controllerContext, LookupEtapaFiltroViewModel filter)
        {
            filter.UnidadesResponsaveis = new List<Framework.Model.SMCDatasourceItem>()
            {
                new SMCDatasourceItem(){Seq=1,Descricao="Centro de Registros Acadêmicos"},
                new SMCDatasourceItem(){Seq=2,Descricao="Colégio Santa Maria"},
                new SMCDatasourceItem(){Seq=3,Descricao="Departamento de Relações Internacionais"},
                new SMCDatasourceItem(){Seq=4,Descricao="Diretoria de Educação Continuada"},
            };

            filter.TiposProcesso = new List<SMCDatasourceItem>()
            {
                new SMCDatasourceItem(){Seq=1,Descricao="Vestibular - Graduação"},
                new SMCDatasourceItem(){Seq=2,Descricao="Processo Seletivo - Ensino Fundamental"},
                new SMCDatasourceItem(){Seq=3,Descricao="Minionu"},
            };

            filter.Etapas = new List<SMCDatasourceItem>()
            {
                new SMCDatasourceItem(){Seq=1,Descricao="Inscrição"},
                new SMCDatasourceItem(){Seq=2,Descricao="Seleção"},
                new SMCDatasourceItem(){Seq=3,Descricao="Convocação"},
            };

            return filter;
        }
    }
}
