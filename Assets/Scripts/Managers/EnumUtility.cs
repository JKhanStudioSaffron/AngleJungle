using System;

/// <summary>
/// Purpose: Defines platform-specific build target flags for conditional feature toggling.
/// Responsibilities:
/// - Provides a bitwise `enum` to represent multiple platforms simultaneously.
/// - Enables checking for specific or combined platform support.
/// Usage:
/// - Use bitwise operations to check or combine multiple platforms.
/// - Example: `BuildTargetFlags.Windows | BuildTargetFlags.Linux`
/// </summary>


[Flags]
public enum BuildTargetFlags
{
    None = 0,
    Windows = (1 << 0),
    MacOS = (1 << 1),
    Linux = (1 << 2),
    Android = (1 << 3),
    iOS = (1 << 4),
    WebGL = (1 << 5)
}
