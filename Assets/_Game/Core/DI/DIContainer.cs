using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Core.DI
{
    public class DIContainer
    {
        private static DIContainer _global;
        public static DIContainer Global => _global ??= new DIContainer();

        private readonly Dictionary<Type, object> _services = new();

        public void Register<T>(T service)
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"DIContainer: Service of type {type.Name} is already registered. Overwriting.");
            }
            _services[type] = service;
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            
            Debug.LogError($"DIContainer: Service of type {type.Name} not found!");
            return default;
        }

        public bool TryResolve<T>(out T service)
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var s))
            {
                service = (T)s;
                return true;
            }
            service = default;
            return false;
        }
    }
}
