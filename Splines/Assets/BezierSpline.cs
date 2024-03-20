using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    [SerializeField] private List<BezierCurve> beziers;
    [SerializeField] private BezierCurve bezierPrefab;
    [SerializeField] private List<Color> curveColors;
    private List<Color> remainingColors;

    public void AddBezierCurve()
    {
        BezierCurve newBezier = Instantiate(bezierPrefab);

        BezierCurve lastCurve = beziers[beziers.Count - 1];

        //set new beziers start data
        newBezier.point1.transform.position = lastCurve.point2.transform.position;
        newBezier.point1.transform.localRotation = lastCurve.point2.transform.localRotation;
        newBezier.point1.transform.localScale = lastCurve.point2.transform.localScale;

        //connecting beziers to each other
        newBezier.point1.connectedPoint = lastCurve.point2;
        lastCurve.point2.connectedPoint = newBezier.point1;

        newBezier.point2.transform.position += newBezier.point1.transform.position;

        newBezier.point1.SetOriginPointData();
        newBezier.point2.SetOriginPointData();
        lastCurve.point2.SetOriginPointData();

        newBezier.segments = beziers[0].segments;

        if (remainingColors.Count == 0)
        {
            remainingColors = new(curveColors);
        }

        int index = Random.Range(0, remainingColors.Count);

        newBezier.gizmoColor = remainingColors[index];
        newBezier.point1.gizmoColor = remainingColors[index];
        newBezier.point2.gizmoColor = remainingColors[index];

        remainingColors.RemoveAt(index);

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
