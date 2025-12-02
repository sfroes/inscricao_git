using SMC.Framework.Fake;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SMC.GPI.Administrativo.Fake
{
    public class SMCFakeListStrategy : SMCFakeStrategyBase
    {
        public override int Priority
        {
            get { return 200; }
        }

        protected override bool CheckProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(List<OfertaInscricaoViewModel>);
        }

        protected override object CreateProperty(PropertyInfo propertyInfo)
        {
            return GeraLista(propertyInfo.PropertyType);
        }

        private static object GeraLista(Type type)
        {
            Type genericType = typeof(List<>).MakeGenericType(type.GenericTypeArguments[0]);
            IList returnMock = (IList)Activator.CreateInstance(genericType);
            for (int i = 0; i < 2; i++)
            {
                returnMock.Add(SMC.Framework.Fake.SMCFake.Create(type.GenericTypeArguments[0]));
            }
            return returnMock;
        }
    }
}