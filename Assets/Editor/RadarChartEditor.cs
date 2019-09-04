using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RadarChart), true)]
[CanEditMultipleObjects]
public class RadarChartEditor : UnityEditor.UI.ImageEditor
{
    SerializedProperty _pointCount;
    SerializedProperty _pointSprite;
    SerializedProperty _pointColor;
    SerializedProperty _pointSize;
    SerializedProperty _handlerRatio;

    protected override void OnEnable()
    {
        base.OnEnable();
        _pointCount = serializedObject.FindProperty("_pointCount");
        _pointSprite = serializedObject.FindProperty("_pointSprite");
        _pointColor = serializedObject.FindProperty("_pointColor");
        _pointSize = serializedObject.FindProperty("_pointSize");
        _handlerRatio = serializedObject.FindProperty("_handlerRatio");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        
        EditorGUILayout.PropertyField(_pointCount);
        EditorGUILayout.PropertyField(_pointSprite);
        EditorGUILayout.PropertyField(_pointColor);
        EditorGUILayout.PropertyField(_pointSize);
        EditorGUILayout.PropertyField(_handlerRatio,true);

        RadarChart radar = target as RadarChart;
        if (radar != null)
        {
            if (GUILayout.Button("生成雷达图顶点"))
            {
                radar.InitPoints();
            }

            if (GUILayout.Button("生成内部可操作顶点"))
            {
                radar.InitHandlers();
            }
        }
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

    }
}
