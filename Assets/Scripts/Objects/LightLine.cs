using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Purpose: Manages interactions between light and objects like Power Gems, Obstacles and Mirrors.
/// Responsibilities:
/// - Activates Power Gems when they come into contact with light.
/// - Triggers mirror activation when hit by light.
/// - Deactivates Power Gems when light exits.
/// - Dont let light pass if an obstacle exists
/// Usage:
/// - Attach this script to a light beam object with a trigger collider.
/// - Ensure objects like Power Gems, obstacles and Mirrors have the correct tags and layers.
/// </summary>

public class LightLine : MonoBehaviour 
{
    private EdgeCollider2D edgeCollider;
    Vector2[] originalPoints;

    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        originalPoints = edgeCollider.points;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Global.LAYER_OBSTACLE))
        {
            Vector2 collisionPoint = collision.ClosestPoint(edgeCollider.transform.position);
            ChangeEdgeColliderPoints(collisionPoint, true);
        }

        else if (collision.CompareTag(Global.TAG_POWER_GEM))
        {
            PowerGem powerGem = collision.GetComponent<PowerGem>();
            powerGem.ActivateGem(gameObject); // Pass self as identifier
        }

        else if (collision.CompareTag(Global.TAG_MIRROR_RECEIVER))
        {
            Mirror mirror = collision.GetComponentInParent<Mirror>();
            if (mirror != null)
                mirror.ActivateIfLightIntersects(this);
        }

    }

    // shortens the edge collider length if it hits an obstacle so that beyone obstacle no collision can occur
    void ChangeEdgeColliderPoints(Vector2 collisionPoint, bool shrink)
    {
        Vector2[] points = edgeCollider.points;

        if (points.Length >= 2)
        {
            if (shrink)
                points[1] = transform.InverseTransformPoint(collisionPoint); // Convert world to local space
            else
                points[1] = collisionPoint;

            edgeCollider.points = points; // Update collider
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(Global.LAYER_OBSTACLE))
            ChangeEdgeColliderPoints(originalPoints[1], false);


        if (collision.CompareTag(Global.TAG_POWER_GEM))
        {
            PowerGem powerGem = collision.GetComponent<PowerGem>();
            powerGem.DeactivateGem(gameObject);
        }

        if (collision.CompareTag(Global.TAG_MIRROR_RECEIVER))
        {
            Mirror mirror = collision.GetComponentInParent<Mirror>();
            if (mirror != null)
                mirror.DeactivateIfNoLightIntersects(this);
        }
    }

}
