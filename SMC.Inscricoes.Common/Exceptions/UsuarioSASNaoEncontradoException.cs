using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Inscricoes.Common.Resources;

namespace SMC.Inscricoes.Common.Exceptions
{
    public class UsuarioSASNaoEncontradoException : SMCApplicationException
    {
        public UsuarioSASNaoEncontradoException() 
            : base(ExceptionsResource.UsuarioSASNaoEncontradoException)
        { }
    }
}
