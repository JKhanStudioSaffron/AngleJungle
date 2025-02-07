using System;

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
