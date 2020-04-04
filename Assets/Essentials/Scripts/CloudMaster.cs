using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMaster : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private void Update()
    {
        this.transform.Rotate(Vector3.up * _speed * Time.deltaTime);
    }
}
