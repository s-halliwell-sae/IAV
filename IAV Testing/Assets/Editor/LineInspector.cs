using UnityEngine;
using UnityEditor;

using System.Collections;

// this was created following the guide @ http://catlikecoding.com/unity/tutorials/curves-and-splines/ 

[CustomEditor(typeof(Curves))]

public class LineInspector : Editor{

    private void OnSceneGUI ()
    {
        Curves curve = target as Curves;
        Transform handleTransform = curve.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;
        Vector3 p0 = handleTransform.TransformPoint(curve.p0);
        Vector3 p1 = handleTransform.TransformPoint(curve.p1);


        Handles.color = Color.white;
        Handles.DrawLine(p0, p1);
        
       

        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.p0 = handleTransform.InverseTransformPoint(p0);
        }

        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.p1 = handleTransform.InverseTransformPoint(p1);
        }

    }


}
