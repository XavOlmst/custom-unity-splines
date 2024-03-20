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

    private IEnumerator Co_FrameDelay()
    {
        yield return new WaitForEndOfFrame();
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
        if (connectedPoint)
        {
            connectedPoint.transform.position = transform.position;
            connectedPoint.transform.localRotation = transform.localRotation;
            connectedPoint.transform.localScale = transform.localScale;


            connectedPoint.connectedTangent.position = isEnd
                ? transform.position - (transform.forward * transform.localScale.z)
                : transform.position + (transform.forward * transform.localScale.z);

        }

        connectedTangent.position = isEnd
            ? transform.position + (transform.forward * transform.localScale.z)
            : transform.position - (transform.forward * transform.localScale.z);
    }
}
