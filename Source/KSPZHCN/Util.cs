using UnityEngine;

namespace KSPZHCN
{
    public static class Util
    {
        public static void Log(string msg)
        {
            Debug.Log("[KSPZHCN] " + msg);
        }

        public static void Warn(string msg)
        {
            Debug.LogWarning("[KSPZHCN] " + msg);
        }
    }
}
