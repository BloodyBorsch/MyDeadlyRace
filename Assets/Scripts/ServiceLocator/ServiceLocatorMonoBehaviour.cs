﻿using System.Collections.Generic;
using UnityEngine;


namespace MaksK_Race
{
    public static class ServiceLocatorMonoBehaviour
    {
        private static Dictionary<object, object> _servicecontainer = null;

        public static T GetService<T>(bool createObjectIfNotFound = true) where T : Object
        {
            if (_servicecontainer == null)
            {
                _servicecontainer = new Dictionary<object, object>();
            }

            if (!_servicecontainer.ContainsKey(typeof(T)))
            {
                return FindService<T>(createObjectIfNotFound);
            }

            var service = (T)_servicecontainer[typeof(T)];

            if (service != null)
            {
                return service;
            }

            _servicecontainer.Remove(typeof(T));

            return FindService<T>(createObjectIfNotFound);

        }

        private static T FindService<T>(bool createObjectIfNotFound = true) where T : Object
        {
            T type = Object.FindObjectOfType<T>();

            if (type != null)
            {
                _servicecontainer.Add(typeof(T), type);
            }

            else if (createObjectIfNotFound)
            {
                var go = new GameObject(typeof(T).Name, typeof(T));
                _servicecontainer.Add(typeof(T), go.GetComponent<T>());
            }

            return (T)_servicecontainer[typeof(T)];
        }
    }
}
