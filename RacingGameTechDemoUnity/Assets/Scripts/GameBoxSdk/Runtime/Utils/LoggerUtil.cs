namespace GameBoxSdk.Runtime.Utils
{
    using UnityEngine;

    public static class LoggerUtil
    {
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }

        public static void LogError(object message)
        {
            Debug.LogError(message);
        }

        public static void Assert(bool condition, object message)
        {
            Debug.Assert(condition, message);
        }
    }
}
