using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMC.Framework.Entity;
using SMC.Framework.Ioc;
using SMC.Framework.Repository;

namespace SMC.Inscricoes.EntityRepository.Ioc
{
    /// <summary>
    /// Classe para mapeamento do repositório do Entity.
    /// </summary>
    public class EntityIocMapping : SMCEntityIocMapping
    {
        /// <summary>
        /// Configura o container de Ioc.
        /// </summary>
        /// <param name="container">Container de Ioc.</param>
        protected override void Configure()
        {
            this.Providers
                .Register<InscricaoContext>();
        }
    }
}
