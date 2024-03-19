using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    [SerializeField] private List<BezierCurve> beziers;
    [SerializeField] private BezierCurve bezierPrefab;

    public void AddBezierCurve()
    {
        // last element: beziers[beziers.Count - 1];

        BezierCurve newBezier = Instantiate(bezierPrefab);

        BezierCurve lastCurve = beziers[beziers.Count - 1];

        newBezier.point1 = lastCurve.point2;
        newBezier.point2.transform.position += newBezier.point1.transform.position;

        newBezier.segments = beziers[0].segments;

        beziers.Add(newBezier);
    }
}


[CustomEditor(typeof(BezierSpline))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as BezierSpline;

        if(GUILayout.Button("Add Bezier Curve"))
        {
            myScript.AddBezierCurve();
        }

        DrawDefaultInspector();
    }
}
