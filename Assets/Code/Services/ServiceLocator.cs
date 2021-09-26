using System;
using System.Collections.Generic;
using System.Linq;
using Code.Interfaces;
using Code.Patterns;
using UnityEngine;

namespace Code.Services
{
    public class ServiceLocator : Singleton<ServiceLocator>
    {
        [SerializeField] private ServicesSettings settings;

        private Dictionary<Type, IService> _services;

        // Start service configuration
        protected override void Initialize()
        {
            if (settings == null)
                settings = Resources.Load<ServicesSettings>("ServicesSettings");
            if (settings == null)
                Debug.LogError(
                    "No ServicesSettings asset found in Resources folder. Create it using the menu: Services/Settings on Project window");

            _services = new Dictionary<Type, IService>();
        }

        // Sistema de búsqueda de ObtainService
        // 1. Busca en el diccionario
        // 2. Busca en la escena si existe ese servicio
        // 3. Busca en el archivo de configuración de servicios
        public T ObtainService<T>() where T : IService
        {
            T service; // Servicio que manda de regreso

            // (1)
            if (_services.ContainsKey(typeof(T)))
            {
                service = (T)_services[typeof(T)];
            }
            else
            {
                // (2)
                service = FindObjectsOfType<MonoBehaviour>().OfType<T>().FirstOrDefault();
                if (service != null)
                {
                    service.gameObject.transform.SetParent(transform);
                }
                else
                {
                    // (3)
                    GameObject servicePrefab = settings.ServicesPrefabs.Where(x => x.GetComponentInChildren<T>() != null).FirstOrDefault();
                    if (servicePrefab != null)
                    {
                        GameObject newServiceInstance = Instantiate(servicePrefab, transform);
                        service = newServiceInstance.GetComponentInChildren<T>();
                    }
                    else
                    {
                        Debug.LogErrorFormat(
                            $"ServiceLocator could not find an existing or default service for type {typeof(T).FullName}");
                    }
                }

                _services[typeof(T)] = service;
            }

            if (service != null)
            {
                service.gameObject.name = typeof(T).Name + " Service";
            }

            return service;
        }
    }
}
