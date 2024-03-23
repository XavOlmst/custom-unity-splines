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
    [Range(0, 1)] private float _time;

    private void OnEnable()
    {
        point2.isEnd = true;
        point1.isEnd = false;
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
    
    private List<Vector3> GenerateCurve(uint numsSegments)
    {
        List<Vector3> lineSegmentPositions = new();

        for(int i = 0; i < numsSegments; i++)
        {
            float t = i / (float)numsSegments;

            Vector3 position = GetSplineLocation(t);
            lineSegmentPositions.Add(position);
        }

        return lineSegmentPositions;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        List<Vector3> linePositions = GenerateCurve(segments);

        linePositions.Add(point2.transform.position);
        
        for(int i = 0; i < linePositions.Count - 1; i++)
        {
            Gizmos.DrawLine(linePositions[i], linePositions[i + 1]);
        }
        
        Gizmos.DrawLine(linePositions[^1], point2.transform.position);
    }
}
