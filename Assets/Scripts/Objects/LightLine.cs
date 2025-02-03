using System.Collections;
using System.Linq;
using UnityEngine;

public class LightLine : MonoBehaviour 
{
    private float collisionTimeout = 0.5f;
        
    void OnParticleCollision(GameObject other)
	{
        if (other.tag == Global.TAG_POWER_GEM)
        {
            PowerGem powerGem = other.GetComponent<PowerGem>();
            powerGem.ActivateGem(gameObject); // Pass self as identifier
            StartCoroutine(RemoveLightAfterDelay(powerGem));
        }

        if (other.tag == Global.TAG_MIRROR_RECEIVER) 
		{
            other.GetComponentInParent<Mirror>().ActiveLight();
        }
    }

    /// <summary>
    /// Checks light still colliding with powergem or not
    /// </summary>
    /// 
    IEnumerator RemoveLightAfterDelay(PowerGem powerGem)
    {
        yield return new WaitForSeconds(collisionTimeout);

        if (!IsStillCollidingWithPowerGem())
        {
            powerGem.DeactivateGem(gameObject);
        }
    }

    /// <summary>
    /// casts a ray after timeout to check if its still colliding with the powergem
    /// </summary>
    /// 

    private bool IsStillCollidingWithPowerGem()
    {        
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.forward);
        if(hit.Length > 0)
        {
            if (hit.ToList().Exists(obj => obj.collider.tag == Global.TAG_POWER_GEM))
                return true;
        }
        return false;
    }
}
