namespace GameBoxSdk.Runtime.Utils
{
    using System.Collections.Generic;

    using UnityEngine;

    public class TimerManager : MonoBehaviour
    {
        private static TimerManager instance = null;

        public static TimerManager Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject instanceGO = new GameObject("Timer");
                    DontDestroyOnLoad(instanceGO);
                    instance = instanceGO.AddComponent<TimerManager>();
                }

                return instance;
            }
        }

        private List<Timer> activeTimers = null;

        #region Unity Methods

        private void Awake()
        {
            activeTimers = new List<Timer>();
        }

        private void Update()
        {
            for(int i = 0; i < activeTimers.Count; i++)
            {
                activeTimers[i].Tick(Time.deltaTime);
            }
        }

        #endregion

        public void AddTimer(Timer timer)
        {
            if(activeTimers.Contains(timer))
            {
                return;
            }

            activeTimers.Add(timer);
        }

        public void RemoveTimer(Timer timer)
        {
            if(!activeTimers.Contains(timer))
            {
                return;
            }

            activeTimers.Remove(timer);
        }
    }
}
