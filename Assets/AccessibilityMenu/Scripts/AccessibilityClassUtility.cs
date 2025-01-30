public class AccessibilityClassUtility
{
    public const string AccessibilitySaveData = "AccessibilitySaveData";

    // Each index corresponds to the font size of TextType that are: Heading, SubHeading, BodyText, ButtonText
    public static float[] fontSizeMin = { 36f, 24f, 14f, 16f };
    public static float[] fontSizeMax = { 48f, 32f, 22f, 24f };
}

/// <summary>
/// Tracks Options Selected in Options screen
/// </summary>
[System.Serializable]
public class OptionsData
{
    public int ColorBlindMode;
    public int FontMultiplier;
    public int Language;

    public OptionsData()
    {
        ColorBlindMode = 0;
        FontMultiplier = 1;
        Language = 0;
    }

    public OptionsData(int colorBlindMode, int fontMultiplier, int language)
    {
        ColorBlindMode = colorBlindMode;
        FontMultiplier = fontMultiplier;
        Language = language;
    }
}