using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class BezierSpline : MonoBehaviour
{
    [SerializeField] private GameObject moveObject;
    [SerializeField] private List<BezierCurve> beziers;
    [SerializeField] private BezierCurve bezierPrefab;
    [SerializeField] private List<Color> curveColors;
    private List<Color> _remainingColors = new();
    private Color _lastColor;
    private int _currentSplineIndex = 0;
    private float _timer = 0.0f;
    
    private void Update()
    {
        if (!moveObject)
            return;
        
        _timer += Time.deltaTime;

        if (_timer > 1.0f)
        {
            _currentSplineIndex++;
            _timer = 0;

            if (_currentSplineIndex == beziers.Count)
                _currentSplineIndex = 0;
        }
        
        moveObject.transform.position = beziers[_currentSplineIndex].GetSplineLocation(_timer);
        moveObject.transform.LookAt(beziers[_currentSplineIndex].GetSplineLocation(_timer + Time.deltaTime));
    }

    public void AddBezierCurve()
    {
        BezierCurve newBezier = Instantiate(bezierPrefab);

        BezierCurve lastCurve = beziers[^1];

        //set new beziers start data
        var bezierTransform = newBezier.point1.transform;
        var lastTransform = lastCurve.point2.transform;
        
        bezierTransform.position = lastTransform.position;
        bezierTransform.localRotation = lastTransform.localRotation;
        bezierTransform.localScale = lastTransform.localScale;

        //connecting beziers to each other
        newBezier.point1.connectedPoint = lastCurve.point2;
        lastCurve.point2.connectedPoint = newBezier.point1;

        newBezier.point2.transform.position += bezierTransform.position;

        //hard set the data to avoid instantiation bugs
        newBezier.point1.SetOriginPointData();
        newBezier.point2.SetOriginPointData();
        lastCurve.point2.SetOriginPointData();

        newBezier.segments = beziers[0].segments;

        if (_remainingColors.Count == 0)
        {
            _remainingColors = new(curveColors);
        }

        int index = Random.Range(0, curveColors.Count);

        newBezier.gizmoColor = curveColors[index];
        newBezier.point1.gizmoColor = curveColors[index];
        newBezier.point2.gizmoColor = curveColors[index];

        if(_lastColor != new Color())
            curveColors.Add(_lastColor);

        _lastColor = curveColors[index];
        curveColors.RemoveAt(index);
        
        beziers.Add(newBezier);
    }
}


[CustomEditor(typeof(BezierSpline))]
public class MyScriptEditor : Editor
{


    public override void OnInspectorGUI()
    {
        var myScript = target as BezierSpline;

        if(GUILayout.Button("Add Bezier Curve") && myScript)
        {
            myScript.AddBezierCurve();
        }

        DrawDefaultInspector();
    }
}
