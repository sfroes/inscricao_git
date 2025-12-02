using SMC.Framework.Model;
using SMC.Framework.Rest;
using SMC.Framework.Service;
using SMC.Framework.Util;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.IntegracaoAcademico.ServiceContract.Areas.IAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class FonteExternaService : SMCServiceBase, IFonteExternaService
    {
        private IIntegracaoAcademicoService IntegracaoAcademicoService => this.Create<IIntegracaoAcademicoService>();

        public List<SMCDatasourceItem> ListarProjetoVinculadoInscricao(string seqProcessoInscricao, string seqUsuarioSAS)
        {
            try
            {
                var lista = new List<SMCDatasourceItem>();

                var url = ConfigurationManager.AppSettings["UrlProjetoVinculadoInscricao"];
                url = $"{url}/{seqProcessoInscricao}/{seqUsuarioSAS}";

                var json = SMCRest.GetJson(url);
                lista = SMCJsonHelper.Parse<List<SMCDatasourceItem>>(json);

                return lista;
            }
            catch (Exception ex)
            {
                return new List<SMCDatasourceItem>();
            }
        }

        public List<SMCDatasourceItem> BuscarUnidadesSelect()
        {
            try
            {
                List<SMCDatasourceItem> dataSource = new List<SMCDatasourceItem>();

                var unidades = IntegracaoAcademicoService.BuscarUnidadesNucleos();

                foreach (var item in unidades)
                {
                    dataSource.Add(new SMCDatasourceItem() { Seq = item.Codigo, Descricao = item.Descricao });
                }

                return dataSource;
            }
            catch
            {
                return new List<SMCDatasourceItem>();
            }
        }

        public List<SMCDatasourceItem> BuscarCursosPorNucleoSelect(string codigoUnidade)
        {
            List<SMCDatasourceItem> datasourceItems = new List<SMCDatasourceItem>();

            int x1 = 0;

            if (!string.IsNullOrEmpty(codigoUnidade) && int.TryParse(codigoUnidade, out x1))
            {
                var lista = IntegracaoAcademicoService.BuscarOfertasCursosPorNucleo(x1);

                foreach (var item in lista)
                {
                    datasourceItems.Add(new SMCDatasourceItem() { Seq = item.Codigo, Descricao = item.Descricao });
                }
            }

            return datasourceItems;
        }

    }
}
