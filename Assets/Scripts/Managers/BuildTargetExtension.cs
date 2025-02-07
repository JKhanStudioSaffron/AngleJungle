using UnityEngine;

public static class BuildTargetExtensions
{
    public static BuildTargetFlags ToBuildTargetFlag(this RuntimePlatform target)
    {
        return target switch
        {
            RuntimePlatform.WindowsPlayer => BuildTargetFlags.Windows,
            RuntimePlatform.WindowsEditor => BuildTargetFlags.Windows,
            RuntimePlatform.OSXPlayer => BuildTargetFlags.MacOS,
            RuntimePlatform.OSXEditor => BuildTargetFlags.MacOS,
            RuntimePlatform.LinuxEditor => BuildTargetFlags.Linux,
            RuntimePlatform.LinuxPlayer => BuildTargetFlags.Linux,
            RuntimePlatform.Android => BuildTargetFlags.Android,
            RuntimePlatform.IPhonePlayer => BuildTargetFlags.iOS,
            RuntimePlatform.WebGLPlayer => BuildTargetFlags.WebGL,
            _ => BuildTargetFlags.None
        };
    }
}
