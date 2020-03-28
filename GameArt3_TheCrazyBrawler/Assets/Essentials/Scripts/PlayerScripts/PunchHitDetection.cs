using System;
using UnityEngine;

public class PunchHitDetection : MonoBehaviour
{
    public event Action<GameObject, Vector3> OnHit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != transform.root && other.CompareTag("hitbox")) 
            OnHit?.Invoke(other.transform.root.gameObject, transform.root.forward);
    }

    private void Update()
    {
        Debug.DrawLine(transform.root.position, transform.root.forward * 100, Color.yellow);
    }
}
