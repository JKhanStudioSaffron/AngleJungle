using UnityEngine;

/// <summary>
/// Purpose: Provides an extension method to convert `RuntimePlatform` to `BuildTargetFlags`.
/// Responsibilities:
/// - Maps Unity's `RuntimePlatform` enum to a corresponding `BuildTargetFlags` value.
/// Usage:
/// - Call `RuntimePlatform.ToBuildTargetFlag()` to get the appropriate `BuildTargetFlags` enum.
/// - Useful for platform-specific logic, such as enabling/disabling features based on the build target.
/// </summary>

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
