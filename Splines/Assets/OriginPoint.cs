using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginPoint : MonoBehaviour
{
    public Transform connectedTangent;

    private void OnDrawGizmos()
    {
        connectedTangent.position = transform.position + (transform.forward * transform.localScale.z);
    }
}
