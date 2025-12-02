using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
	public interface IInscritoControllerService
	{
        /// <summary>
        /// Busca os nomes do inscrito
        /// </summary>
        AlterarDadosInscritoViewModel BuscarNomesDadosInscrito(long seqInscrito);

        void AlterarNomeSocial(AlterarDadosInscritoViewModel modelo);
    }
}
