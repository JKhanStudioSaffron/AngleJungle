using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Purpose: Generates and configures gem objects based on predefined angle values.
/// Responsibilities:
/// - Instantiates gem prefabs and assigns them a numerical angle value.
/// - Sets the appropriate sprite for positive and negative angles.
/// - Assigns the gem's angle data for gameplay logic.
/// Usage:
/// - Attach to an empty GameObject and assign `GemPrefab`, `PositiveAngle`, and `NegativeAngle`.
/// - Populate `GemsList` with angle values and call `CreateGems()` to generate gems.
/// </summary>

public class GemCreator : MonoBehaviour
{
    public List<int> GemsList;
    public GameObject GemPrefab;
    public Sprite PositiveAngle, NegativeAngle;

    public void CreateGems()
    {
        foreach (var gemVal in GemsList)
        {
            GameObject gem = (GameObject)PrefabUtility.InstantiatePrefab(GemPrefab);
            gem.name = $"Gem_{gemVal}";
            gem.transform.parent = transform;
            gem.GetComponentInChildren<TMP_Text>().text = gemVal.ToString() + "°";

            if (gemVal < 0)
                gem.GetComponent<SpriteRenderer>().sprite = NegativeAngle;
            else
                gem.GetComponent<SpriteRenderer>().sprite = PositiveAngle;

            gem.GetComponent<Gem>().gemAngle = gemVal;
        }
    }
}
