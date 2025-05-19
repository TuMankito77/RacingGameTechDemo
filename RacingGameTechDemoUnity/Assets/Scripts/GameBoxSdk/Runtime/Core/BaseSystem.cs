namespace GameBoxSdk.Runtime.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;

    public abstract class BaseSystem
    {
        private Dictionary<Type, BaseSystem> dependencies = null;

        public IReadOnlyDictionary<Type, BaseSystem> Dependencies => dependencies;

        public BaseSystem()
        {
            dependencies = new Dictionary<Type, BaseSystem>();
        }

        public virtual async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            foreach(BaseSystem baseSystem in sourceDependencies)
            {
                Type baseSystemType = baseSystem.GetType();

                if(dependencies.ContainsKey(baseSystemType))
                {
                    dependencies[baseSystemType] = baseSystem;
                }
            }

            await Task.Yield();
            return true;
        }

        public BaseSystem AddDependency<T>() where T : BaseSystem
        {
            if(dependencies.ContainsKey(typeof(T)))
            {
                Debug.LogError($"{GetType().Name}: You are trying to add the dependency {typeof(T).Name} more than once.");
            }
            else
            {
                dependencies.Add(typeof(T), null);
            }

            return this;
        }
        
        protected T GetDependency<T>() where T : BaseSystem
        {
            Type systemType = typeof(T);
            return dependencies.ContainsKey(systemType) ? dependencies[systemType] as T : null;
        }
    }
}
