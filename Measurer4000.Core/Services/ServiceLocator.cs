using System;
using System.Collections.Generic;

namespace Measurer4000.Core.Services
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static T Get<T>()
        {
            if(_services.ContainsKey(typeof(T)))
            {
                return (T)_services[typeof(T)];
            }

            throw new Exception($"Service {typeof(T)} not register");
        }

        public static void Register<T>(T implementation)
        {
            if(!_services.ContainsKey(typeof(T)))
            {
                _services.Add(typeof(T), implementation);
            }
            else
            {
                _services[typeof(T)] = implementation;
            }
        }
    }
}
