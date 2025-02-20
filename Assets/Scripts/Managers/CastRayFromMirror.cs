using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Purpose: Casts rays from a mirror to detect and interact with objects.
/// Responsibilities:
/// - Casts rays in multiple directions to find the nearest interactable object.
/// - Activates or deactivates power gems and mirrors based on ray intersections.
/// - Draws gizmo rays in the scene view for debugging.
/// Usage:
/// - Attach this script to a GameObject that emits light from a mirror.
/// - Ensure the mirror has the appropriate event subscriptions for activation.
/// </summary>

public class CastRayFromMirror : MonoBehaviour
{
    Mirror myMirror;
    public LayerMask LayersToInteractWith;
    PowerGem powerGem;
    Mirror mirrorRecv;
    bool drawRay;
    Collider2D hitCollider;
    float thickness = 0.1f;

    private void OnEnable()
    {
        if(myMirror == null)
            myMirror = GetComponentInParent<Mirror>();

        CastRay();
        myMirror.MirrorStopped += CastRay;
        myMirror.SetAngle += DeactivateCollidedObjects;
    }

    private void OnDisable()
    {
        myMirror.MirrorStopped -= CastRay;
        myMirror.SetAngle -= DeactivateCollidedObjects;
    }

    void DeactivateCollidedObjects()
    {
        if (powerGem != null)
            powerGem.DeactivateGem(gameObject);

        if (mirrorRecv != null)
        {
            mirrorRecv.DeactivateIfNoLightIntersects(this);
        }

        powerGem = null;
        mirrorRecv = null;
        hitCollider = null;
    }

    private void OnDrawGizmos()
    {
        if (!drawRay)
            return;
        
        Vector3 offset = Vector3.up * thickness;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + offset, transform.forward * 1000);
        Gizmos.DrawRay(transform.position, transform.forward * 1000f);
        Gizmos.DrawRay(transform.position - offset, transform.forward * 1000f);
    }

    private void CastRay()
    {
        Vector3 offset = Vector3.up * thickness;
        List<RaycastHit2D> hit = new()
        {
            Physics2D.Raycast(transform.position + offset, transform.forward, 1000, LayersToInteractWith),
            Physics2D.Raycast(transform.position, transform.forward, 1000, LayersToInteractWith),
            Physics2D.Raycast(transform.position - offset, transform.forward, 1000, LayersToInteractWith)
        };

        RaycastHit2D bestHit = hit
            .Where(h => h.collider != null) // Only consider valid hits
            .OrderBy(h => h.distance) // Prioritize the closest hit
            .FirstOrDefault(); // Get the nearest valid hit

        if (bestHit.collider != null)
        {
            hitCollider = bestHit.collider;
            drawRay = true;
        }
        else
        {
            drawRay = false;
            return;
        }

        if (hitCollider.gameObject.layer == LayerMask.NameToLayer(Global.LAYER_OBSTACLE))
        {
            return;
        }
        if (hitCollider.CompareTag(Global.TAG_POWER_GEM))
        {
            powerGem = hitCollider.GetComponent<PowerGem>();
            powerGem.ActivateGem(gameObject); // Pass self as identifier                        
        }

        else if (hitCollider.CompareTag(Global.TAG_MIRROR_RECEIVER))
        {
            mirrorRecv = hitCollider.GetComponentInParent<Mirror>();
            if (mirrorRecv != null)
            {
                if (mirrorRecv == myMirror)
                {
                    mirrorRecv = null;
                    return;
                }
                mirrorRecv.ActivateIfLightIntersects(this);
            }
        }
    }
}
