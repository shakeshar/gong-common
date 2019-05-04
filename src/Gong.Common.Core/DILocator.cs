using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gong.Common.Core
{
    public class DILocator
    {
        static IServiceProvider provider;
        public DILocator()
        {

        }
        public static void Init(IServiceCollection collection)
        { 

            provider = collection.BuildServiceProvider();
        }
        public static T Resolve<T>()
        {
            return provider.GetService<T>();
        }
        public static object Resolve(Type t)
        {
            return provider.GetService(t);
        }
    }
}