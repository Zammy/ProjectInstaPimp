using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionChecker : MonoBehaviour
{
    List<Collider> colliders = new List<Collider>();

    public bool IsCollidingWith(string tag)
    {
        foreach (var collider in colliders)
        {
            if (collider.tag == tag)
            {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider collider)
    {
        colliders.Add(collider);
    }
    
    void OnTriggerExit(Collider collider)
    {
        colliders.Remove(collider);
    }
}
