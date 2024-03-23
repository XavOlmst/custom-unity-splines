using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OriginPoint : MonoBehaviour
{
    public Transform connectedTangent;
    public OriginPoint connectedPoint;
    public bool isEnd;
    public Color gizmoColor = Color.white;

    private void OnEnable()
    {
        SetOriginPointData();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        if(!connectedPoint || !isEnd)
            Gizmos.DrawSphere(transform.position, 0.5f);

        Gizmos.DrawSphere(connectedTangent.position, 0.25f);
    }

    private void OnDrawGizmosSelected()
    {
        SetOriginPointData();   
    }

    public void SetOriginPointData()
    {
        var ourTransform = transform;
        
        if (connectedPoint)
        {
            var connectedPointTransform = connectedPoint.transform;

            connectedPointTransform.position = ourTransform.position;
            connectedPointTransform.localRotation = ourTransform.localRotation;
            connectedPointTransform.localScale = ourTransform.localScale;


            connectedPoint.connectedTangent.position = isEnd
                ? transform.position - (ourTransform.forward * ourTransform.localScale.z)
                : transform.position + (ourTransform.forward * ourTransform.localScale.z);

        }

        connectedTangent.position = isEnd
            ? transform.position + (ourTransform.forward * ourTransform.localScale.z)
            : transform.position - (ourTransform.forward * ourTransform.localScale.z);
    }
}
