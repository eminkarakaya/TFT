using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookat : MonoBehaviour
{
    [SerializeField] Transform target;
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(-50,0,0);
    }
}

