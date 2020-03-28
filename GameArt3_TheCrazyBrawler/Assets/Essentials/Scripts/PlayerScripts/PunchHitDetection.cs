using System;
using UnityEngine;

public class PunchHitDetection : MonoBehaviour
{
    public event Action<GameObject, Vector3> OnHit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != transform.root && other.CompareTag("hitbox") && !other.transform.root.CompareTag(transform.root.tag)) 
            OnHit?.Invoke(other.transform.root.gameObject, transform.root.forward);
    }
}
