using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{
    public OriginPoint point1;
    public OriginPoint point2;
    public uint segments;
    public Color gizmoColor = Color.white;

    private Matrix4x4 bezierMatrix = new(new(1, -3, 3, -1), new(0, 3, -6, 3), new(0, 0, 3, -3), new(0, 0, 0, 1));
    [Range(0, 1)] private float time;

    private void OnEnable()
    {
        point2.isEnd = true;
        point1.isEnd = false;
    }

    public Vector3[] GenerateCurve(Transform startPoint, Vector3 startTangent, Transform endPoint, Vector3 endTangent, uint segments)
    {
        /*
            P(t) = [1, t, t^2, t^3] * bezierMatrix * [pos1, pos1a, pos2a, pos2] 

         */

        List<Vector3> lineSegmentPositions = new();

        for(int i = 0; i < segments; i++)
        {
            float t = i / (float)segments;
            Vector4 timeMatrix = new(1, t, Mathf.Pow(t, 2), Mathf.Pow(t, 3));

            Vector3 position = startPoint.position + t * (-3 * startPoint.position + 3 * startTangent)
                + Mathf.Pow(t, 2) * (3 * startPoint.position - 6 * startTangent + 3 * endTangent)
                + Mathf.Pow(t, 3) * (-startPoint.position + 3 * startTangent - 3 * endTangent + endPoint.position);

            lineSegmentPositions.Add(position);
        }

        lineSegmentPositions.Add(endPoint.position);

        return lineSegmentPositions.ToArray();
    }

    public Vector3 GetSplineLocation(float time)
    {
        var transform1 = point1.transform;
        var startPoint = transform1.position;
        var startTangent = point1.connectedTangent.position;
        var endTangent = point2.connectedTangent.position;
        var endPoint = point2.transform.position;

        return startPoint + time * (-3 * startPoint + 3 * startTangent)
                          + Mathf.Pow(time, 2) * (3 * startPoint - 6 * startTangent + 3 * endTangent)
                          + Mathf.Pow(time, 3) * (-startPoint + 3 * startTangent - 3 * endTangent + endPoint);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Vector3[] linePositions = GenerateCurve(point1.transform, point1.connectedTangent.position, 
            point2.transform, point2.connectedTangent.position, segments);

        for(int i = 0; i < linePositions.Length - 1; i++)
        {
            Gizmos.DrawLine(linePositions[i], linePositions[i + 1]);
        }
    }


}
