using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Purpose: Manages interactions between light and objects like Power Gems and Mirrors.
/// Responsibilities:
/// - Activates Power Gems when they come into contact with light.
/// - Triggers mirror activation when hit by light.
/// - Deactivates Power Gems when light exits.
/// Usage:
/// - Attach this script to a light beam object with a trigger collider.
/// - Ensure objects like Power Gems and Mirrors have the correct tags.
/// </summary>

public class LightLine : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Global.TAG_POWER_GEM))
        {
            PowerGem powerGem = collision.GetComponent<PowerGem>();
            powerGem.ActivateGem(gameObject); // Pass self as identifier
        }

        if (collision.CompareTag(Global.TAG_MIRROR_RECEIVER))
        {
            Mirror mirror = collision.GetComponentInParent<Mirror>();
            if (mirror != null)
                mirror.ActiveLight();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Global.TAG_POWER_GEM))
        {
            PowerGem powerGem = collision.GetComponent<PowerGem>();
            powerGem.DeactivateGem(gameObject);
        }

        if (collision.CompareTag(Global.TAG_MIRROR_RECEIVER))
        {
            Mirror mirror = collision.GetComponentInParent<Mirror>();
            if(mirror != null && mirror.isReceiver)
                mirror.DeactivateLight();
        }
    }

}
