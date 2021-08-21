using UnityEngine;

public class DisableAnalyticsAtRuntime : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnRuntimeMethodLoad()
    {
        UnityEngine.Analytics.Analytics.enabled = false;
        UnityEngine.Analytics.Analytics.deviceStatsEnabled = false;
        UnityEngine.Analytics.Analytics.limitUserTracking = true;
#if UNITY_2018_3_OR_NEWER
        UnityEngine.Analytics.Analytics.initializeOnStartup = false;
#endif
    }
}