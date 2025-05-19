namespace GameBoxSdk.Runtime.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;

    public class SystemsInitializer
    {
        public event Action OnSystemsInitialized = null;

        private Dictionary<Type, BaseSystem> systemsInitialized = null;

        public SystemsInitializer()
        {
            systemsInitialized = new Dictionary<Type, BaseSystem>();
        }

        public async void InitializeSystems(IEnumerable<BaseSystem> sourceSystemsToInitialize)
        {
            Dictionary<Type, BaseSystem> systemsToInitialize = new Dictionary<Type, BaseSystem>();

            foreach(BaseSystem baseSystem in sourceSystemsToInitialize)
            {
                systemsToInitialize.Add(baseSystem.GetType(), baseSystem);
            }
            
            Dictionary<Task<bool>, BaseSystem> initializationTaskPerSystem = new Dictionary<Task<bool>, BaseSystem>();
            List<Type> dependenciesToVerify = new List<Type>(systemsToInitialize.Count);

            foreach (Type baseSystemType in systemsToInitialize.Keys)
            {
                Debug.Assert(systemsToInitialize.ContainsKey(baseSystemType), 
                    $"{GetType()}: You are adding a type of system more than once, you can only have one system per class type");
                dependenciesToVerify.Add(baseSystemType);
            }

            while(systemsToInitialize.Count > 0)
            {
                foreach(BaseSystem baseSystem in systemsToInitialize.Values)
                {
                    dependenciesToVerify.Remove(baseSystem.GetType());
                    bool isDepencyNeededToInitialize = false;

                    foreach(Type dependency in dependenciesToVerify)
                    {
                        if(baseSystem.Dependencies.ContainsKey(dependency))
                        {
                            isDepencyNeededToInitialize = true;
                            break;
                        }
                    }
                    
                    dependenciesToVerify.Add(baseSystem.GetType());

                    if(isDepencyNeededToInitialize)
                    {
                        continue;
                    }

                    initializationTaskPerSystem.Add(baseSystem.Initialize(systemsInitialized.Values), baseSystem);
                }

                await Task.WhenAll(initializationTaskPerSystem.Keys);

                foreach(Task<bool> task in initializationTaskPerSystem.Keys)
                {
                    BaseSystem systemInitialized = initializationTaskPerSystem[task];
                    Type systemType = systemInitialized.GetType();
                    Debug.Assert(task.Result, $"{GetType()} - Failed to initialize {systemType}.");
                    systemsToInitialize.Remove(systemType);
                    dependenciesToVerify.Remove(systemType);
                    systemsInitialized.Add(systemType, systemInitialized);
                }

                initializationTaskPerSystem.Clear();
            }

            OnSystemsInitialized?.Invoke();
        }

        public T GetSystem<T>() where T : BaseSystem
        {
            Type systemType = typeof(T);
            return systemsInitialized.ContainsKey(systemType) ? systemsInitialized[systemType] as T : null;
        }

        public void Dispose()
        {
            systemsInitialized.Clear();
        }
    }
}
